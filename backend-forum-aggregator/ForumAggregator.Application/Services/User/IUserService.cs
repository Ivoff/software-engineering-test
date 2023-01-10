namespace ForumAggregator.Application.Services;

using System.Collections.Generic;

public interface IUserService
{
    public bool UserEmailExist(string email);
    public UserAppServiceModel? GetUser(Guid id);
    public UserAppServiceModel? GetUser(string email);
    public ICollection<UserAppServiceModel> GetAll();
    public UserAppServiceModel? GetUserByName(string name);
    public ServiceResult UpdateUser(UserAppServiceModel user);
    public ServiceResult DeleteUser(Guid id);
}