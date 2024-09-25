namespace Forum.SIEM.Core.Requests;

public class LogEntryRequest
{
    public string Source { get; set; }
    public string EventType { get; set; }
    public string Message { get; set; }
    public string Severity { get; set; }
}
