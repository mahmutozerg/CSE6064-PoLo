using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.DTOs;

public class ResultDto
{
    [Required]
    public string ExcelFileId { get; set; }
}