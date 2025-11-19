using ConsoleApp1.Models;
using PersonalRole = ConsoleApp1.enums.PersonalRole;

namespace ConsoleApp1.IRepository
{
    public interface IPersonalRepository
    {
        Task<Personal?> GetPersonalByLoginAsync(string login);
        Task<Personal?> GetPersonalByIdAsync(int id);
        Task<List<Personal>> GetAllPersonalAsync();
        Task<List<Personal>> GetActivePersonalAsync();
        Task<List<Personal>> GetPersonalByRoleAsync(PersonalRole role);
        Task<List<Personal>> GetPersonalByRoleAndStatusAsync(PersonalRole role, bool isActive);
        Task<int> CreatePersonalAsync(Personal personal);
        Task<bool> UpdatePersonalAsync(Personal personal);
        Task<bool> UpdatePersonalStatusAsync(int personalId, bool isActive);
        Task<bool> UpdatePersonalPasswordAsync(int personalId, string newPasswordHash);
        Task<bool> IsLoginExistsAsync(string login, int? excludePersonalId = null);
        Task<List<Personal>> SearchPersonalByNameAsync(string searchTerm);
    }
}