using Microsoft.AspNetCore.Authorization;

namespace SharedLibrary.AuthRequirements;

public class AdminClientsRequirementBypass:IAuthorizationRequirement
{
    public AdminClientsRequirementBypass(List<string> clientId)
    {
        ClientIdList = new List<string>(clientId);
    }

    public List<string> ClientIdList { get; }

  
}