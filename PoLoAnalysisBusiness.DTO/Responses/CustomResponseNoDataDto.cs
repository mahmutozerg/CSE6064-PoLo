namespace SharedLibrary.DTO;

public class CustomResponseNoDataDto
{
    public int StatusCode { get; set; }
    public string[] Errors { get; set; }
    
    public static CustomResponseNoDataDto Fail(int statusCode,string errors)
    {
        return new CustomResponseNoDataDto { StatusCode = statusCode,Errors = errors.Split('\n')};
    }   
    public static CustomResponseNoDataDto Success(int statusCode)
    {
        return new CustomResponseNoDataDto { StatusCode = statusCode,Errors = Array.Empty<string>()};
    }    


}