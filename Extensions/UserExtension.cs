using System.Security.Claims;

namespace Music.Extensions;


public static class UserExtension
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.Parse(value!);
    }
}