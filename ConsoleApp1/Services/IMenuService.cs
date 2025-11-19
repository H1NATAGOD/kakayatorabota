using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public interface IMenuService
    {
        Task<List<Menu>> GetAllMenusAsync();
        Task<List<Menu>> GetActiveMenusAsync();
        Task<Menu?> GetMenuByIdAsync(int id);
        Task<List<Menu>> GetMenusByCategoryAsync(int categoryId);
        Task<List<Menu>> GetMenusByCategoryNameAsync(string categoryName);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<List<Category>> GetCategoriesWithMenusAsync();
        Task<List<Menu>> GetMenuByCategoriesAsync();
        Task<int> CreateMenuAsync(Menu menu);
        Task<bool> UpdateMenuAsync(Menu menu);
        Task<bool> UpdateMenuPriceAsync(int menuId, decimal newPrice);
        Task<bool> DeleteMenuAsync(int menuId);
        Task<bool> AddMenuToCategoryAsync(int menuId, int categoryId);
        Task<bool> RemoveMenuFromCategoryAsync(int menuId, int categoryId);
        Task<List<Menu>> SearchMenusAsync(string searchTerm);
        Task<MenuStatistics> GetMenuStatisticsAsync();
    }
}