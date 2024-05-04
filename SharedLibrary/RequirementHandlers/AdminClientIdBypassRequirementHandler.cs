using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SharedLibrary.AuthRequirements;

namespace SharedLibrary.RequirementHandlers;

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