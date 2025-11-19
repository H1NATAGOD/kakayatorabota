using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp1.IRepository
{
    /// <summary>
    /// Репозиторий для работы с меню (таблицы menu, category, menu_and_category)
    /// </summary>
    public interface IMenuRepository
    {
        #region Menu Operations
        
        /// <summary>
        /// Получить все блюда/напитки из меню
        /// </summary>
        Task<List<Menu>> GetAllMenusAsync();
        
        /// <summary>
        /// Получить все активные блюда/напитки
        /// </summary>
        Task<List<Menu>> GetActiveMenusAsync();
        
        /// <summary>
        /// Получить блюдо/напиток по ID
        /// </summary>
        Task<Menu?> GetMenuByIdAsync(int id);
        
        /// <summary>
        /// Получить блюда/напитки по списку ID
        /// </summary>
        Task<List<Menu>> GetMenusByIdsAsync(List<int> menuIds);
        
        /// <summary>
        /// Создать новое блюдо/напиток
        /// </summary>
        Task<int> CreateMenuAsync(Menu menu);
        
        /// <summary>
        /// Обновить блюдо/напиток
        /// </summary>
        Task<bool> UpdateMenuAsync(Menu menu);
        
        /// <summary>
        /// Обновить цену блюда/напитка
        /// </summary>
        Task<bool> UpdateMenuPriceAsync(int menuId, decimal newPrice);
        
        /// <summary>
        /// Удалить блюдо/напиток
        /// </summary>
        Task<bool> DeleteMenuAsync(int menuId);
        
        /// <summary>
        /// Поиск блюд/напитков по названию
        /// </summary>
        Task<List<Menu>> SearchMenusByNameAsync(string searchTerm);
        
        /// <summary>
        /// Получить блюда/напитки в ценовом диапазоне
        /// </summary>
        Task<List<Menu>> GetMenusByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        
        /// <summary>
        /// Проверить существование названия меню
        /// </summary>
        Task<bool> IsMenuNameExistsAsync(string name, int? excludeMenuId = null);

        #endregion

        #region Category Operations
        
        /// <summary>
        /// Получить все категории
        /// </summary>
        Task<List<Category>> GetAllCategoriesAsync();
        
        /// <summary>
        /// Получить категорию по ID
        /// </summary>
        Task<Category?> GetCategoryByIdAsync(int id);
        
        /// <summary>
        /// Получить категорию по названию
        /// </summary>
        Task<Category?> GetCategoryByNameAsync(string name);
        
        /// <summary>
        /// Создать новую категорию
        /// </summary>
        Task<int> CreateCategoryAsync(Category category);
        
        /// <summary>
        /// Обновить категорию
        /// </summary>
        Task<bool> UpdateCategoryAsync(Category category);
        
        /// <summary>
        /// Удалить категорию
        /// </summary>
        Task<bool> DeleteCategoryAsync(int categoryId);
        
        /// <summary>
        /// Проверить существование названия категории
        /// </summary>
        Task<bool> IsCategoryNameExistsAsync(string name, int? excludeCategoryId = null);

        #endregion

        #region Menu-Category Relations
        
        /// <summary>
        /// Получить блюда/напитки по категории
        /// </summary>
        Task<List<Menu>> GetMenusByCategoryAsync(int categoryId);
        
        /// <summary>
        /// Получить блюда/напитки по названию категории
        /// </summary>
        Task<List<Menu>> GetMenusByCategoryNameAsync(string categoryName);
        
        /// <summary>
        /// Получить категории для блюда/напитка
        /// </summary>
        Task<List<Category>> GetCategoriesByMenuAsync(int menuId);
        
        /// <summary>
        /// Добавить блюдо/напиток в категорию
        /// </summary>
        Task<bool> AddMenuToCategoryAsync(int menuId, int categoryId);
        
        /// <summary>
        /// Удалить блюдо/напиток из категории
        /// </summary>
        Task<bool> RemoveMenuFromCategoryAsync(int menuId, int categoryId);
        
        /// <summary>
        /// Обновить категории для блюда/напитка
        /// </summary>
        Task<bool> UpdateMenuCategoriesAsync(int menuId, List<int> categoryIds);
        
        /// <summary>
        /// Проверить связь меню-категория
        /// </summary>
        Task<bool> IsMenuInCategoryAsync(int menuId, int categoryId);

        #endregion

        #region Advanced Menu Operations
        
        /// <summary>
        /// Получить меню с категориями (полная информация)
        /// </summary>
        Task<List<Menu>> GetMenusWithCategoriesAsync();
        
        /// <summary>
        /// Получить категории с блюдами/напитками
        /// </summary>
        Task<List<Category>> GetCategoriesWithMenusAsync();
        
        /// <summary>
        /// Получить популярные блюда/напитки (по количеству заказов)
        /// </summary>
        Task<List<MenuPopularity>> GetPopularMenusAsync(int limit = 10, DateTime? startDate = null, DateTime? endDate = null);
        
        /// <summary>
        /// Получить статистику по меню
        /// </summary>
        Task<MenuStatistics> GetMenuStatisticsAsync();
        
        /// <summary>
        /// Обновить несколько цен сразу
        /// </summary>
        Task<bool> BulkUpdateMenuPricesAsync(Dictionary<int, decimal> menuPrices);

        #endregion
    }

    /// <summary>
    /// Популярность блюда/напитка
    /// </summary>
    public class MenuPopularity
    {
        public Menu Menu { get; set; } = new();
        public int OrderCount { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    /// <summary>
    /// Статистика по меню
    /// </summary>
    public class MenuStatistics
    {
        public int TotalMenus { get; set; }
        public int TotalCategories { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public Menu? MostExpensiveMenu { get; set; }
        public Menu? LeastExpensiveMenu { get; set; }
    }
}