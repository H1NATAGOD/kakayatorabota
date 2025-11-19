using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp1.IRepository
{
    /// <summary>
    /// Репозиторий для работы с персоналом (таблица personal)
    /// </summary>
    public interface IPersonalRepository
    {
        /// <summary>
        /// Получить сотрудника по логину
        /// </summary>
        Task<Personal?> GetPersonalByLoginAsync(string login);
        
        /// <summary>
        /// Получить сотрудника по ID
        /// </summary>
        Task<Personal?> GetPersonalByIdAsync(int id);
        
        /// <summary>
        /// Получить всех активных сотрудников
        /// </summary>
        Task<List<Personal>> GetAllActivePersonalAsync();
        
        /// <summary>
        /// Получить всех сотрудников (включая неактивных)
        /// </summary>
        Task<List<Personal>> GetAllPersonalAsync();
        
        /// <summary>
        /// Получить сотрудников по роли
        /// </summary>
        Task<List<Personal>> GetPersonalByRoleAsync(PersonalRole role);
        
        /// <summary>
        /// Получить сотрудников по роли и статусу
        /// </summary>
        Task<List<Personal>> GetPersonalByRoleAndStatusAsync(PersonalRole role, bool isActive);
        
        /// <summary>
        /// Создать нового сотрудника
        /// </summary>
        Task<int> CreatePersonalAsync(Personal personal);
        
        /// <summary>
        /// Обновить данные сотрудника
        /// </summary>
        Task<bool> UpdatePersonalAsync(Personal personal);
        
        /// <summary>
        /// Обновить статус сотрудника (активен/неактивен)
        /// </summary>
        Task<bool> UpdatePersonalStatusAsync(int personalId, bool isActive);
        
        /// <summary>
        /// Обновить пароль сотрудника
        /// </summary>
        Task<bool> UpdatePersonalPasswordAsync(int personalId, string newPasswordHash);
        
        /// <summary>
        /// Обновить фото сотрудника
        /// </summary>
        Task<bool> UpdatePersonalPhotoAsync(int personalId, string? photoBase64);
        
        /// <summary>
        /// Проверить существование логина
        /// </summary>
        Task<bool> IsLoginExistsAsync(string login, int? excludePersonalId = null);
        
        /// <summary>
        /// Удалить сотрудника (мягкое удаление через статус)
        /// </summary>
        Task<bool> SoftDeletePersonalAsync(int personalId);
        
        /// <summary>
        /// Поиск сотрудников по имени/фамилии
        /// </summary>
        Task<List<Personal>> SearchPersonalByNameAsync(string searchTerm);
        
        /// <summary>
        /// Получить статистику по сотрудникам
        /// </summary>
        Task<PersonalStatistics> GetPersonalStatisticsAsync();
    }

    /// <summary>
    /// Статистика по персоналу
    /// </summary>
    public class PersonalStatistics
    {
        public int TotalPersonal { get; set; }
        public int ActivePersonal { get; set; }
        public int InactivePersonal { get; set; }
        public int AdministratorsCount { get; set; }
        public int WaitersCount { get; set; }
        public int CooksCount { get; set; }
    }
}