namespace ConsoleApp1.Models;

public class MenuAndCategory
{
    public int MenuId { get; set; }
    public int CategoryId { get; set; }
    
    // Navigation properties
    public Menu? Menu { get; set; }
    public Category? Category { get; set; }
}