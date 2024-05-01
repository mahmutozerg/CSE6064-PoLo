using System.ComponentModel.DataAnnotations;

namespace PoLoAnalysisAuthServer.Core.DTOs.Role;

public class AddRoleDto
{
    [Required]
    public string RoleName { get; set; }
}