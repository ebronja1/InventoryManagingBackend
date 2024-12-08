namespace WebApplication1.Models;

public class Organization
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public List<User> Users { get; set; } = new List<User>();
    
    public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
}