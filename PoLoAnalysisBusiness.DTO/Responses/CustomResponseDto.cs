namespace SharedLibrary.DTO;

public class CustomResponseDto<TEntity>
{


    public TEntity? Data { get; set; }
    public int StatusCode { get; set; }
    public List<string>? Errors { get; set; }


    public static CustomResponseDto<TEntity> Success(TEntity data,int statusCode)
    {
        return new CustomResponseDto<TEntity> {Data = data, StatusCode = statusCode,Errors = new List<string>()};
    }

    public static CustomResponseDto<TEntity> Success(int statusCode)
    {
        return new CustomResponseDto<TEntity> {StatusCode = statusCode,Errors = new List<string>()};
    }
    public static CustomResponseDto<TEntity> Fail(List<string> errors,int statusCode)
    {
        return new CustomResponseDto<TEntity> {Errors = errors, StatusCode = statusCode};
    }
    
    
    public static CustomResponseDto<TEntity> Fail(string error,int statusCode)
    {
        return new CustomResponseDto<TEntity> {Errors = new List<string>{error}, StatusCode = statusCode};
    }
}