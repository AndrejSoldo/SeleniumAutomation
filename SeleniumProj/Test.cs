using NUnit.Framework;
using System;
using OpenQA.Selenium;
using SeleniumProj.BaseClass;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System.Collections.Generic;
//using NunitVideoRecorder;

namespace SeleniumProj
{
    //[WatchDog(SaveInClass.FailedTestsOnly)]
    [TestFixture()]
    public class Test : BaseTest
    {
        public int[] soldo = new int[5];
        //[Video(Name = "Very important test", Mode = SaveMe.Always)]
        [Test(), Category("Soldo")]
        public void TestCase()
        {

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            js.ExecuteScript("console.log(new Date(), 'Test Started');");

            Thread.Sleep(1000);

            IWebElement selectButton = driver.FindElement(By.XPath(".//*[@id='setInputLocaleCountry']"));

            selectButton.Click();

            Thread.Sleep(1000);

            IWebElement approveButton = driver.FindElement(By.Id("cc-approve-button-thissite"));
            approveButton.Click();

            IWebElement input = driver.FindElement(By.XPath(".//*[@id='registration-form-email']"));
            input.SendKeys("soldo.andrej@gmail.com");

            IWebElement registerButton = driver.FindElement(By.XPath(".//*[@class='button mobile-extended wide button-black button-register-margin-40']"));

            registerButton.Click();

            Thread.Sleep(2000);

            IWebElement phonePrefix = driver.FindElement(By.XPath(".//*[@class='chosen-single']"));
            phonePrefix.Click();

            IWebElement dropdown = driver.FindElement(By.XPath(".//*[@class='chosen-drop']"));

            IWebElement selectedPrefix = driver.FindElement(By.XPath(".//*[@data-option-array-index='0']"));
            selectedPrefix.Click();

            IWebElement PhoneNumber = driver.FindElement(By.XPath(".//*[@id='registration-form-phone']"));
            PhoneNumber.SendKeys("953502025");

            IWebElement GoButton = driver.FindElement(By.XPath(".//*[@class='button wide mobile-extended button-black registerPhoneButton']"));
            GoButton.Click();

            Thread.Sleep(2000);

            IWebElement GenderRadio = driver.FindElement(By.XPath(".//*[@for='gender-1']"));
            GenderRadio.Click();

            IWebElement FirstName = driver.FindElement(By.XPath(".//*[@id='registration-form-fname']"));
            FirstName.SendKeys("Andrej");

            IWebElement LastName = driver.FindElement(By.XPath(".//*[@name='dwfrm_profile_customer_lastname']"));
            LastName.SendKeys("Soldo");

            IWebElement Pass = driver.FindElement(By.XPath(".//*[@name='dwfrm_profile_login_password']"));
            Pass.SendKeys("1234567890Aa!");

            IWebElement Repass = driver.FindElement(By.XPath(".//*[@id='registration-form-password-confirm']"));
            Repass.SendKeys("1234567890Aa!");

            IWebElement RecieveNews = driver.FindElement(By.XPath(".//*[@for='add-to-email-list-yes']"));
            RecieveNews.Click();

            Thread.Sleep(1000);
            IWebElement RecieveNewsProfile = driver.FindElement(By.XPath(".//*[@for='add-to-personalized-news-list-yes']"));
            RecieveNewsProfile.Click();

            IWebElement FalconeriClub = driver.FindElement(By.XPath(".//*[@for='loyalty-no']"));
            FalconeriClub.Click();

            IWebElement RegisterButton = driver.FindElement(By.XPath(".//*[@class='button wide button-register-final button-black']"));

            #region Dummy
            //IWebElement emailText = driver.FindElement(By.Id("login-form-email"));
            //emailText.SendKeys("test_uat_flc_4@yopmail.com");

            //IWebElement passwordText = driver.FindElement(By.Id("login-form-password"));
            //passwordText.SendKeys("8mF.AER9fo");

            //IWebElement LoginButton = driver.FindElement(By.XPath(".//*[@class='button button-black mobile-extended wide']"));
            //Console.WriteLine(LoginButton);
            //LoginButton.Click();

            //try
            //{
            //    driver.Navigate().GoToUrl("https://test.falconeri.com/us/myprofile/");
            //}
            //finally
            //{
            //    //driver.Quit();
            //}

            //Thread.Sleep(3000);

            //driver.Navigate().GoToUrl("https://test.falconeri.com/us/myprofile/");
            //js.ExecuteScript("console.log(new Date(), 'Test Ended');");

            //driver.Navigate().GoToUrl("https://test.falconeri.com/on/demandware.store/Sites-falconeri-us-Site/en_US/Account-EditProfile");

            //IWebElement input = driver.FindElement(By.XPath(".//*[@id='firstName']"));
            //input.SendKeys("Soldo");
            //input.SendKeys(Keys.Return);

            //IWebElement SaveButton = driver.FindElement(By.XPath(".//*[@name='save']"));
            //SaveButton.Click();
            #endregion

            Assert.Pass("Falconeri Registration");
        }
    }
}
