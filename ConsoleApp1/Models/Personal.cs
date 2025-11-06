using ConsoleApp1.enums;

namespace ConsoleApp1.Models;

public class Personal
{
    public int Id { get; set; }
    public string FName { get; set; } = string.Empty;
    public string LName { get; set; } = string.Empty;
    public string? SName { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Pass { get; set; } = string.Empty;
    public PersonalRole PersonalRole { get; set; }
    public bool Status { get; set; } = true;
    public string? Photo { get; set; }
    
    public string FullName => $"{LName} {FName} {SName}".Trim();
}