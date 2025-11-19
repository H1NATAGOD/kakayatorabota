// Services/PersonalService.cs
public class PersonalService : IPersonalService
{
    private readonly IPersonalRepository _personalRepository;
    
    public PersonalService(IPersonalRepository personalRepository)
    {
        _personalRepository = personalRepository;
    }
    
    public async Task<bool> RegisterPersonalAsync(Personal personal)
    {
        // Проверка уникальности логина
        if (await _personalRepository.IsLoginExistsAsync(personal.Login))
        {
            throw new ArgumentException("Логин уже существует");
        }
        
        // Хеширование пароля
        personal.Pass = HashPassword(personal.Pass);
        
        // Создание сотрудника
        var personalId = await _personalRepository.CreatePersonalAsync(personal);
        return personalId > 0;
    }
    
    public async Task<List<Personal>> GetWaitersForShiftAsync()
    {
        return await _personalRepository.GetPersonalByRoleAndStatusAsync(PersonalRole.Waiter, true);
    }
    
    private string HashPassword(string password)
    {
        // Реализация хеширования пароля
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}

