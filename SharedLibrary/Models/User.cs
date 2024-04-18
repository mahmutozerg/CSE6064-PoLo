using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models;

public class User:Base
{
    public string Name { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    [EmailAddress] public string EMail { get; set; } = string.Empty;
    public List<Course> Courses { get; set; } = new();


}