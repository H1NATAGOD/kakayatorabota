namespace ConsoleApp1.Models;

public class Menu
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    // Navigation properties
    public List<OrderAndMenu> OrderItems { get; set; } = new();
    public List<MenuAndCategory> MenuCategories { get; set; } = new();
    public List<Category> Categories => MenuCategories.Select(x => x.Category!).ToList();
}