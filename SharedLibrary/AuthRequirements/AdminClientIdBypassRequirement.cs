using Microsoft.AspNetCore.Authorization;

namespace SharedLibrary.AuthRequirements;

public class AdminClientIdBypassRequirement:IAuthorizationRequirement
{
    public string ClientId { get; }

    public AdminClientIdBypassRequirement(string clientId)
    {
        ClientId = clientId;
    }
}