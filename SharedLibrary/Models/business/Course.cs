using System.Text.Json.Serialization;

namespace SharedLibrary.Models.business;

public class Course:Base
{
    public string Year { get; set; } = string.Empty;
    public bool IsCompulsory { get; set; }
    public List<AppUser> Users { get; set; }
    [JsonIgnore]
    public List<File> File { get; set; }
}