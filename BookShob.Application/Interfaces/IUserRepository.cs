using Microsoft.AspNetCore.Identity;

namespace BookShob.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(Guid id);
        Task<ApplicationUser?> GetByUsernameAsync(string username);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> AddToRoleAsync(ApplicationUser user, string role);
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);


    }
}
