using ConsoleApp1.Models;
using Npgsql;
using OrderStatus = ConsoleApp1.enums.OrderStatus;

namespace ConsoleApp1.IRepository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DatabaseConfig _config;

        public OrderRepository(DatabaseConfig config)
        {
            _config = config;
        }

        public async Task<int> CreateAsync(Order order)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                INSERT INTO "order" ("table", personal, client_count, status) 
                VALUES (@table, @personalId, @clientCount, @status) 
                RETURNING id
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@table", order.Table);
            command.Parameters.AddWithValue("@personalId", order.PersonalId);
            command.Parameters.AddWithValue("@clientCount", order.ClientCount);
            command.Parameters.AddWithValue("@status", (int)order.Status);

            return (int)await command.ExecuteScalarAsync();
        }

        public async Task<bool> AddMenuToOrderAsync(int orderId, int menuId, int quantity)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                INSERT INTO order_and_menu (order_id, menu_id, quantity) 
                VALUES (@orderId, @menuId, @quantity)
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@orderId", orderId);
            command.Parameters.AddWithValue("@menuId", menuId);
            command.Parameters.AddWithValue("@quantity", quantity);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> RemoveMenuFromOrderAsync(int orderId, int menuId)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = "DELETE FROM order_and_menu WHERE order_id = @orderId AND menu_id = @menuId";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@orderId", orderId);
            command.Parameters.AddWithValue("@menuId", menuId);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = "UPDATE \"order\" SET status = @status WHERE id = @id";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", orderId);
            command.Parameters.AddWithValue("@status", (int)status);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                SELECT o.id, o.table, o.personal, o.client_count, o.status, o.created_at,
                       p.id, p.f_name, p.l_name, p.s_name
                FROM "order" o
                LEFT JOIN personal p ON o.personal = p.id
                WHERE o.id = @id
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var order = new Order
                {
                    Id = reader.GetInt32(0),
                    Table = reader.GetInt32(1),
                    PersonalId = reader.GetInt32(2),
                    ClientCount = reader.GetInt32(3),
                    Status = (OrderStatus)reader.GetInt32(4),
                    CreatedAt = reader.GetDateTime(5),
                    Personal = new Personal
                    {
                        Id = reader.GetInt32(6),
                        FName = reader.GetString(7),
                        LName = reader.GetString(8),
                        SName = reader.IsDBNull(9) ? null : reader.GetString(9)
                    }
                };

                // Загружаем элементы заказа
                await LoadOrderItemsAsync(order);
                return order;
            }

            return null;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            var orders = new List<Order>();
            
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                SELECT o.id, o.table, o.personal, o.client_count, o.status, o.created_at,
                       p.id, p.f_name, p.l_name, p.s_name
                FROM "order" o
                LEFT JOIN personal p ON o.personal = p.id
                ORDER BY o.created_at DESC
                """;

            using var command = new NpgsqlCommand(sql, connection);
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var order = new Order
                {
                    Id = reader.GetInt32(0),
                    Table = reader.GetInt32(1),
                    PersonalId = reader.GetInt32(2),
                    ClientCount = reader.GetInt32(3),
                    Status = (OrderStatus)reader.GetInt32(4),
                    CreatedAt = reader.GetDateTime(5),
                    Personal = new Personal
                    {
                        Id = reader.GetInt32(6),
                        FName = reader.GetString(7),
                        LName = reader.GetString(8),
                        SName = reader.IsDBNull(9) ? null : reader.GetString(9)
                    }
                };

                await LoadOrderItemsAsync(order);
                orders.Add(order);
            }

            return orders;
        }

        private async Task LoadOrderItemsAsync(Order order)
        {
            using var connection = new NpgsqlConnection(_config.GetConnectionString());
            await connection.OpenAsync();

            const string sql = """
                SELECT om.order_id, om.menu_id, om.quantity,
                       m.id, m.name, m.price
                FROM order_and_menu om
                LEFT JOIN menu m ON om.menu_id = m.id
                WHERE om.order_id = @orderId
                """;

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@orderId", order.Id);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var orderItem = new OrderAndMenu
                {
                    OrderId = reader.GetInt32(0),
                    MenuId = reader.GetInt32(1),
                    Quantity = reader.GetInt32(2),
                    Menu = new Menu
                    {
                        Id = reader.GetInt32(3),
                        Name = reader.GetString(4),
                        Price = reader.GetDecimal(5)
                    }
                };
                order.OrderItems.Add(orderItem);
            }
        }

        // Остальные методы можно реализовать по аналогии
        public Task<bool> UpdateAsync(Order order) => throw new NotImplementedException();
        public Task<bool> DeleteAsync(int id) => throw new NotImplementedException();
        public Task<bool> UpdateOrderItemQuantityAsync(int orderId, int menuId, int quantity) => throw new NotImplementedException();
        public Task<List<Order>> GetByWaiterAsync(int waiterId) => throw new NotImplementedException();
        public Task<List<Order>> GetByStatusAsync(OrderStatus status) => throw new NotImplementedException();
        public Task<List<Order>> GetByTableAsync(int tableNumber) => throw new NotImplementedException();
        public Task<List<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate) => throw new NotImplementedException();
        public Task<decimal> GetOrderTotalAsync(int orderId) => throw new NotImplementedException();
        public Task<int> GetOrderItemsCountAsync(int orderId) => throw new NotImplementedException();
    }
}