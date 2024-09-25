using AutoMapper;
using Forum.Core.Repositories.Interfaces;
using Forum.Core.Requests;
using Forum.Core.Responses;
using Forum.Core.Services.Interfaces;
using Forum.EF.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Forum.Core.Services;

public class UserService : GenericService<IUserRepository, UserService, SingleUserRequest, UserResponse, User, string>, IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IPermissionRepository _permissionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IEmailService _emailService;

    public UserService(IUserRepository repository, ILogger<UserService> logger, IMapper mapper, UserManager<User> userManager, IPermissionRepository permissionRepository, ICategoryRepository categoryRepository, IEmailService emailService) : base(repository, logger, mapper)
    {
        _userManager = userManager;
        _permissionRepository = permissionRepository;
        _categoryRepository = categoryRepository;
        _emailService = emailService;
    }

    public async Task ApproveUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id) ?? throw new InvalidOperationException();
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _userManager.ConfirmEmailAsync(user, code);
        await _userManager.SetTwoFactorEnabledAsync(user, true);
        user.IsHandled = true;
        await _emailService.SendEmail(user.Email!, "Approved request", "Your requests to join the forum has been approved");
        await _userManager.UpdateAsync(user);
    }

    public async Task DenyUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id) ?? throw new InvalidOperationException();
        user.IsHandled = true;
        await _emailService.SendEmail(user.Email!, "Denied request", "Your requests to join the forum has been denied");
        await _userManager.UpdateAsync(user);
    }

    public async Task<IEnumerable<UserResponse>> GetRequests()
    {
        return _mapper.Map<IEnumerable<UserResponse>>(await _repository.GetRequests());
    }

    public async Task<SingleUserResponse> GetSingle(string id)
    {
        var user = await _repository.GetById(id);
        if (user == null)
        {
            return new SingleUserResponse() { Success = false, Message = "User not found" };
        }
        var userResponse = _mapper.Map<SingleUserResponse>(user);
        userResponse.Role = (await _userManager.GetRolesAsync(user)).First();
        userResponse.Success = true;
        var categories = (await _categoryRepository.GetAllAsync()).ToList();
        var claims = await _userManager.GetClaimsAsync(user);
        var permission = await _permissionRepository.GetAll();
        foreach (var claim in claims)
        {
            int index = int.Parse(claim.Type);
            userResponse.Permissions.Add(new PermissionResponse() { Id = permission.Where(p => p.CategoryId == index && p.RequestType == claim.Value.ToString()).First().Id, CategoryId = index, RequestType = claim.Value, CategoryName = categories[index - 1].Name });
        }
        return userResponse;
    }

    public async Task<IEnumerable<UserResponse>> GetUsers()
    {
        var users = (await _repository.GetUsers()).ToList();
        var userResponses = _mapper.Map<IEnumerable<UserResponse>>(users).ToList();
        for (int i = 0; i < users.Count; i++)
        {
            userResponses[i].Role = (await _userManager.GetRolesAsync(users[i])).First();
        }
        return userResponses;
    }

    public override Task Update(string id, SingleUserRequest entity)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateUserPermissions(string id, SingleUserRequest request)
    {
        var user = await _userManager.FindByIdAsync(id) ?? throw new Exception();
        var userClaims = await _userManager.GetClaimsAsync(user);
        await _userManager.RemoveClaimsAsync(user, userClaims);
        var userRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, userRoles);
        await _userManager.AddToRoleAsync(user, request.Role.Normalize());
        var permissions = (await _permissionRepository.GetAllAsync()).ToList();
        foreach (var claim in request.Permissions)
        {
            await _userManager.AddClaimAsync(user, new System.Security.Claims.Claim(permissions[claim.Id].CategoryId.ToString(), permissions[claim.Id].RequestType));
        }
    }
}
