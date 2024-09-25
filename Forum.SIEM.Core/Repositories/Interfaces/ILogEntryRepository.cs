using Forum.SIEM.EF.Entities;

namespace Forum.SIEM.Core.Repositories.Interfaces;

public interface ILogEntryRepository
{
    Task<List<LogEntry>> GetAll();

    Task Add(LogEntry entry);
}
