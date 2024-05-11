using System.Text.Json.Serialization;
using SharedLibrary.Models.business;
using File = SharedLibrary.Models.business.File;

namespace SharedLibrary.DTOs;

public class CourseWithUsersDto
{
  
    public string Year { get; set; } = string.Empty;
    public bool IsCompulsory { get; set; }
    public List<AppUser> Users { get; set; }
    [JsonIgnore]
    public List<File> File { get; set; }
    
}