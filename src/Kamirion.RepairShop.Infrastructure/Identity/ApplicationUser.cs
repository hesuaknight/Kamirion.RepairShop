using Kamirion.RepairShop.Shared.Utils;
using Microsoft.AspNetCore.Identity;

namespace Kamirion.RepairShop.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        Id = UlidGenerator.New();
    }

    public ApplicationUser(string userName) : base(userName)
    {
        Id = UlidGenerator.New();
    }
}
