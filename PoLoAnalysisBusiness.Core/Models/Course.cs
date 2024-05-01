using System.Text.Json.Serialization;
using SharedLibrary.Models;

namespace PoLoAnalysisBusiness.Core.Models;

public class Course:Base
{
    public string Year { get; set; } = string.Empty;
    public bool IsCompulsory { get; set; }
    [JsonIgnore]
    public List<AppUser> Users { get; set; }
    [JsonIgnore]
    public List<File> File { get; set; }
}