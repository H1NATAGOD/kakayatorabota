// Services/OrderService.cs
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
        var menu = await _menuRepository.GetByIdAsync(menuId);
        if (menu == null)
            throw new ArgumentException("Блюдо не найдено");

        // Проверка доступности заказа для изменений
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null || !CanModifyOrder(order))
            throw new InvalidOperationException("Невозможно изменить заказ");

        // Бизнес-логика добавления продукта
        return await _orderRepository.AddProductAsync(orderId, menuId, quantity);
    }

    public bool CanModifyOrder(Order order)
    {
        return order.Status == OrderStatus.Created || order.Status == OrderStatus.InProgress;
    }
}