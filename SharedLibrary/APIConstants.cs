namespace SharedLibrary;

public static class ApiConstants
{
    public const string AuthServerIP = "https://localhost:7248";
    public const string BusinessAPIIp = "https://localhost:7273";
    public const string MVCIP = "https://localhost:7298";
    public const string AuthServerCatsLogin = AuthServerIP + "/api/CatsLogin/Login";
    public static List<string> Aud = new List<string> { "www.polo.com", };
    public const string SessionCookieName = "Session";
    public const string RefreshCookieName = "Refresh";
    

}