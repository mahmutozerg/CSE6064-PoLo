﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SharedLibrary.DTOs.Responses;
namespace PoLoAnalysisAuthSever.Service.Services;

public class CatsLoginService
{
    private readonly string _userName;
    private readonly string _password;
    private readonly ChromeDriver _driver;
    public CatsLoginService(string userName,string password)
    {
        _userName = userName;
        _password = password;

        var path = Directory.GetCurrentDirectory()+@"//..//chromedriver.exe";
        var chromeDriverService = ChromeDriverService
            .CreateDefaultService(path);
        chromeDriverService.HideCommandPromptWindow = true;
        
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArguments("--disable-gpu", "--no-sandbox", "--start-maximized --headless");  
        _driver = new ChromeDriver(chromeDriverService, chromeOptions);
    }

    public CustomResponseNoDataDto Start()
    {
        GoToLoginPage();
        return AnalyzePage();
    }

    public void GoToLoginPage()
    {
        _driver.Navigate().GoToUrl("https://cats.iku.edu.tr/portal");

        var userNameInput = _driver.FindElement(By.Id("eid"));
        userNameInput.SendKeys(_userName);
        var passwordInput = _driver.FindElement(By.Id("pw"));
        passwordInput.SendKeys(_password);
        passwordInput.SendKeys(Keys.Enter);
    }
    public CustomResponseNoDataDto AnalyzePage()
    {
        try
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            
            var usernameButton = wait.Until(ExpectedConditions.ElementExists(By.XPath("/html/body/div[2]/div[1]/div[1]/header/nav/ul/li[2]/div/a")));

            usernameButton.Click();
            var userNameElement =
                wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[contains(@class,'Mrphs-userNav__submenuitem--fullname')]")));
            var userNameText = userNameElement.Text.Trim();
            return CustomResponseNoDataDto.Success(200);
        }
        catch (Exception ex)
        {
            _driver.Quit();
            throw new Exception(ex.InnerException.ToString());

        }
    }


    ~CatsLoginService()
    {
        _driver.Quit();
    }
}