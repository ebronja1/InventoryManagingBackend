using WebApplication1.Dtos;
using WebApplication1.Helpers;

namespace WebApplication1.IServices;

public interface IInventoryService
{
    Task<IEnumerable<InventoryItemDto>> GetInventoryItemsAsync(int organizationId, InventoryItemQuery query);
    Task<InventoryItemDto> AddInventoryItemAsync(int organizationId, CreateInventoryItemDto dto);
}
