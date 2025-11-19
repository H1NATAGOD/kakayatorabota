using ConsoleApp1.Models;
using PersonalRole = ConsoleApp1.enums.PersonalRole;

namespace ConsoleApp1.Services
{
    public interface IPersonalService
    {
        Task<bool> RegisterPersonalAsync(Personal personal);
        Task<bool> UpdatePersonalAsync(Personal personal);
        Task<bool> ChangePersonalStatusAsync(int personalId, bool isActive);
        Task<bool> ChangePasswordAsync(int personalId, string newPassword);
        Task<Personal?> AuthenticateAsync(string login, string password);
        Task<Personal?> GetPersonalByIdAsync(int id);
        Task<List<Personal>> GetAllPersonalAsync();
        Task<List<Personal>> GetActivePersonalAsync();
        Task<List<Personal>> GetPersonalByRoleAsync(PersonalRole role);
        Task<List<Personal>> GetWaitersForShiftAsync();
        Task<List<Personal>> GetCooksForShiftAsync();
        Task<bool> IsLoginUniqueAsync(string login, int? excludePersonalId = null);
    }
}