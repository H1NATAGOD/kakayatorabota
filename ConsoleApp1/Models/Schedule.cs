namespace ConsoleApp1.Models;

public class Schedule
{
    public int Id { get; set; }
    public int PersonalId { get; set; }
    public TimeSpan? TimeWork { get; set; }
    public DateTime StartWork { get; set; }
    public DateTime? FinishWork { get; set; }
    
    // Navigation properties
    public Personal? Personal { get; set; }
    
    public bool IsActive => FinishWork == null || FinishWork > DateTime.Now;
}