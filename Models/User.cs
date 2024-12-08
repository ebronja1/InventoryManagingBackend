using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models;

public class User: IdentityUser
{
    public int OrganizationId { get; set; } = 1;
    public Organization? Organization { get; set; } // Navigation property
}