using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Helpers;
using WebApplication1.IServices;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class InventoryService : IInventoryService
{
    private readonly ApplicationDbContext _context;

    public InventoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<InventoryItemDto>> GetInventoryItemsAsync(int organizationId, InventoryItemQuery query)
    {
        var items = _context.InventoryItems
            .Where(i => i.OrganizationId == organizationId);

        if (!string.IsNullOrEmpty(query.Name))
            items = items.Where(i => i.Name.Contains(query.Name));

        if (query.State.HasValue)
            items = items.Where(i => i.State == query.State.Value);

        if (!string.IsNullOrEmpty(query.Type))
            items = items.Where(i => i.Type.Contains(query.Type));

        return await items
            .Select(i => new InventoryItemDto
            {
                Id = i.Id,
                Name = i.Name,
                Type = i.Type,
                CreationDate = i.CreationDate,
                State = i.State
            })
            .ToListAsync();
    }

    public async Task<InventoryItemDto> AddInventoryItemAsync(int organizationId, CreateInventoryItemDto dto)
    {
        var item = new InventoryItem
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Type = dto.Type,
            CreationDate = DateTime.UtcNow,
            State = dto.State,
            OrganizationId = organizationId
        };

        _context.InventoryItems.Add(item);
        await _context.SaveChangesAsync();

        return new InventoryItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Type = item.Type,
            CreationDate = item.CreationDate,
            State = item.State
        };
    }
}
