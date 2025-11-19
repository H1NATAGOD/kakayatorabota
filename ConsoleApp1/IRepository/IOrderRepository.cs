// Repositories/IOrderRepository.cs
public interface IOrderRepository
{
    // Только CRUD операции с БД
    Task<Order?> GetByIdAsync(int id);
    Task<List<Order>> GetByStatusAsync(OrderStatus status);
    Task<int> CreateAsync(Order order);
    Task<bool> UpdateAsync(Order order);
    Task<bool> UpdateStatusAsync(int orderId, OrderStatus status);
}