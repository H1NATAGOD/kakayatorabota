using ConsoleApp1.Models;
using ConsoleApp1.IRepository;
using MenuStatistics = ConsoleApp1.Models.MenuStatistics;

namespace ConsoleApp1.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        
        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }
        
        public async Task<List<Menu>> GetMenuByCategoriesAsync()
        {
            var categories = await _menuRepository.GetCategoriesWithMenusAsync();
            return categories.SelectMany(c => c.Menus).ToList();
        }
        
        public async Task<bool> UpdateMenuPriceAsync(int menuId, decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("Цена должна быть положительной");
                
            return await _menuRepository.UpdateMenuPriceAsync(menuId, newPrice);
        }

        public async Task<List<Menu>> GetAllMenusAsync() 
        {
            return await _menuRepository.GetAllMenusAsync();
        }
        
        public async Task<List<Menu>> GetActiveMenusAsync() 
        {
            return await _menuRepository.GetActiveMenusAsync();
        }
        
        public async Task<Menu?> GetMenuByIdAsync(int id) 
        {
            return await _menuRepository.GetMenuByIdAsync(id);
        }
        
        public async Task<List<Menu>> GetMenusByCategoryAsync(int categoryId) 
        {
            return await _menuRepository.GetMenusByCategoryAsync(categoryId);
        }
        
        public async Task<List<Menu>> GetMenusByCategoryNameAsync(string categoryName) 
        {
            return await _menuRepository.GetMenusByCategoryNameAsync(categoryName);
        }
        
        public async Task<List<Category>> GetAllCategoriesAsync() 
        {
            return await _menuRepository.GetAllCategoriesAsync();
        }
        
        public async Task<List<Category>> GetCategoriesWithMenusAsync() 
        {
            return await _menuRepository.GetCategoriesWithMenusAsync();
        }
        
        public async Task<int> CreateMenuAsync(Menu menu) 
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(menu.Name))
                throw new ArgumentException("Название меню не может быть пустым");
                
            if (menu.Price <= 0)
                throw new ArgumentException("Цена должна быть положительной");
                
            return await _menuRepository.CreateMenuAsync(menu);
        }
        
        public async Task<bool> UpdateMenuAsync(Menu menu) 
        {
            // Валидация
            if (string.IsNullOrWhiteSpace(menu.Name))
                throw new ArgumentException("Название меню не может быть пустым");
                
            if (menu.Price <= 0)
                throw new ArgumentException("Цена должна быть положительной");
                
            return await _menuRepository.UpdateMenuAsync(menu);
        }
        
        public async Task<bool> DeleteMenuAsync(int menuId) 
        {
            return await _menuRepository.DeleteMenuAsync(menuId);
        }
        
        public async Task<bool> AddMenuToCategoryAsync(int menuId, int categoryId) 
        {
            return await _menuRepository.AddMenuToCategoryAsync(menuId, categoryId);
        }
        
        public async Task<bool> RemoveMenuFromCategoryAsync(int menuId, int categoryId) 
        {
            return await _menuRepository.RemoveMenuFromCategoryAsync(menuId, categoryId);
        }
        
        public async Task<List<Menu>> SearchMenusAsync(string searchTerm) 
        {
            return await _menuRepository.SearchMenusByNameAsync(searchTerm);
        }
        
        public async Task<MenuStatistics> GetMenuStatisticsAsync() 
        {
            var repoStats = await _menuRepository.GetMenuStatisticsAsync();
            
            // Преобразуем из IRepository.MenuStatistics в Models.MenuStatistics
            return new MenuStatistics
            {
                TotalMenus = repoStats.TotalMenus,
                ActiveMenus = repoStats.ActiveMenus,
                TotalCategories = repoStats.TotalCategories,
                AveragePrice = repoStats.AveragePrice,
                MinPrice = repoStats.MinPrice,
                MaxPrice = repoStats.MaxPrice,
                MostExpensiveMenu = repoStats.MostExpensiveMenu,
                LeastExpensiveMenu = repoStats.LeastExpensiveMenu
            };
        }
    }
}