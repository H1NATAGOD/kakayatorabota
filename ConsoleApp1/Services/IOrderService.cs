using ConsoleApp1.Models;
using OrderStatus = ConsoleApp1.enums.OrderStatus;

namespace ConsoleApp1.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(int tableNumber, int waiterId, int clientCount);
        Task<bool> AddProductToOrderAsync(int orderId, int menuId, int quantity);
        Task<bool> RemoveProductFromOrderAsync(int orderId, int menuId);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<bool> ProcessPaymentAsync(int orderId, decimal amount);
        Task<Order?> GetOrderDetailsAsync(int orderId);
        Task<List<Order>> GetActiveOrdersAsync();
        Task<List<Order>> GetOrdersByWaiterAsync(int waiterId);
        Task<decimal> CalculateOrderTotalAsync(int orderId);
        bool CanModifyOrder(Order order);
        bool CanCancelOrder(Order order);
    }
}