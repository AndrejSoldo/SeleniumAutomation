using NUnit.Framework;
using System;
using OpenQA.Selenium;
using SeleniumProj.BaseClass;
using System.Threading;
//using NunitVideoRecorder;

namespace SeleniumProj
{
    //[WatchDog(SaveInClass.FailedTestsOnly)]
    [TestFixture()]
    public class Falconeri : BaseFalconeri
    {
        public IWebElement FindElement(By selector, NLog.Logger logger)
        {
            // Return null by default
            IWebElement elementToReturn = null;

            try
            {
                // Use the selenium driver to find the element
                elementToReturn = driver.FindElement(selector);
                //logger.Debug($"{selector} was found");
            }
            catch (NoSuchElementException)
            {
                // Do something if the exception occurs, I am just logging
                logger.Error($"No such element: {selector.ToString()} could be found.");
            }
            catch (Exception e)
            {
                // Throw any error we didn't account for
                logger.Fatal($"Exception: {e}");
                throw e;
            }

            // return either the element or null
            return elementToReturn;
        }

        public int[] soldo = new int[5];
        //[Video(Name = "Very important test", Mode = SaveMe.Always)]
        [Test(), Category("Soldo")]
        public void TestCase()
        {
            soldo[0] = 10;
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

            //try
            //{
            //logger.Debug("Test started...");

            //}
            //catch (Exception ex)
            //{
            //    logger.Error(ex, "Test failed");
            //    throw;
            //}
            //finally
            //{
            //    NLog.LogManager.Shutdown();
            //}
          
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            //js.ExecuteScript("console.clear();");
            //js.ExecuteScript("async function recordScreen(){return await navigator.mediaDevices.getDisplayMedia({audio:true,video:{mediaSource:'screen'}})}function createRecorder(stream,mimeType){let recordedChunks=[];const mediaRecorder=new MediaRecorder(stream);mediaRecorder.ondataavailable=function(e){if(e.data.size>0){recordedChunks.push(e.data)}};mediaRecorder.onstop=function(){saveFile(recordedChunks);recordedChunks=[]};mediaRecorder.start(200);return mediaRecorder}function saveFile(recordedChunks){const blob=new Blob(recordedChunks,{type:'video/webm'});let filename='Soldo',downloadLink=document.createElement('a');downloadLink.href=URL.createObjectURL(blob);downloadLink.download=`${filename}.webm`;document.body.appendChild(downloadLink);downloadLink.click();URL.revokeObjectURL(blob);document.body.removeChild(downloadLink)}let stream=await recordScreen();let mimeType='video/webm';let mediaRecorder=createRecorder(stream,mimeType);");
            js.ExecuteScript("console.log(new Date(), 'Test Started');") ;

            //driver.SwitchTo().ActiveElement().Click();
            //driver.SwitchTo().Alert().Accept();


            //var rec_alert = driver.SwitchTo().Alert();
            //String alertText = rec_alert.Text;
            //Console.WriteLine("Alert text is " + alertText);
            //rec_alert.Dismiss();
            //Console.WriteLine(rec_alert);

            //logger.Debug("-----------------------");
            logger.Debug("Test started...");
            Thread.Sleep(1000);
            

            //Actions action = new Actions(driver);
            //action.MoveToElement(selectButton).Perform(); // move to the button

            //IWebElement selectButton = driver.FindElement(By.XPath(".//*[@id='setInputLocaleCountry']"));
            IWebElement selectButton = FindElement(By.XPath(".//*[@id='setInputLocaleCountry']"), logger);
            selectButton.Click();
            logger.Debug("Country selected...");

            Thread.Sleep(1000);

            IWebElement approveButton = FindElement(By.Id("cc-approve-button-thissite"), logger);
            approveButton.Click();
            logger.Debug("Policy accepted...");

            IWebElement login = FindElement(By.XPath(".//*[@data-tab='tab-login-cell']"), logger);
            login.Click();
            logger.Debug("Login clicked...");

            IWebElement emailText = FindElement(By.Id("login-form-email"), logger);
            emailText.SendKeys("test_uat_flc_4@yopmail.com");
            logger.Debug("Mail entered...");

            IWebElement passwordText = FindElement(By.Id("login-form-password"), logger);
            passwordText.SendKeys("8mF.AER9fo");
            logger.Debug("Password entered...");

            IWebElement LoginButton = FindElement(By.XPath(".//*[@class='button button-black mobile-extended wide']"), logger);
            Console.WriteLine(LoginButton);
            LoginButton.Click();
            logger.Debug("Loging in...");
            

            //try
            //{
               // driver.Navigate().GoToUrl("https://test.falconeri.com/us/myprofile/");
            //}
            //finally
            //{
                //driver.Quit();
            //}

            //Thread.Sleep(3000);

            //IWebElement myData = driver.FindElement(By.XPath(".//*[@class='account-menu-link']"));
            //myData.Click();

            driver.Navigate().GoToUrl("https://test.falconeri.com/us/myprofile/");
            js.ExecuteScript("console.log(new Date(), 'Test Ended');");

            driver.Navigate().GoToUrl("https://test.falconeri.com/on/demandware.store/Sites-falconeri-us-Site/us_US/Account-EditProfile");
            logger.Debug("Go to Edit profile...");

            IWebElement input = FindElement(By.XPath(".//*[@id='firstName']"), logger);
            input.SendKeys("Soldo");
            logger.Debug("First name edited...");
            input.SendKeys(Keys.Return);
            logger.Debug("Updated...");

            //IWebElement SaveButton = driver.FindElement(By.XPath(".//*[@name='save']"));
            //SaveButton.Click();

            //js.ExecuteScript("mediaRecorder.stop()");
            //Console.WriteLine(soldo[0]);
            //js.ExecuteScript($"console.log({soldo[0]})");
            //js.ExecuteScript("window.alert('Soldo');");
            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Falconeri testing");
        }

        [Test(), Category("Soldo")]
        public void Soldo()
        {
            Console.WriteLine("Soldo Test");
        }
    }
}
