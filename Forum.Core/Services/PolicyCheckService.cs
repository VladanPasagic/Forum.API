using Forum.Core.Services.Interfaces;
using Forum.EF.Entities;

namespace Forum.Core.Services;

public class PolicyCheckService : IPolicyCheckService
{
    public bool CheckPolicy(int categoryId, RequestType type, string policy)
    {
        policy = policy.Trim();
        string[] policies = policy.Split(" ");
        foreach (var polic in policies)
        {
            string[] parts = polic.Split(":");
            if (int.Parse(parts[0]) == categoryId && parts[1].Equals(type.ToString()))
                return true;
        }
        return false;
    }
}
