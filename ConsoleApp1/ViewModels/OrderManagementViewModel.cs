public class OrderManagementViewModel : ViewModelBase
{
    private readonly IOrderService _orderService;
    private readonly Personal _currentUser;
    private ObservableCollection<Order> _orders = new();
    private Order _selectedOrder = new();
    private ObservableCollection<Menu> _availableMenus = new();
    private Menu _selectedMenu;
    private int _quantity = 1;
    private string _statusMessage = "";

    public OrderManagementViewModel(IOrderService orderService, Personal currentUser)
    {
        _orderService = orderService;
        _currentUser = currentUser;

        LoadOrdersCommand = new AsyncRelayCommand(LoadOrdersAsync);
        CreateOrderCommand = new AsyncRelayCommand(CreateOrderAsync);
        AddProductCommand = new AsyncRelayCommand(AddProductToOrderAsync);
        UpdateStatusCommand = new AsyncRelayCommand(UpdateOrderStatusAsync);
        ProcessPaymentCommand = new AsyncRelayCommand(ProcessPaymentAsync, CanProcessPayment);
        LoadMenuCommand = new AsyncRelayCommand(LoadAvailableMenusAsync);

        // Загружаем данные при инициализации
        LoadOrdersCommand.Execute(null);
        LoadMenuCommand.Execute(null);
    }

    public ObservableCollection<Order> Orders
    {
        get => _orders;
        set => this.RaiseAndSetIfChanged(ref _orders, value);
    }

    public Order SelectedOrder
    {
        get => _selectedOrder;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedOrder, value);
            ProcessPaymentCommand.NotifyCanExecuteChanged();
        }
    }

    public ObservableCollection<Menu> AvailableMenus
    {
        get => _availableMenus;
        set => this.RaiseAndSetIfChanged(ref _availableMenus, value);
    }

    public Menu SelectedMenu
    {
        get => _selectedMenu;
        set => this.RaiseAndSetIfChanged(ref _selectedMenu, value);
    }

    public int Quantity
    {
        get => _quantity;
        set => this.RaiseAndSetIfChanged(ref _quantity, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
    }

    public decimal NewOrderTotal => SelectedOrder?.TotalPrice ?? 0;

    public IAsyncRelayCommand LoadOrdersCommand { get; }
    public IAsyncRelayCommand CreateOrderCommand { get; }
    public IAsyncRelayCommand AddProductCommand { get; }
    public IAsyncRelayCommand UpdateStatusCommand { get; }
    public IAsyncRelayCommand ProcessPaymentCommand { get; }
    public IAsyncRelayCommand LoadMenuCommand { get; }

    private async Task LoadOrdersAsync()
    {
        try
        {
            StatusMessage = "Загрузка заказов...";
            var orders = _currentUser.PersonalRole == PersonalRole.Waiter
                ? await _orderService.GetOrdersByWaiterAsync(_currentUser.Id)
                : await _orderService.GetActiveOrdersAsync();
            
            Orders = new ObservableCollection<Order>(orders);
            StatusMessage = $"Загружено {Orders.Count} заказов";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка загрузки: {ex.Message}";
        }
    }

    private async Task CreateOrderAsync()
    {
        if (SelectedOrder.Table <= 0 || SelectedOrder.ClientCount <= 0)
        {
            StatusMessage = "Заполните номер стола и количество клиентов";
            return;
        }

        try
        {
            var newOrder = await _orderService.CreateOrderAsync(
                SelectedOrder.Table,
                _currentUser.Id,
                SelectedOrder.ClientCount);

            StatusMessage = $"Заказ #{newOrder.Id} создан";
            SelectedOrder = new Order(); // Сброс формы
            await LoadOrdersAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка создания заказа: {ex.Message}";
        }
    }

    private async Task AddProductToOrderAsync()
    {
        if (SelectedOrder?.Id == 0 || SelectedMenu == null || Quantity <= 0)
        {
            StatusMessage = "Выберите заказ, блюдо и количество";
            return;
        }

        try
        {
            var success = await _orderService.AddProductToOrderAsync(
                SelectedOrder.Id, SelectedMenu.Id, Quantity);

            StatusMessage = success ? "Блюдо добавлено" : "Ошибка добавления";
            await LoadOrdersAsync();
            Quantity = 1;
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
        }
    }

    private async Task UpdateOrderStatusAsync()
    {
        if (SelectedOrder?.Id == 0) return;

        try
        {
            var newStatus = SelectedOrder.Status switch
            {
                OrderStatus.Created => OrderStatus.InProgress,
                OrderStatus.InProgress => OrderStatus.Completed,
                _ => SelectedOrder.Status
            };

            var success = await _orderService.UpdateOrderStatusAsync(SelectedOrder.Id, newStatus);
            StatusMessage = success ? "Статус обновлен" : "Ошибка обновления";
            await LoadOrdersAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
        }
    }

    private async Task ProcessPaymentAsync()
    {
        if (SelectedOrder?.Id == 0) return;

        try
        {
            var success = await _orderService.ProcessPaymentAsync(SelectedOrder.Id, SelectedOrder.TotalPrice);
            StatusMessage = success ? "Оплата прошла успешно" : "Ошибка оплаты";
            await LoadOrdersAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
        }
    }

    private bool CanProcessPayment()
    {
        return SelectedOrder?.Id > 0 && 
               SelectedOrder.Status == OrderStatus.Completed &&
               SelectedOrder.TotalPrice > 0;
    }

    private async Task LoadAvailableMenusAsync()
    {
        try
        {
            // Здесь должен быть вызов сервиса меню
            // AvailableMenus = new ObservableCollection<Menu>(await _menuService.GetActiveMenusAsync());
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка загрузки меню: {ex.Message}";
        }
    }
}