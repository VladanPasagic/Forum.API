using Forum.SIEM.Core.Services;
using Forum.SIEM.Core.Requests;
using Forum.SIEM.Core.Services.Interfaces;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using System.Collections.Concurrent;

namespace Forum.API.Middleware;

public class WAFMiddlerware
{
    private readonly RequestDelegate _next;
    private readonly TimeSpan _timespan = TimeSpan.FromMinutes(10);
    private readonly int _maxRequests = 100;
    private readonly ConcurrentDictionary<string, RequestCounter> _requestCounters = new();

    public WAFMiddlerware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogEntryService logEntryService)
    {
        if (IsSqlInjection(context.Request))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await logEntryService.LogEntry(new LogEntryRequest()
            {
                Severity = "Serious",
                EventType = "SQL Injection",
                Message = $"User {context.Connection.RemoteIpAddress?.ToString()} tried an sql injection",
            });
        }

        if (IsXssAttack(context.Request))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await logEntryService.LogEntry(new LogEntryRequest()
            {
                Severity = "Serious",
                EventType = "XSS Attack",
                Message = $"User {context.Connection.RemoteIpAddress?.ToString()} tried an xss attack",
            });
        }

        if (IsRateLimited(context))
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await logEntryService.LogEntry(new LogEntryRequest()
            {
                Severity = "Warning",
                EventType = "RateLimit Exceeded",
                Message = $"User {context.Connection.RemoteIpAddress?.ToString()} did to many requests",
            });
        }

        await _next(context);
    }

    private bool IsSqlInjection(HttpRequest request)
    {
        var inputs = request.Query
            .Select(q => q.Value.ToString());

        string sqlInjectionPattern = @"'|\b(SELECT|INSERT|UPDATE|DELETE|FROM|WHERE|--|;|#|--)\b";
        return inputs.Any(input => Regex.IsMatch(input, sqlInjectionPattern, RegexOptions.IgnoreCase));
    }

    private bool IsXssAttack(HttpRequest request)
    {
        var inputs = request.Query
            .Select(q => q.Value.ToString());

        string xssPattern = @"<script.*?>|</script>|%3Cscript|%3C/script%3E|<img.*?src=.*?javascript:";
        return inputs.Any(input => Regex.IsMatch(input, xssPattern, RegexOptions.IgnoreCase));
    }

    private bool IsRateLimited(HttpContext context)
    {
        var ipAddress = context.Connection.RemoteIpAddress.ToString();
        var rc = _requestCounters.GetOrAdd(ipAddress, new RequestCounter());
        return rc.Increment(_timespan, _maxRequests);
    }
}
