using Microsoft.AspNetCore.Identity;

namespace PoLoAnalysisAuthServer.Core.Models;

public class User:IdentityUser
{
    public DateTime? CreatedAt { get; set; }
    public String? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public String? UpdatedBy { get; set; }
    
}