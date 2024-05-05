using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using SharedLibrary.AuthRequirements;

namespace SharedLibrary.RequirementHandlers;

public class AdminClientsRequirementHandler:AuthorizationHandler<AdminClientsRequirementBypass>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminClientsRequirementBypass requirementBypass)
    {
        var nameIdClaim = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if ((nameIdClaim != null  && requirementBypass.ClientIdList.Contains(nameIdClaim))|| context.User.IsInRole("Admin"))
        {
            context.Succeed(requirementBypass);
        }

        return Task.CompletedTask;    
    }
}