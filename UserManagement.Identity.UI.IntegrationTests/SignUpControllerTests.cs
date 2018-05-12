using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Configuration;

namespace UserManagement.Identity.UI.IntegrationTests
{
    
    [TestFixture]
    public class SignUpControllerTests
    {
        private static string ChromeDriver = @"C:\Kingston-Brilliant-CodeRepos\UserManagementAPIv2\packages\Selenium.WebDriver.ChromeDriver.2.22.0.0\driver";

        private static string applicationURI = ConfigurationManager.AppSettings["applicationURL"];

        [Test]
        public void SignUpIndividual_WithValidCredentials_ShouldBeSuccessful()
        {
            var chromeDriver = new ChromeDriver(ChromeDriver);

            chromeDriver.Navigate().GoToUrl(applicationURI + @"SignUp/SignUpIndividual");

            //complete individual sign up form with all proper credentials
            chromeDriver.FindElement(By.Name("PrimaryUser.Email")).SendKeys("jesse5000jm@yahoo.com");
            chromeDriver.FindElement(By.Name("PrimaryUser.Username")).SendKeys("jesse5000jm@yahoo.com");
            chromeDriver.FindElement(By.Name("PrimaryUser.FirstName")).SendKeys("Christopher");
            chromeDriver.FindElement(By.Name("PrimaryUser.LastName")).SendKeys("Walton");

            chromeDriver.FindElement(By.Name("PrimaryUser.MobileNumber")).SendKeys("876-316-7009");
            chromeDriver.FindElement(By.Name("PrimaryUser.Password")).SendKeys("Password*123");
            chromeDriver.FindElement(By.Name("PrimaryUser.ConfirmPassword")).SendKeys("Password*123");
            chromeDriver.FindElement(By.Name("Address")).SendKeys("Test Address in a Test Parish");
            chromeDriver.FindElement(By.Name("TRN")).SendKeys("129-124-834");

            chromeDriver.FindElement(By.ClassName("btn-danger")).Click();
                    
            var textOnSignUpPage = chromeDriver.FindElement(By.TagName("h3")).Text;

            var foundTextOnSuccessfulSignUpPage = textOnSignUpPage.Contains("Thank you for signing up");

            Assert.IsTrue(foundTextOnSuccessfulSignUpPage);

            chromeDriver.Close();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            IntegrationTestHelper.SignUpControllerTest_DBCleanUp();
        }
    }
}
