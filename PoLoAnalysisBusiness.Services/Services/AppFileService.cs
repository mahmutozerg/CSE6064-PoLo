using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoLoAnalysisBusiness.Core.Repositories;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.Core.UnitOfWorks;
using PoLoAnalysisBusiness.DTO.Responses;
using SharedLibrary;
using File = PoLoAnalysisBusiness.Core.Models.File;

namespace PoLoAnalysisBusiness.Services.Services;

public class AppFileService:GenericService<File>,IAppFileServices
{
    private readonly IAppFileRepository _fileRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AppFileService(IGenericRepository<File?> repository, IUnitOfWork unitOfWork, IAppFileRepository fileRepository) : base(repository, unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _fileRepository = fileRepository;
    }

    public async Task<CustomResponseDto<File>> WriteExcelFileToCurrentDirectoryAsync(IFormFile? model)
    {
        try
        {
            if (model == null || model.Length == 0)
                return CustomResponseDto<File>.Fail( StatusCodes.BadRequest,FileConstants.FILENULL);


            if (!IsExcelFile(model))
                return CustomResponseDto<File>.Fail(StatusCodes.BadRequest, FileConstants.FILEMUSTBEEXCEL);


            var id = Guid.NewGuid().ToString();
            var fileName = $"../UploadedFiles/{id}.xlsx";

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await model.CopyToAsync(stream);

            var file = new File()
            {
                Id = id,
                CourseId = "test",
                Path = fileName

            };
            var result =await AddAsync(file,"mahmut");
            await _unitOfWork.CommitAsync();
            return result;
        }        
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing Excel file: {ex.Message}");
            return CustomResponseDto<File>.Fail(StatusCodes.SystemFail,$"Error writing Excel file: {ex.Message}");
        }
    }

    public bool IsExcelFile(IFormFile file)
    {
        return file.ContentType == FileConstants.EXCELFILEFORMATEXTENTION;
    }

    public async Task<FileStreamResult> GetFileStream(string fileId)
    {
        var entity = await _fileRepository.GetById(fileId); 
            
        if (entity == null)
            throw new Exception("File not found."); 

        var filePath = entity.Path; 

        if (!System.IO.File.Exists(filePath))
            throw new Exception("File not found on disk.");

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var contentType = "application/octet-stream"; 


        return new FileStreamResult(fileStream, contentType)
        {
            FileDownloadName = Path.GetFileName(entity.CourseId+".xlsx")
        };
    }
}