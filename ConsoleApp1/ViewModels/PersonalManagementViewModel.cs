public class PersonalManagementViewModel : ViewModelBase
{
    private readonly IPersonalService _personalService;
    private ObservableCollection<Personal> _personalList = new();
    private Personal _selectedPersonal = new();
    private string _statusMessage = "";

    public PersonalManagementViewModel(IPersonalService personalService)
    {
        _personalService = personalService;

        LoadPersonalCommand = new AsyncRelayCommand(LoadPersonalAsync);
        CreatePersonalCommand = new AsyncRelayCommand(CreatePersonalAsync);
        UpdatePersonalCommand = new AsyncRelayCommand(UpdatePersonalAsync);
        ChangeStatusCommand = new AsyncRelayCommand(ChangePersonalStatusAsync);

        LoadPersonalCommand.Execute(null);
    }

    public ObservableCollection<Personal> PersonalList
    {
        get => _personalList;
        set => this.RaiseAndSetIfChanged(ref _personalList, value);
    }

    public Personal SelectedPersonal
    {
        get => _selectedPersonal;
        set => this.RaiseAndSetIfChanged(ref _selectedPersonal, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
    }

    public Array PersonalRoles => Enum.GetValues(typeof(PersonalRole));

    public IAsyncRelayCommand LoadPersonalCommand { get; }
    public IAsyncRelayCommand CreatePersonalCommand { get; }
    public IAsyncRelayCommand UpdatePersonalCommand { get; }
    public IAsyncRelayCommand ChangeStatusCommand { get; }

    private async Task LoadPersonalAsync()
    {
        try
        {
            var personal = await _personalService.GetAllPersonalAsync();
            PersonalList = new ObservableCollection<Personal>(personal);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка загрузки: {ex.Message}";
        }
    }

    private async Task CreatePersonalAsync()
    {
        if (string.IsNullOrWhiteSpace(SelectedPersonal.Login) || 
            string.IsNullOrWhiteSpace(SelectedPersonal.Pass))
        {
            StatusMessage = "Заполните логин и пароль";
            return;
        }

        try
        {
            var personalId = await _personalService.CreatePersonalAsync(SelectedPersonal);
            StatusMessage = $"Сотрудник '{SelectedPersonal.FullName}' создан";
            SelectedPersonal = new Personal();
            await LoadPersonalAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка создания: {ex.Message}";
        }
    }

    private async Task UpdatePersonalAsync()
    {
        if (SelectedPersonal?.Id == 0) return;

        try
        {
            var success = await _personalService.UpdatePersonalAsync(SelectedPersonal);
            StatusMessage = success ? "Данные обновлены" : "Ошибка обновления";
            await LoadPersonalAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
        }
    }

    private async Task ChangePersonalStatusAsync()
    {
        if (SelectedPersonal?.Id == 0) return;

        try
        {
            var newStatus = !SelectedPersonal.Status;
            var success = await _personalService.ChangePersonalStatusAsync(SelectedPersonal.Id, newStatus);
            StatusMessage = success ? "Статус изменен" : "Ошибка изменения статуса";
            await LoadPersonalAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Ошибка: {ex.Message}";
        }
    }
}