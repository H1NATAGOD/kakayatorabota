public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<List<Product>> GetByCategoryAsync(string category);
    Task<List<Product>> GetActiveAsync();
    Task<int> AddAsync(Product product);
    Task<bool> UpdateAsync(Product product);
    Task<bool> DeleteAsync(int id);
    Task<bool> SoftDeleteAsync(int id);
    Task<bool> RestoreAsync(int id);
    Task<bool> UpdatePriceAsync(int id, decimal newPrice);
    Task<List<Product>> SearchByNameAsync(string searchTerm);
    Task<List<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    Task<List<string>> GetCategoriesAsync();
    Task<bool> ExistsAsync(int id);
    Task<bool> IsNameUniqueAsync(string name, int? excludeId = null);
    Task<int> GetTotalCountAsync();
    Task<ProductStatistics> GetStatisticsAsync();
    Task<List<Product>> GetRecentlyAddedAsync(int days = 7);
    Task<bool> BulkUpdatePricesAsync(Dictionary<int, decimal> priceUpdates);
}