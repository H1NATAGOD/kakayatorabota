using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1.Models
{
    public class Personal
    {
        public int Id { get; set; }
        public string FName { get; set; } = string.Empty;
        public string LName { get; set; } = string.Empty;
        public string? SName { get; set; }
        public string Login { get; set; } = string.Empty;
        public string Pass { get; set; } = string.Empty;
        public enums.PersonalRole PersonalRole { get; set; }
        public bool Status { get; set; } = true;
        public string? Photo { get; set; }
        
        public string FullName => $"{LName} {FName} {SName}".Trim();
    }

    public class Order
    {
        public int Id { get; set; }
        public int Table { get; set; }
        public int PersonalId { get; set; }
        public int ClientCount { get; set; }
        public enums.OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public Personal? Personal { get; set; }
        public List<OrderAndMenu> OrderItems { get; set; } = new();
        public List<Menu> Menus => OrderItems.Select(x => x.Menu!).ToList();
        
        public decimal TotalPrice => OrderItems.Sum(x => (x.Menu?.Price ?? 0) * x.Quantity);
    }

    public class OrderAndMenu
    {
        public int OrderId { get; set; }
        public int MenuId { get; set; }
        public int Quantity { get; set; } = 1;
        
        public Order? Order { get; set; }
        public Menu? Menu { get; set; }
    }

    public class Menu
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int PreparationTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        public List<OrderAndMenu> OrderItems { get; set; } = new();
        public List<MenuAndCategory> MenuCategories { get; set; } = new();
        public List<Category> Categories => MenuCategories.Select(x => x.Category!).ToList();
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Image { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public List<MenuAndCategory> MenuCategories { get; set; } = new();
        public List<Menu> Menus => MenuCategories.Select(x => x.Menu!).Where(m => m.IsActive).ToList();
    }

    public class MenuAndCategory
    {
        public int MenuId { get; set; }
        public int CategoryId { get; set; }
        
        public Menu? Menu { get; set; }
        public Category? Category { get; set; }
    }

    public class Schedule
    {
        public int Id { get; set; }
        public int PersonalId { get; set; }
        public TimeSpan? TimeWork { get; set; }
        public DateTime StartWork { get; set; }
        public DateTime? FinishWork { get; set; }
        
        public Personal? Personal { get; set; }
        
        public bool IsActive => FinishWork == null || FinishWork > DateTime.Now;
    }

    // Enums
    public enum PersonalRole
    {
        Administrator = 1,
        Waiter = 2,
        Cook = 3
    }

    public enum OrderStatus
    {
        Created = 1,
        InProgress = 2,
        Completed = 3,
        Paid = 4,
        Cancelled = 5
    }

    public enum PaymentType
    {
        Cash = 1,
        Card = 2
    }

    // Common DTOs
    public class MenuStatistics
    {
        public int TotalMenus { get; set; }
        public int ActiveMenus { get; set; }
        public int TotalCategories { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public Menu? MostExpensiveMenu { get; set; }
        public Menu? LeastExpensiveMenu { get; set; }
    }

    public class MenuPopularity
    {
        public Menu Menu { get; set; } = new();
        public int OrderCount { get; set; }
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class FinancialReport
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<Order> Orders { get; set; } = new();
    }

    public class PersonalReport
    {
        public Personal Personal { get; set; } = new();
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<Order> Orders { get; set; } = new();
    }
}