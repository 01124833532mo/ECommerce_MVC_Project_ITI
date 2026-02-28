using System.Security.Claims;

namespace EcommerceIti.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(id))
        {
            throw new InvalidOperationException("User is not authenticated");
        }
        return id;
    }
}
