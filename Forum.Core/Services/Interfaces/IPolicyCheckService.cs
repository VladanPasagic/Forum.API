using Forum.EF.Entities;

namespace Forum.Core.Services.Interfaces;

public interface IPolicyCheckService
{
    bool CheckPolicy(int categoryId, RequestType type, string policy);
}
