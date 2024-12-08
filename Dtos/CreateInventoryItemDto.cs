using WebApplication1.Models;

namespace WebApplication1.Dtos;

public class CreateInventoryItemDto
{
    public string Name { get; set; }
    public string Type { get; set; }
    public InventoryState State { get; set; }
}
