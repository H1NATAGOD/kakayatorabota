using Avalonia.Controls;

namespace ConsoleApp1.Models;

public class OrderAndMenu
{
    public int OrderId { get; set; }
    public int MenuId { get; set; }
    public int Quantity { get; set; } = 1;
    
    // Navigation properties
    public Order? Order { get; set; }
    public Menu? Menu { get; set; }
}