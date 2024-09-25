using AutoMapper;
using Forum.SIEM.Core.Repositories.Interfaces;
using Forum.SIEM.Core.Requests;
using Forum.SIEM.Core.Services.Interfaces;
using Forum.SIEM.EF.Entities;

namespace Forum.SIEM.Core.Services;

public class LogEntryService : ILogEntryService
{
    private ILogEntryRepository _logEntryRepository;
    private IMapper _mapper;

    public LogEntryService(ILogEntryRepository logEntryRepository, IMapper mapper)
    {
        _logEntryRepository = logEntryRepository;
        _mapper = mapper;
    }

    public async Task LogEntry(LogEntryRequest logEntryRequest)
    {
        await _logEntryRepository.Add(_mapper.Map<LogEntry>(logEntryRequest));
    }
}
