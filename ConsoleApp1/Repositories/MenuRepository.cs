using ConsoleApp1.Models;
using Npgsql;

namespace ConsoleApp1.IRepository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly DatabaseConfig _config;

        public MenuRepository(DatabaseConfig config)
        {
            _config = config;
        }

        public async Task<List<Menu>> GetAllMenusAsync()
        {
            var menus = new List<Menu>();
            
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = "SELECT id, name, price, is_active, description, image, preparation_time, created_at FROM menu";
            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                menus.Add(new Menu
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    IsActive = reader.GetBoolean(3),
                    Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Image = reader.IsDBNull(5) ? null : reader.GetString(5),
                    PreparationTime = reader.GetInt32(6),
                    CreatedAt = reader.GetDateTime(7)
                });
            }

            return menus;
        }

        public async Task<List<Menu>> GetActiveMenusAsync()
        {
            var menus = new List<Menu>();
            
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = "SELECT id, name, price, is_active, description, image, preparation_time, created_at FROM menu WHERE is_active = true";
            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                menus.Add(new Menu
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    IsActive = reader.GetBoolean(3),
                    Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Image = reader.IsDBNull(5) ? null : reader.GetString(5),
                    PreparationTime = reader.GetInt32(6),
                    CreatedAt = reader.GetDateTime(7)
                });
            }

            return menus;
        }

        public async Task<Menu?> GetMenuByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = "SELECT id, name, price, is_active, description, image, preparation_time, created_at FROM menu WHERE id = @id";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Menu
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Price = reader.GetDecimal(2),
                    IsActive = reader.GetBoolean(3),
                    Description = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Image = reader.IsDBNull(5) ? null : reader.GetString(5),
                    PreparationTime = reader.GetInt32(6),
                    CreatedAt = reader.GetDateTime(7)
                };
            }

            return null;
        }

        public async Task<int> CreateMenuAsync(Menu menu)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                INSERT INTO menu (name, price, is_active, description, image, preparation_time, created_at) 
                VALUES (@name, @price, @isActive, @description, @image, @preparationTime, @createdAt) 
                RETURNING id
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@name", menu.Name);
            command.Parameters.AddWithValue("@price", menu.Price);
            command.Parameters.AddWithValue("@isActive", menu.IsActive);
            command.Parameters.AddWithValue("@description", menu.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@image", menu.Image ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@preparationTime", menu.PreparationTime);
            command.Parameters.AddWithValue("@createdAt", DateTime.UtcNow);

            return (int)await command.ExecuteScalarAsync();
        }

        public async Task<bool> UpdateMenuAsync(Menu menu)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                UPDATE menu 
                SET name = @name, price = @price, is_active = @isActive, 
                    description = @description, image = @image, preparation_time = @preparationTime
                WHERE id = @id
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", menu.Id);
            command.Parameters.AddWithValue("@name", menu.Name);
            command.Parameters.AddWithValue("@price", menu.Price);
            command.Parameters.AddWithValue("@isActive", menu.IsActive);
            command.Parameters.AddWithValue("@description", menu.Description ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@image", menu.Image ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@preparationTime", menu.PreparationTime);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdateMenuPriceAsync(int menuId, decimal newPrice)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = "UPDATE menu SET price = @price WHERE id = @id";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", menuId);
            command.Parameters.AddWithValue("@price", newPrice);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteMenuAsync(int id)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = "DELETE FROM menu WHERE id = @id";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories = new List<Category>();
            
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = "SELECT id, name, description, display_order, is_active, image, created_at FROM category ORDER BY display_order, name";
            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                categories.Add(new Category
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                    DisplayOrder = reader.GetInt32(3),
                    IsActive = reader.GetBoolean(4),
                    Image = reader.IsDBNull(5) ? null : reader.GetString(5),
                    CreatedAt = reader.GetDateTime(6)
                });
            }

            return categories;
        }

        // Остальные методы можно реализовать по аналогии
        public Task<List<Category>> GetCategoriesWithMenusAsync() => throw new NotImplementedException();
        public Task<Category?> GetCategoryByIdAsync(int id) => throw new NotImplementedException();
        public Task<int> CreateCategoryAsync(Category category) => throw new NotImplementedException();
        public Task<bool> UpdateCategoryAsync(Category category) => throw new NotImplementedException();
        public Task<bool> DeleteCategoryAsync(int id) => throw new NotImplementedException();
        public Task<List<Menu>> GetMenusByCategoryAsync(int categoryId) => throw new NotImplementedException();
        public Task<List<Menu>> GetMenusByCategoryNameAsync(string categoryName) => throw new NotImplementedException();
        public Task<bool> AddMenuToCategoryAsync(int menuId, int categoryId) => throw new NotImplementedException();
        public Task<bool> RemoveMenuFromCategoryAsync(int menuId, int categoryId) => throw new NotImplementedException();
        public Task<MenuStatistics> GetMenuStatisticsAsync() => throw new NotImplementedException();
        public Task<List<Menu>> GetMenusByPriceRangeAsync(decimal minPrice, decimal maxPrice) => throw new NotImplementedException();
        public Task<bool> IsMenuNameUniqueAsync(string name, int? excludeId = null) => throw new NotImplementedException();
        public Task<List<Menu>> SearchMenusByNameAsync(string searchTerm) => throw new NotImplementedException();

        // Устаревшие методы - удалите их из интерфейса
        public Task<List<Menu>> GetAllAsync() => throw new NotImplementedException();
        public Task<Menu?> GetByIdAsync(int id) => throw new NotImplementedException();
        public Task<List<Menu>> GetByCategoryAsync(int categoryId) => throw new NotImplementedException();
        public Task<List<Menu>> GetActiveAsync() => throw new NotImplementedException();
        public Task<int> AddAsync(Menu menu) => throw new NotImplementedException();
        public Task<bool> UpdateAsync(Menu menu) => throw new NotImplementedException();
        public Task<bool> DeleteAsync(int id) => throw new NotImplementedException();
        public Task<bool> SoftDeleteAsync(int id) => throw new NotImplementedException();
        public Task<bool> RestoreAsync(int id) => throw new NotImplementedException();
        public Task<List<Menu>> SearchByNameAsync(string searchTerm) => throw new NotImplementedException();
        public Task<List<Menu>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice) => throw new NotImplementedException();
        public Task<bool> ExistsAsync(int id) => throw new NotImplementedException();
        public Task<bool> IsNameUniqueAsync(string name, int? excludeId = null) => throw new NotImplementedException();
        public Task<int> GetTotalCountAsync() => throw new NotImplementedException();
        public Task<MenuStatistics> GetStatisticsAsync() => throw new NotImplementedException();
        public Task<List<Menu>> GetRecentlyAddedAsync(int days = 7) => throw new NotImplementedException();
        public Task<bool> BulkUpdatePricesAsync(Dictionary<int, decimal> priceUpdates) => throw new NotImplementedException();
        public Task<List<Category>> GetCategoriesAsync() => throw new NotImplementedException();
    }
}