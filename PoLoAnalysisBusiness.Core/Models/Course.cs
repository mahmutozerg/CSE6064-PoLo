using SharedLibrary.Models;

namespace PoLoAnalysisBusiness.Core.Models;

public class Course:Base
{
    public string Year { get; set; } = string.Empty;
    public bool IsCompulsory { get; set; }
    public List<AppUser> Users { get; set; }
    public File File { get; set; }
}