namespace SharedLibrary.Models;

public class Course:Base
{
    public string Year { get; set; } = string.Empty;
    public bool IsCompulsory { get; set; }
    public List<User> Users { get; set; }
    public File File { get; set; } = new();
    public string FileId { get; set; } = string.Empty;
}