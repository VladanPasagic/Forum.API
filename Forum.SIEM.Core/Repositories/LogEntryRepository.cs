using Forum.SIEM.Core.Repositories.Interfaces;
using Forum.SIEM.EF;
using Forum.SIEM.EF.Entities;
using Microsoft.EntityFrameworkCore;

namespace Forum.SIEM.Core.Repositories
{
    public class LogEntryRepository : ILogEntryRepository
    {
        private readonly SiemContext _context;

        public LogEntryRepository(SiemContext context)
        {
            _context = context;
        }

        public async Task Add(LogEntry entry)
        {
            await _context.LogEntries.AddAsync(entry);
        }

        public async Task<List<LogEntry>> GetAll()
        {
            return await _context.LogEntries.ToListAsync();
        }
    }
}
