namespace WebApplication1.Models;

public class InventoryItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public InventoryState State { get; set; }

    public int OrganizationId { get; set; }
    public Organization Organization { get; set; }
}