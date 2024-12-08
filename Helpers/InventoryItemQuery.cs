using WebApplication1.Models;

namespace WebApplication1.Helpers;

public class InventoryItemQuery
{
    public string? Name { get; set; }
    public InventoryState? State { get; set; }
    public string? Type { get; set; }
}
