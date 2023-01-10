namespace ForumAggregator.Application.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using AutoMapper;
using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Domain.UserRegistry;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;
    private readonly Domain.Services.IUserService _domainUserService;
    private readonly IAppContext _appContext;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository, 
        IMapper mapper, 
        IPasswordService passwordService,
        Domain.Services.IUserService domainUserService,
        IHttpContextAccessor httpContext,
        IAppContext appContext,
        ILogger<UserService> logger
    )
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _passwordService = passwordService;
        _domainUserService = domainUserService;
        _appContext = appContext;
        _logger = logger;
    }

    public bool UserEmailExist(string email)
    {
        User? userExit = _userRepository.Get(email);
        return userExit != null;
    }

    public UserAppServiceModel? GetUser(Guid id)
    {
        User? user = _userRepository.Get(id);
        
        if (user == null)
            return null;
        
        return _mapper.Map<UserAppServiceModel>(user);
    }

    public UserAppServiceModel? GetUser(string email)
    {
        User? user = _userRepository.Get(email);
        
        if (user == null)
            return null;
        
        return _mapper.Map<UserAppServiceModel>(user);
    }

    public ICollection<UserAppServiceModel> GetAll()
    {
        return _userRepository.GetAll().Select(
            user => _mapper.Map<UserAppServiceModel>(user)
        ).ToList();
    }

    public UserAppServiceModel? GetUserByName(string name)
    {
        User? user = _userRepository.GetByName(name);
        if (user == null)
            return null;
            
        return _mapper.Map<UserAppServiceModel>(user);
    }

    public ServiceResult UpdateUser(UserAppServiceModel user)
    {
        _logger.LogCritical(_appContext.UserId.ToString());
        _logger.LogCritical(user.Id.ToString());

        User? trackedishUser = _userRepository.Get(user.Id);
        if (trackedishUser == null)
        {
            return new ServiceResult(false, "User does not exist.");
        }
        
        UserResult result;

        if (!string.IsNullOrWhiteSpace(user.Name))
        {
            if (!_domainUserService.IsUserNameUnique(user.Name))
                return new ServiceResult(false, "UserName already taken.");

            result = trackedishUser.EditName(_appContext.UserId, user.Name);

            if (result.Value == false)
                return new ServiceResult(false, result.Result);
        }
        
        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            result = trackedishUser.EditEmail(_appContext.UserId, user.Email);
            if (result.Value == false)
                return new ServiceResult(false, result.Result);
        }
        
        byte[]? salt = null;
        if (!string.IsNullOrWhiteSpace(user.Password))
        {
            string newPassword = _passwordService.HashPassword(user.Password, out salt);
            result = trackedishUser.EditPassword(_appContext.UserId, newPassword);
            if (result.Value == false)
                return new ServiceResult(false, result.Result);
        }

        bool saveResult = salt == null ? 
            _userRepository.Save(trackedishUser) : 
            _userRepository.Save(trackedishUser, salt);

        return new ServiceResult(
            saveResult, 
            saveResult ? string.Empty : "Something wrong happened during data persistance"
        );
    }

    public ServiceResult DeleteUser(Guid userId)
    {
        User? trackedishUser = _userRepository.Get(userId);
        if (trackedishUser == null)
        {
            return new ServiceResult(false, "User does not exist.");
        }

        var deletionResult = trackedishUser.Delete(_appContext.UserId);
        if (deletionResult.Value == false)
        {
            return new ServiceResult(false, deletionResult.Result);
        }

        bool saveResult = _userRepository.Save(trackedishUser);

        return new ServiceResult(
            saveResult,
            saveResult ? string.Empty : "Something wrong happened during data persistance"
        );
    }
}