using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

public interface IUserOrganizationValidator
{
    Task<bool> IsUserInOrganizationAsync(string userId, int organizationId);
}

public class UserOrganizationValidator : IUserOrganizationValidator
{
    private readonly ApplicationDbContext _context;

    public UserOrganizationValidator(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsUserInOrganizationAsync(string userId, int organizationId)
    {
        var userOrganization = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.OrganizationId == organizationId);
        
        return userOrganization != null;
    }
}