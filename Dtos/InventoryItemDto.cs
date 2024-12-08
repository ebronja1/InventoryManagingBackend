using WebApplication1.Models;

namespace WebApplication1.Dtos;

public class InventoryItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public DateTime CreationDate { get; set; }
    public InventoryState State { get; set; }
}
