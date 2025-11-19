// ViewModels/OrderViewModel.cs - ДЛЯ UI
public class OrderViewModel : ViewModelBase
{
    private readonly IOrderService _orderService;
    private ObservableCollection<Order> _orders = new();
    private Order _selectedOrder = new();
    private string _statusMessage = "";

    public OrderViewModel(IOrderService orderService)
    {
        _orderService = orderService;
        
        // Команды для UI
        LoadOrdersCommand = new AsyncRelayCommand(LoadOrdersAsync);
        CreateOrderCommand = new AsyncRelayCommand(CreateOrderAsync);
        AddProductCommand = new AsyncRelayCommand(AddProductToOrderAsync);
        ProcessPaymentCommand = new AsyncRelayCommand(ProcessPaymentAsync, CanProcessPayment);
    }

    // Свойства для привязки данных в UI
    public ObservableCollection<Order> Orders
    {
        get => _orders;
        set => this.RaiseAndSetIfChanged(ref _orders, value);
    }

    public Order SelectedOrder
    {
        get => _selectedOrder;
        set => this.RaiseAndSetIfChanged(ref _selectedOrder, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
    }

    // Команды для UI
    public IAsyncRelayCommand LoadOrdersCommand { get; }
    public IAsyncRelayCommand CreateOrderCommand { get; }
    public IAsyncRelayCommand AddProductCommand { get; }
    public IAsyncRelayCommand ProcessPaymentCommand { get; }

    // Методы, которые вызываются из UI
    private async Task LoadOrdersAsync()
    {
        try
        {
            StatusMessage = "Загрузка заказов...";
            var orders = await _orderService.GetActiveOrdersAsync();
            Orders = new ObservableCollection<Order>(orders);
            StatusMessage = $"Загружено {Orders.Count} заказов";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
        }
    }

    private async Task CreateOrderAsync()
    {
        if (SelectedOrder.Table <= 0 || SelectedOrder.ClientCount <= 0)
        {
            StatusMessage = "Заполните все поля";
            return;
        }

        try
        {
            var newOrder = await _orderService.CreateOrderAsync(
                SelectedOrder.Table, 
                CurrentUser.Id, 
                SelectedOrder.ClientCount);
            
            StatusMessage = $"Заказ #{newOrder.Id} создан";
            await LoadOrdersAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка создания заказа: {ex.Message}";
        }
    }

    private async Task AddProductToOrderAsync()
    {
        // Логика добавления продукта к заказу
    }

    private async Task ProcessPaymentAsync()
    {
        if (SelectedOrder?.Id > 0)
        {
            var success = await _orderService.ProcessPaymentAsync(SelectedOrder.Id, SelectedOrder.TotalPrice);
            StatusMessage = success ? "Оплата прошла успешно" : "Ошибка оплаты";
            await LoadOrdersAsync();
        }
    }

    private bool CanProcessPayment()
    {
        return SelectedOrder?.Id > 0 && 
               SelectedOrder.Status == OrderStatus.Completed &&
               SelectedOrder.TotalPrice > 0;
    }
}