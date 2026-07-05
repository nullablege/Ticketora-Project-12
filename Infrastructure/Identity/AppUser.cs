using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class AppUser:IdentityUser<int>
{
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
