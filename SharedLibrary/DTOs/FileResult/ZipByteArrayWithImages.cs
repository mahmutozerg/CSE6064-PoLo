using System.IO.Compression;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace SharedLibrary.DTOs.FileResult;

public class ZipByteArrayWithImages
{

    
    public Stream ArchiveStream { get; set; } // Represents the stream of the zip archive

    public List<string> GraphResultPaths { get; set; } = new List<string>();
}