using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PoLoAnalysisBusinessAPI.AuthRequirements;

namespace PoLoAnalysisBusinessAPI.RequirementHandlers;
public class ClientIdRequirementHandler : AuthorizationHandler<ClientIdRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClientIdRequirement requirement)
    {
        var nameIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if ((nameIdClaim != null && nameIdClaim == requirement.ClientId))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

 
}