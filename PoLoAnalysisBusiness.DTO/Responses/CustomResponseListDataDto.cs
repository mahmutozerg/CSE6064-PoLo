namespace SharedLibrary.DTO;

public class CustomResponseListDataDto<T>

{
    public List<T>? Data { get; set; }
    public int StatusCode { get; set; }
    public string Errors { get; set; }
    
    public static CustomResponseListDataDto<T> Success(List<T> data,int statusCode)
    {
        return new CustomResponseListDataDto<T> {Data = data, StatusCode = statusCode};
    }    

    public static CustomResponseListDataDto<T> Fail(String errors,int statusCode)
    {
        return new CustomResponseListDataDto<T> {Data = default,Errors = errors, StatusCode = statusCode};
    }    
}