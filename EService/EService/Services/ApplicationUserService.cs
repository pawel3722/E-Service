using EService.Dtos.MessageDtos;
using EService.Dtos.RolesDtos;
using EService.Models;
using EService.Repositories.Interfaces;
using EService.Services.Interfaces;
using System.Security.Claims;

namespace EService.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        public ApplicationUserService(IApplicationUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }
        public async Task<List<ApplicationUser>> GetAllUsersAsync()
        {
            return await _applicationUserRepository.GetAllUsersAsync();
        }
        public async Task<ApplicationUser?> GetUserAsync(int id)
        {
            return await _applicationUserRepository.GetUserByIdAsync(id);
        }
        public async Task<(bool Confirmed, string Response)> AddUserRolesAsync(UpdateRolesDto request, int id)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                List<Role> roles = new List<Role>();
                foreach(var roleName in request.RoleNames)
                {
                    var role = await _applicationUserRepository.GetRoleByNameAsync(roleName);
                    if (role == null) return await Task.FromResult((false, "Role with given name does not exist."));
                    user.Roles.Add(role);
                    role.Users.Add(user);
                }
                await _applicationUserRepository.SaveChangesAsync();
                return await Task.FromResult((true, "User's roles successfully added."));
            }
            else return await Task.FromResult((false, "User with given id does not exist."));
        }
        public async Task<(bool Confirmed, string Response)> RemoveUserRolesAsync(UpdateRolesDto request, int id)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                List<Role> roles = new List<Role>();
                foreach (var roleName in request.RoleNames)
                {
                    var role = await _applicationUserRepository.GetRoleByNameAsync(roleName);
                    if (role == null) return await Task.FromResult((false, "Role with given name does not exist."));
                    if (user.Roles.Contains(role)) user.Roles.Remove(role);
                    else return await Task.FromResult((false, "Role is not performed by this user."));
                    if (role.Users.Contains(user)) role.Users.Remove(user);
                    else return await Task.FromResult((false, "User does not perform this role."));
                }
                await _applicationUserRepository.SaveChangesAsync();
                return await Task.FromResult((true, "User's roles successfully removed."));
            }
            else return await Task.FromResult((false, "User with given id does not exist."));
        }

        public async Task<(bool Confirmed, string Response)> DeleteUserAsync(int id)
        {
            var user = await _applicationUserRepository.GetUserByIdAsync(id);
            if (user != null)
            {
                await _applicationUserRepository.RemoveUserAsync(user);
                return await Task.FromResult((true, "User successfully deleted."));
            }
            else return await Task.FromResult((false, "User with given id does not exist."));
        }
    }
}
