using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;

namespace SharedLibrary.DTOs.FileResult;

public class ZipByteArrayWithImages
{

    public Byte[] Archive { get; set; } 

    public List<string> GraphResultPaths { get; set; } = new List<string>();
}