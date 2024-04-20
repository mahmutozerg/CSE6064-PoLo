using System.Text.Json.Serialization;

namespace WebProgrammingTerm.Auth.Core.DTOs;

public class Response<TEntity> where TEntity : class
{
    public TEntity Data { get; private set; }
    public int StatusCode { get; private set; }

    [JsonIgnore]
    public bool IsSuccessful { get; private set; }
    
    public List<string> Errors { get; private set; }

    public static Response<TEntity> Success(TEntity data, int statusCode)
    {
        return new Response<TEntity> { Data = data, StatusCode = statusCode, IsSuccessful = true };
    }

    public static Response<TEntity> Success(int statusCode)
    {
        return new Response<TEntity> { Data = default, StatusCode = statusCode, IsSuccessful = true };
    }

    public static Response<TEntity> Fail(List<string> errorDto, int statusCode)
    {
        return new Response<TEntity>
        {
            Errors = errorDto,
            StatusCode = statusCode,
            IsSuccessful = false
        };
    }

    public static Response<TEntity> Fail(string errorMessage, int statusCode, bool isShow)
    {
        var errorDto = new List<string>(){errorMessage};

        return new Response<TEntity> { Errors = errorDto, StatusCode = statusCode, IsSuccessful = false };
    }
}