using CompressMedia.DTOs;
using CompressMedia.Models;

namespace CompressMedia.Repositories.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUser(Guid? tenantId);
        string GetUserNameLoggedIn();
        Task<User> GetUserByName(string username);
        Task<User> EditProfile(UserDto userDto);
    }
}
