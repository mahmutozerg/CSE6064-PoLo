using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PoLoAnalysisBusinessAPI.AuthRequirements;

namespace PoLoAnalysisBusinessAPI.RequirementHandlers;

public class AdminClientIdBypassRequirementHandler:AuthorizationHandler<AdminClientIdBypassRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminClientIdBypassRequirement requirement)
    {
        
        var nameIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (context.User.IsInRole("Admin")||(nameIdClaim != null && nameIdClaim == requirement.ClientId))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}