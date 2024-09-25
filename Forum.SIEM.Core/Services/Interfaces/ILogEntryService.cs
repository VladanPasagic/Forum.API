using Forum.SIEM.Core.Requests;

namespace Forum.SIEM.Core.Services.Interfaces;

public interface ILogEntryService
{
    public Task LogEntry(LogEntryRequest logEntryRequest);
}
