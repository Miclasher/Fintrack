using System.Security.Claims;

namespace Fintrack.WebAPI.Extensions;

internal static class ClaimsPrincipalExension
{
    public static Guid GetUserIdFromJwt(this ClaimsPrincipal claimsPrincipal)
    {
        var claim = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

        return claim is null
            ? throw new InvalidDataException("User id was not found in data from JWT token.")
            : Guid.Parse(claim.Value);
    }
}
