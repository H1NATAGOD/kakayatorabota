using System.Collections.ObjectModel;
using ConsoleApp1.Models;

namespace ConsoleApp1.Services
{
    public interface IProductService
    {
        Task<ObservableCollection<Menu>> GetProductsAsync();
        Task<bool> AddProductAsync(Menu product);
        Task<bool> DeleteProductAsync(int id);
        Task<bool> UpdateProductAsync(Menu product);
        Task<Menu?> GetProductByIdAsync(int id);
        Task<List<Menu>> SearchProductsAsync(string searchTerm);
        Task<List<Menu>> GetProductsByCategoryAsync(int categoryId);
    }
}