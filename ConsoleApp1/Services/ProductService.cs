using System.Collections.ObjectModel;
using ConsoleApp1.IRepository;
using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public class ProductService : IProductService
    {
        private readonly IMenuRepository _repository;

        public ProductService(IMenuRepository repository)
        {
            _repository = repository;
        }

        public async Task<ObservableCollection<Menu>> GetProductsAsync()
        {
            var products = await _repository.GetAllMenusAsync();
            return new ObservableCollection<Menu>(products);
        }

        public async Task<bool> AddProductAsync(Menu product)
        {
            try
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(product.Name))
                    return false;

                if (product.Price <= 0)
                    return false;

                await _repository.CreateMenuAsync(product);
                return true;
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Ошибка добавления продукта: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                return await _repository.DeleteMenuAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка удаления продукта: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateProductAsync(Menu product)
        {
            try
            {
                // Валидация
                if (string.IsNullOrWhiteSpace(product.Name))
                    return false;

                if (product.Price <= 0)
                    return false;

                return await _repository.UpdateMenuAsync(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обновления продукта: {ex.Message}");
                return false;
            }
        }

        public async Task<Menu?> GetProductByIdAsync(int id)
        {
            try
            {
                return await _repository.GetMenuByIdAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения продукта: {ex.Message}");
                return null;
            }
        }

        public async Task<List<Menu>> SearchProductsAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return await _repository.GetAllMenusAsync();

                return await _repository.SearchMenusByNameAsync(searchTerm);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка поиска продуктов: {ex.Message}");
                return new List<Menu>();
            }
        }

        public async Task<List<Menu>> GetProductsByCategoryAsync(int categoryId)
        {
            try
            {
                return await _repository.GetMenusByCategoryAsync(categoryId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка получения продуктов по категории: {ex.Message}");
                return new List<Menu>();
            }
        }
    }
}