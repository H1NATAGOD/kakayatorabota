using ConsoleApp1.Models;

namespace ConsoleApp1.IRepository
{
    public interface IMenuRepository
    {
        // Основные операции с меню
        Task<List<Menu>> GetAllMenusAsync();
        Task<List<Menu>> GetActiveMenusAsync();
        Task<Menu?> GetMenuByIdAsync(int id);
        Task<int> CreateMenuAsync(Menu menu);
        Task<bool> UpdateMenuAsync(Menu menu);
        Task<bool> DeleteMenuAsync(int id);
        Task<bool> UpdateMenuPriceAsync(int menuId, decimal newPrice);
        Task<List<Menu>> SearchMenusByNameAsync(string searchTerm);
        
        // Операции с категориями
        Task<List<Category>> GetAllCategoriesAsync();
        Task<List<Category>> GetCategoriesWithMenusAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<int> CreateCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int id);
        
        // Связи меню-категории
        Task<List<Menu>> GetMenusByCategoryAsync(int categoryId);
        Task<List<Menu>> GetMenusByCategoryNameAsync(string categoryName);
        Task<bool> AddMenuToCategoryAsync(int menuId, int categoryId);
        Task<bool> RemoveMenuFromCategoryAsync(int menuId, int categoryId);
        
        // Дополнительные методы
        Task<MenuStatistics> GetMenuStatisticsAsync();
        Task<List<Menu>> GetMenusByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<bool> IsMenuNameUniqueAsync(string name, int? excludeId = null);
    }

    public class MenuStatistics
    {
        public int TotalMenus { get; set; }
        public int ActiveMenus { get; set; }
        public int TotalCategories { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public Menu? MostExpensiveMenu { get; set; }
        public Menu? LeastExpensiveMenu { get; set; }
    }
}