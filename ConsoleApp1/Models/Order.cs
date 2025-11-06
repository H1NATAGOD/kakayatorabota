using Avalonia.Controls;
using ConsoleApp1.enums;

namespace ConsoleApp1.Models;

public class Order
{
    public int Id { get; set; }
    public int Table { get; set; }
    public int PersonalId { get; set; }
    public int ClientCount { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public Personal? Personal { get; set; }
    public List<OrderAndMenu> OrderItems { get; set; } = new();
    public List<Menu> Menus => OrderItems.Select(x => x.Menu!).ToList();
    
    public decimal TotalPrice => OrderItems.Sum(x => x.Menu?.Price ?? 0);
}