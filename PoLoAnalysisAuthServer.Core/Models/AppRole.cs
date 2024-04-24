using Microsoft.AspNetCore.Identity;

namespace PoLoAnalysisAuthServer.Core.Models;

public class AppRole:IdentityRole
{
    public AppRole(string name):base(name)
    {
        
    }
}