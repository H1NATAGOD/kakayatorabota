public class MenuManagementViewModel : ViewModelBase
{
    private readonly IMenuService _menuService;
    private ObservableCollection<Menu> _menus = new();
    private ObservableCollection<Category> _categories = new();
    private Menu _selectedMenu = new();
    private Category _selectedCategory = new();
    private string _statusMessage = "";

    public MenuManagementViewModel(IMenuService menuService)
    {
        _menuService = menuService;

        LoadMenusCommand = new AsyncRelayCommand(LoadMenusAsync);
        LoadCategoriesCommand = new AsyncRelayCommand(LoadCategoriesAsync);
        CreateMenuCommand = new AsyncRelayCommand(CreateMenuAsync);
        UpdateMenuCommand = new AsyncRelayCommand(UpdateMenuAsync);
        DeleteMenuCommand = new AsyncRelayCommand(DeleteMenuAsync);

        LoadMenusCommand.Execute(null);
        LoadCategoriesCommand.Execute(null);
    }

    public ObservableCollection<Menu> Menus
    {
        get => _menus;
        set => this.RaiseAndSetIfChanged(ref _menus, value);
    }

    public ObservableCollection<Category> Categories
    {
        get => _categories;
        set => this.RaiseAndSetIfChanged(ref _categories, value);
    }

    public Menu SelectedMenu
    {
        get => _selectedMenu;
        set => this.RaiseAndSetIfChanged(ref _selectedMenu, value);
    }

    public Category SelectedCategory
    {
        get => _selectedCategory;
        set => this.RaiseAndSetIfChanged(ref _selectedCategory, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
    }

    public IAsyncRelayCommand LoadMenusCommand { get; }
    public IAsyncRelayCommand LoadCategoriesCommand { get; }
    public IAsyncRelayCommand CreateMenuCommand { get; }
    public IAsyncRelayCommand UpdateMenuCommand { get; }
    public IAsyncRelayCommand DeleteMenuCommand { get; }

    private async Task LoadMenusAsync()
    {
        try
        {
            var menus = await _menuService.GetAllMenusAsync();
            Menus = new ObservableCollection<Menu>(menus);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка загрузки меню: {ex.Message}";
        }
    }

    private async Task LoadCategoriesAsync()
    {
        try
        {
            var categories = await _menuService.GetAllCategoriesAsync();
            Categories = new ObservableCollection<Category>(categories);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка загрузки категорий: {ex.Message}";
        }
    }

    private async Task CreateMenuAsync()
    {
        if (string.IsNullOrWhiteSpace(SelectedMenu.Name) || SelectedMenu.Price <= 0)
        {
            StatusMessage = "Заполните название и цену";
            return;
        }

        try
        {
            var menuId = await _menuService.CreateMenuAsync(SelectedMenu);
            StatusMessage = $"Блюдо '{SelectedMenu.Name}' создано";
            SelectedMenu = new Menu();
            await LoadMenusAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка создания: {ex.Message}";
        }
    }

    private async Task UpdateMenuAsync()
    {
        if (SelectedMenu?.Id == 0) return;

        try
        {
            var success = await _menuService.UpdateMenuAsync(SelectedMenu);
            StatusMessage = success ? "Блюдо обновлено" : "Ошибка обновления";
            await LoadMenusAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
        }
    }

    private async Task DeleteMenuAsync()
    {
        if (SelectedMenu?.Id == 0) return;

        try
        {
            var success = await _menuService.DeleteMenuAsync(SelectedMenu.Id);
            StatusMessage = success ? "Блюдо удалено" : "Ошибка удаления";
            await LoadMenusAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
        }
    }
}