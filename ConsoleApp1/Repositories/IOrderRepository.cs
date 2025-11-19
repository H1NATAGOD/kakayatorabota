using ConsoleApp1.Models;
using OrderStatus = ConsoleApp1.enums.OrderStatus;

namespace ConsoleApp1.IRepository
{
    public interface IOrderRepository
    {
        // Основные CRUD операции
        Task<Order?> GetByIdAsync(int id);
        Task<List<Order>> GetAllAsync();
        Task<int> CreateAsync(Order order);
        Task<bool> UpdateAsync(Order order);
        Task<bool> DeleteAsync(int id);
        
        // Операции со статусами
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        
        // Работа с элементами заказа
        Task<bool> AddMenuToOrderAsync(int orderId, int menuId, int quantity);
        Task<bool> RemoveMenuFromOrderAsync(int orderId, int menuId);
        Task<bool> UpdateOrderItemQuantityAsync(int orderId, int menuId, int quantity);
        
        // Поиск и фильтрация
        Task<List<Order>> GetByWaiterAsync(int waiterId);
        Task<List<Order>> GetByStatusAsync(OrderStatus status);
        Task<List<Order>> GetByTableAsync(int tableNumber);
        Task<List<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        
        // Дополнительные методы
        Task<decimal> GetOrderTotalAsync(int orderId);
        Task<int> GetOrderItemsCountAsync(int orderId);
    }
}