using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Helpers;
using WebApplication1.IServices;

namespace WebApplication1.Controllers;
[ApiController]
[Route("organizations/{organizationId:int}/inventory")]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly IUserOrganizationValidator _userOrganizationValidator;

    public InventoryController(IInventoryService inventoryService, IUserOrganizationValidator userOrganizationValidator)
    {
        _inventoryService = inventoryService;
        _userOrganizationValidator = userOrganizationValidator;
    }

    [HttpGet]
    [Authorize]   // Ensures the user is authenticated via JWT
    public async Task<IActionResult> GetInventoryItems([FromRoute] int organizationId, [FromQuery] InventoryItemQuery query)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from the JWT claims
        
        var isUserInOrganization = await _userOrganizationValidator.IsUserInOrganizationAsync(userId, organizationId);
        if (!isUserInOrganization)
        {
            return Unauthorized("You are not authorized to view this organization's inventory.");
        }
        
        var items = await _inventoryService.GetInventoryItemsAsync(organizationId, query);
        return Ok(items);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // Ensures the user is authenticated via JWT and has the "Admin" role
    public async Task<IActionResult> AddInventoryItem([FromRoute] int organizationId, [FromBody] CreateInventoryItemDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from the JWT claims
    
        // Check if the user has the "Admin" role (optional, as you already check for this in the attribute)
        if (!User.IsInRole("Admin"))
        {
            return Unauthorized("You do not have the required role to add items.");
        }
    
        var isUserInOrganization = await _userOrganizationValidator.IsUserInOrganizationAsync(userId, organizationId);
        if (!isUserInOrganization)
        {
            return Unauthorized("You are not authorized to add items to this organization's inventory.");
        }
    
        var item = await _inventoryService.AddInventoryItemAsync(organizationId, dto);
        return CreatedAtAction(nameof(GetInventoryItems), new { organizationId }, item);
    }
}
