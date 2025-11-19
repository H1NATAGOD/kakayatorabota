public class MainViewModel : ViewModelBase
{
    private readonly IOrderService _orderService;
    private readonly IShiftService _shiftService;
    private Personal _currentUser;
    private ViewModelBase _currentPage;
    private string _title = "Кафе Management System";

    public MainViewModel(IOrderService orderService, 
                        IShiftService shiftService, 
                        Personal user)
    {
        _orderService = orderService;
        _shiftService = shiftService;
        _currentUser = user;
        
        // Начальная страница в зависимости от роли
        _currentPage = _currentUser.PersonalRole switch
        {
            PersonalRole.Administrator => new AdminDashboardViewModel(),
            PersonalRole.Waiter => new OrderManagementViewModel(orderService, user),
            PersonalRole.Cook => new KitchenViewModel(orderService),
            _ => new OrderManagementViewModel(orderService, user)
        };

        // Команды навигации
        NavigateToOrdersCommand = new RelayCommand(() => NavigateTo(new OrderManagementViewModel(orderService, user)));
        NavigateToMenuCommand = new RelayCommand(() => NavigateTo(new MenuManagementViewModel()));
        NavigateToPersonalCommand = new RelayCommand(() => NavigateTo(new PersonalManagementViewModel()));
        NavigateToReportsCommand = new RelayCommand(() => NavigateTo(new ReportsViewModel()));
        NavigateToShiftsCommand = new RelayCommand(() => NavigateTo(new ShiftManagementViewModel()));
        
        LogoutCommand = new RelayCommand(Logout);
    }

    public Personal CurrentUser
    {
        get => _currentUser;
        set => this.RaiseAndSetIfChanged(ref _currentUser, value);
    }

    public ViewModelBase CurrentPage
    {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    public string Title
    {
        get => _title;
        set => this.RaiseAndSetIfChanged(ref _title, value);
    }

    public bool IsAdmin => CurrentUser.PersonalRole == PersonalRole.Administrator;
    public bool IsWaiter => CurrentUser.PersonalRole == PersonalRole.Waiter;
    public bool IsCook => CurrentUser.PersonalRole == PersonalRole.Cook;

    public ICommand NavigateToOrdersCommand { get; }
    public ICommand NavigateToMenuCommand { get; }
    public ICommand NavigateToPersonalCommand { get; }
    public ICommand NavigateToReportsCommand { get; }
    public ICommand NavigateToShiftsCommand { get; }
    public ICommand LogoutCommand { get; }

    private void NavigateTo(ViewModelBase viewModel)
    {
        CurrentPage = viewModel;
    }

    private void Logout()
    {
        // Возврат к окну входа
        var loginWindow = new LoginWindow();
        loginWindow.Show();
        
        // Закрытие текущего окна
        (App.Current as App)?.CloseMainWindow();
    }
}