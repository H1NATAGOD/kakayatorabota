namespace ConsoleApp1.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // Navigation properties
    public List<MenuAndCategory> MenuCategories { get; set; } = new();
    public List<Menu> Menus => MenuCategories.Select(x => x.Menu!).ToList();
}