using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models;

public class File:Base
{
    public Course Course { get; set; } = new();
    public string CourseId { get; set; } = string.Empty;
    [Column(TypeName = "nvarchar(450)")] public string Path { get; set; } = string.Empty;

    public Result Result { get; set; } = new();
}