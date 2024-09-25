namespace Forum.SIEM.EF.Entities;

public class LogEntry
{
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public string? Source { get; set; }
    public string? EventType { get; set; }
    public string? Message { get; set; }
    public string? Severity { get; set; }
}
