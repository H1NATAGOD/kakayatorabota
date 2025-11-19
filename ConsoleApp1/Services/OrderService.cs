using ConsoleApp1.enums;
using ConsoleApp1.IRepository;
using ConsoleApp1.Models;
using ConsoleApp1.Services;
using OrderStatus = ConsoleApp1.enums.OrderStatus;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuRepository _menuRepository;

    public OrderService(IOrderRepository orderRepository, IMenuRepository menuRepository)
    {
        _orderRepository = orderRepository;
        _menuRepository = menuRepository;
    }

    public async Task<Order> CreateOrderAsync(int tableNumber, int waiterId, int clientCount)
    {
        // Бизнес-правила
        if (tableNumber <= 0)
            throw new ArgumentException("Номер стола должен быть положительным");
        
        if (clientCount <= 0)
            throw new ArgumentException("Количество клиентов должно быть положительным");

        var order = new Order
        {
            Table = tableNumber,
            PersonalId = waiterId,
            ClientCount = clientCount,
            Status = OrderStatus.Created,
            CreatedAt = DateTime.UtcNow
        };

        order.Id = await _orderRepository.CreateAsync(order);
        return order;
    }

    public async Task<bool> AddProductToOrderAsync(int orderId, int menuId, int quantity)
    {
        // Проверка существования меню
        var menu = await _menuRepository.GetMenuByIdAsync(menuId);
        if (menu == null)
            throw new ArgumentException("Блюдо не найдено");

        // Проверка доступности заказа для изменений
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null || !CanModifyOrder(order))
            throw new InvalidOperationException("Невозможно изменить заказ");

        // Бизнес-логика добавления продукта
        return await _orderRepository.AddMenuToOrderAsync(orderId, menuId, quantity);
    }

    public async Task<bool> RemoveProductFromOrderAsync(int orderId, int menuId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null || !CanModifyOrder(order))
            throw new InvalidOperationException("Невозможно изменить заказ");

        return await _orderRepository.RemoveMenuFromOrderAsync(orderId, menuId);
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new ArgumentException("Заказ не найден");

        // Бизнес-правила для смены статуса
        if (!IsValidStatusTransition(order.Status, status))
            throw new InvalidOperationException($"Недопустимый переход статуса из {order.Status} в {status}");

        return await _orderRepository.UpdateOrderStatusAsync(orderId, status);
    }

    public async Task<bool> ProcessPaymentAsync(int orderId, decimal amount)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new ArgumentException("Заказ не найден");

        if (order.Status != OrderStatus.Completed)
            throw new InvalidOperationException("Можно оплачивать только завершенные заказы");

        if (amount <= 0 || amount != order.TotalPrice)
            throw new ArgumentException("Сумма оплаты должна соответствовать сумме заказа");

        // Обновляем статус на "Оплачен"
        return await _orderRepository.UpdateOrderStatusAsync(orderId, OrderStatus.Paid);
    }

    public async Task<Order?> GetOrderDetailsAsync(int orderId)
    {
        return await _orderRepository.GetByIdAsync(orderId);
    }

    public async Task<List<Order>> GetActiveOrdersAsync()
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Where(o => o.Status != OrderStatus.Paid && o.Status != OrderStatus.Cancelled).ToList();
    }

    public async Task<List<Order>> GetOrdersByWaiterAsync(int waiterId)
    {
        var orders = await _orderRepository.GetAllAsync();
        return orders.Where(o => o.PersonalId == waiterId).ToList();
    }

    public async Task<decimal> CalculateOrderTotalAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        return order?.TotalPrice ?? 0;
    }

    public bool CanModifyOrder(Order order)
    {
        return order.Status == OrderStatus.Created || order.Status == OrderStatus.InProgress;
    }

    public bool CanCancelOrder(Order order)
    {
        return order.Status == OrderStatus.Created || order.Status == OrderStatus.InProgress;
    }

    private bool IsValidStatusTransition(OrderStatus currentStatus, OrderStatus newStatus)
    {
        return (currentStatus, newStatus) switch
        {
            (OrderStatus.Created, OrderStatus.InProgress) => true,
            (OrderStatus.Created, OrderStatus.Cancelled) => true,
            (OrderStatus.InProgress, OrderStatus.Completed) => true,
            (OrderStatus.InProgress, OrderStatus.Cancelled) => true,
            (OrderStatus.Completed, OrderStatus.Paid) => true,
            _ => false
        };
    }
}