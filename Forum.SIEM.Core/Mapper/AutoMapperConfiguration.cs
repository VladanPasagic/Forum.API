using AutoMapper;
using Forum.SIEM.Core.Requests;
using Forum.SIEM.EF.Entities;

namespace Forum.SIEM.Core.Mapper;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        CreateMap<LogEntryRequest, LogEntry>();
        CreateMap<LogEntry, LogEntryRequest>();
    }
}
