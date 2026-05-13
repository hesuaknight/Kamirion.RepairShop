using System.ComponentModel.DataAnnotations;
using Kamirion.RepairShop.Shared.Utils;
using Microsoft.AspNetCore.Identity;

namespace Kamirion.RepairShop.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    [MaxLength(26)]
    public string TenantId { get; set; } = string.Empty;

    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    [MaxLength(10)]
    public string? PreferredCulture { get; set; }

    public DateTime? LastSeenAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ApplicationUser()
    {
        Id = UlidGenerator.New();
    }

    public ApplicationUser(string userName) : base(userName)
    {
        Id = UlidGenerator.New();
    }
}
