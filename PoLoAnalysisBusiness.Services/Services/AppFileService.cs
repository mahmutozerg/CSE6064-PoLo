﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoLoAnalysisBusiness.Core.Repositories;
using PoLoAnalysisBusiness.Core.Services;
using PoLoAnalysisBusiness.Core.UnitOfWorks;
using SharedLibrary;
using SharedLibrary.DTOs.Responses;
using File = PoLoAnalysisBusiness.Core.Models.File;
using StatusCodes = SharedLibrary.StatusCodes;
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
            var fileName = $"..\\UploadedFiles\\{id}.xlsx";

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await model.CopyToAsync(stream);

            var file = new File()
            {
                Id = id,
                CourseId = "test",
                Path = fileName

            };
            var result =await AddAsync(file,Guid.NewGuid().ToString());
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

    public async Task<FileStreamResult> GetFileStreamAsync(string fileId)
    {
        var entity = await _fileRepository.GetById(fileId); 
            
        if (entity == null)
            throw new Exception(ResponseMessages.NotFound); 

        var filePath = entity.Path; 

        if (!System.IO.File.Exists(filePath))
            throw new Exception(ResponseMessages.BadRecords);

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var contentType = "application/octet-stream"; 


        return new FileStreamResult(fileStream, contentType)
        {
            FileDownloadName = Path.GetFileName(entity.CourseId+".xlsx")
        };
    }

    public async Task<File> GetFileWithResultAsync(string id)
    {
        var file = await _fileRepository.Where(f => !f.IsDeleted && f.Id == id)
            .Include(f => f.Result)
            .SingleOrDefaultAsync();

        return file;
    }
}