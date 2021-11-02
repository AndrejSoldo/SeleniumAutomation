using NUnit.Framework;
using System;
using OpenQA.Selenium;
using SeleniumProj.BaseClass;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Globalization;
using System.Linq;
using CsvHelper.Configuration;
using SeleniumProj.CsvTemplates;
using OpenQA.Selenium.Support.UI;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
 
//using NunitVideoRecorder;

namespace SeleniumProj
{

    public class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Tea
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class OrderInfo
    {
        public string Sku { get; set; }
        public string Options { get; set; }
    }

    public class SoldoJson
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string[] Products { get; set; }
        public List<string> Locales { get; set; }
        public IDictionary<string, string> ObjectHolder { get; set; }

    }

    public class Users
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }



        public string Password { get; set; }
        public string NewsletterSubscription { get; set; }
        public string LoyaltySubscription { get; set; }



    }

    [TestFixture()]
    public class Falconeri : BaseFalconeri
    {

        public void TryAndClick(string xpath, int delay)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(delay);
            if (driver.FindElements(By.XPath(xpath)).Count > 0)
            {
                IWebElement element = driver.FindElement(By.XPath(xpath));
                element.Click();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            }
            else
            {
                Console.WriteLine("Error while finding element.");
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            }
        }

        public void acceptCookiesButtonOnFalconeri()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            //WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
            //IWebElement acceptCookies = driver.FindElement(By.XPath(".//*[@id='cc-approve-button-thissite']"));
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            if (driver.FindElements(By.XPath(".//*[@id='cc-approve-button-thissite']")).Count > 0)
            {
                IWebElement acceptCookies = driver.FindElement(By.XPath(".//*[@id='cc-approve-button-thissite']"));
                acceptCookies.Click();
                logger.Debug("Element found!");
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            }
            else
            {
                Console.WriteLine("Error while finding element.");
                logger.Debug("Element not found!");
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            }

        }



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

        public List<Foo> InitializeCSV(string path)
        {
            var reader = new StreamReader(path);
            var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<Foo>().ToList();

            return records;
        }

        public List<Tea> InitializeTeaCSV(string path, string delimiter)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter,
            };

            var reader = new StreamReader(path);
            var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<Tea>().ToList();

            return records;
        }

        public List<OrderInfo> InitializeOrderInfoCSV(string path, string delimiter)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter,
            };

            var reader = new StreamReader(path);
            var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<OrderInfo>().ToList();

            return records;
        }

        public SoldoJson InitializeJson(string name)
        {
            StreamReader fstream = new StreamReader("JsonFiles/"+name);
            

            var myjson = JsonSerializer.Deserialize<SoldoJson>(fstream.ReadToEnd(), new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

            return myjson;
        }

        public void InsertOrder(string csvp ,string orderNum)
        {
            using (var stream = File.Open(csvp, FileMode.Append))
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false , Delimiter = ";"}))
                    {
                        //var orders = Orders.GetOrders();
                        //csvWriter.WriteRecords(orders);
                        var order = Orders.AddOrder(orderNum);
                        csvWriter.WriteRecords(order);
                    }
                }
            }
        }
        public void InsertOrder(string csvp, string orderNum, string lastName)
        {
            using (var stream = File.Open(csvp, FileMode.Append))
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, Delimiter = ";" }))
                    {
                        //var orders = Orders.GetOrders();
                        //csvWriter.WriteRecords(orders);
                        var order = Orders.AddOrder(orderNum, lastName);
                        csvWriter.WriteRecords(order);
                    }
                }
            }
        }

        public void InsertOrder(string csvp, string orderNum, string lastName, string paymentMethod)
        {
            using (var stream = File.Open(csvp, FileMode.Append))
            {
                using (var streamWriter = new StreamWriter(stream))
                {
                    using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = false, Delimiter = ";" }))
                    {
                        //var orders = Orders.GetOrders();
                        //csvWriter.WriteRecords(orders);
                        var order = Orders.AddOrder(orderNum, lastName, paymentMethod);
                        csvWriter.WriteRecords(order);
                    }
                }
            }
        }

        public List<Users> InitializeUsersCSV(string path, string delimiter)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter,
            };



            var reader = new StreamReader(path);
            var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<Users>().ToList();



            return records;
        }

        //[Video(Name = "Very important test", Mode = SaveMe.Always)]
        [Test(), Category("Soldo")]
        public void EditProfile()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
          
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

            //IWebElement approveButton = FindElement(By.Id("cc-approve-button-thissite"), logger);
            //approveButton.Click();
            //logger.Debug("Policy accepted...");

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

        [Test(), Category("Register")]
        public void Register()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

            Thread.Sleep(1000);

            IWebElement languageButton = FindElement(By.XPath(".//*[@id='setInputLocaleCountry']"), logger);
            languageButton.Click();
            logger.Debug("Language selected...");

            Thread.Sleep(1000);
           
           /* IWebElement acceptCookiesButton = FindElement(By.XPath(".//*[@id='cc-approve-button-thissite']"), logger);
            acceptCookiesButton.Click();
            logger.Debug("Accept cookies..."); */

            IWebElement selectEmailButton = FindElement(By.XPath(".//*[@id='registration-form-email']"), logger);
            selectEmailButton.Click();
            logger.Debug("Email input selected...");

            //previous used email was test121233@yopmail.com
            selectEmailButton.SendKeys("test121234@yopmail.com");
            logger.Debug("Email entered...");

            Thread.Sleep(1000);

            IWebElement registerButton = FindElement(By.XPath(".//*[@class='button mobile-extended wide button-black button-register-margin-40']"), logger);
            registerButton.Click();
            logger.Debug("Register button clicked...");

            Thread.Sleep(1000);

            IWebElement numberSelectorButton = FindElement(By.XPath(".//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch']"), logger);
            numberSelectorButton.Click();
            logger.Debug("Number selector clicked...");

            IWebElement numberSelect = FindElement(By.XPath(".//*[@data-option-array-index='1']"), logger);
            numberSelect.Click();
            logger.Debug("Number choses");

            IWebElement phoneNumber = FindElement(By.XPath(".//*[@id='registration-form-phone']"),logger);
            phoneNumber.SendKeys("953502025");
            logger.Debug("Phone number entered...");

            IWebElement goButton = FindElement(By.XPath(".//*[@class='button wide mobile-extended button-black registerPhoneButton']"), logger);
            goButton.Click();
            logger.Debug("Go button clicked...");

            Thread.Sleep(1000);

            IWebElement genderButton = FindElement(By.XPath(".//*[@for='gender-4']"), logger);
            genderButton.Click();
            logger.Debug("Gender selected...");

            IWebElement firstName= FindElement(By.XPath(".//*[@id='registration-form-fname']"), logger);
            firstName.SendKeys("Test");
            logger.Debug("First name entered...");

            IWebElement lastName = FindElement(By.XPath(".//*[@id='registration-form-lname']"), logger);
            lastName.SendKeys("Test");
            logger.Debug("Last name entered...");

            IWebElement password = FindElement(By.XPath(".//*[@id='registration-form-password']"), logger);
            password.SendKeys("Test111?");
            logger.Debug("Password entered...");

            IWebElement passwordRepeat= FindElement(By.XPath(".//*[@id='registration-form-password-confirm']"), logger);
            passwordRepeat.SendKeys("Test111?");
            logger.Debug("Repeated password entered...");

            IWebElement recevingNews = FindElement(By.XPath(".//*[@for='add-to-email-list-no']"), logger);
            recevingNews.Click();
            logger.Debug("Receving news choice selected...");

            IWebElement registrationForFalconeriLoyalty = FindElement(By.XPath(".//*[@for='loyalty-yes']"), logger);
            registrationForFalconeriLoyalty.Click();
            logger.Debug("Subscribing for Falconeri Loyalty...");

            Actions actions = new Actions(driver);
            actions.MoveToElement(registrationForFalconeriLoyalty);
            actions.Perform();

            Thread.Sleep(3000);

            IWebElement clickingOnMonth = FindElement(By.XPath(".//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch']"), logger);
            clickingOnMonth.Click();
            clickingOnMonth.Click();
            logger.Debug("Month clicked...");

            IWebElement choosingMonth = FindElement(By.XPath(".//*[@data-option-array-index='1']"), logger);
            choosingMonth.Click();
            logger.Debug("Month choosen...");

            IWebElement clickingOnDay = FindElement(By.XPath(".//*[@class='form-item cell large-2 small-4 date-picker-day-container']"), logger);
            clickingOnDay.Click();
            logger.Debug("Day clicked...");

            IWebElement choosingDay = FindElement(By.XPath(".//*[@data-option-array-index='31']"), logger);

            actions.MoveToElement(choosingDay);
            actions.Perform();

            choosingDay.Click();
            logger.Debug("Day choosen...");

            IWebElement clickingOnYear = FindElement(By.XPath(".//*[@class='form-item cell large-2 small-4 date-picker-year-container']"), logger);
            clickingOnYear.Click();
            logger.Debug("Year clicked...");

            IWebElement choosingYear = FindElement(By.XPath(".//*[@data-option-array-index='36']"), logger);

            actions.MoveToElement(choosingYear);
            actions.Perform();

            choosingYear.Click();
            logger.Debug("Year choosen...");

            /*  IWebElement registrationButton = FindElement(By.XPath(".//*[@class='button wide button-register-final button-black']"), logger);
              registrationButton.Click();
              logger.Debug("registration button clicked..."); */

            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Falconeri testing");
        }

        [Test(),Category("BuyingProductWithPaypal")]
        [Obsolete]
        public void BuyingProductWithPaypal()
        {   
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var csv = InitializeCSV("C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/CsvFiles/user.csv");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

            var csvPath = Path.Combine(Environment.CurrentDirectory, $"C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orders/orders-{DateTime.UtcNow.ToFileTime()}.csv");
            InsertOrder(csvPath, "012345");

            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

            InsertOrder(csvPath, "0123456666");

            #region LangugeButton
            IWebElement languageButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='setInputLocaleCountry']")));
            languageButton.Click();
            logger.Debug("Language selected...");
            #endregion

            #region AlreadyLoggedIn
            Thread.Sleep(1000);
            IWebElement loginButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@data-tab='tab-login-cell']")));
            loginButton.Click();
            logger.Debug("Login button clicked...");
            #endregion

            #region EmailFieldForLogin
            IWebElement emailField = FindElement(By.XPath(".//*[@id='login-form-email']"), logger);
            emailField.SendKeys("KTeyGGrWE170@yopmail.com");
            logger.Debug("Email field filled out...");
            #endregion

            InsertOrder(csvPath, "012345677777777");
            InsertOrder(csvPath, "012345677777777", "Soldo");

            #region PasswordFieldForLogin
            IWebElement passwordField = FindElement(By.XPath(".//*[@id='login-form-password']"), logger);
            passwordField.SendKeys("Test??170");
            logger.Debug("Password field filled out...");
            #endregion

            #region FinalButtonToLogin
            IWebElement finalLoginButton = FindElement(By.XPath(".//*[@class='button button-black mobile-extended wide']"), logger);
            finalLoginButton.Click();
            logger.Debug("Final Login button clicked...");
            #endregion


            Thread.Sleep(100);

            IWebElement addressButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@class='account-menu-link' and @href='/us/addressbook/']")));
            //FindElement(By.XPath(".//*[@class='account-menu-link' and @href='/us/addressbook/']"), logger);
            addressButton.Click();
            logger.Debug("Address button clicked...");

            IWebElement addAddressButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@class='add-address-button']")));
            //FindElement(By.XPath(".//*[@class='add-address-button']"), logger);
            addAddressButton.Click();
            logger.Debug("Add address button clicked...");

            IWebElement firstName = FindElement(By.XPath(".//*[@id='firstName']"), logger);
            firstName.SendKeys("Teya");
            logger.Debug("First name entered...");

            IWebElement lastName = FindElement(By.XPath(".//*[@id='lastName']"), logger);
            lastName.SendKeys("TeyaTest");
            logger.Debug("Last name entered...");

            IWebElement streetAddress = FindElement(By.XPath(".//*[@id='address1']"), logger);
            streetAddress.SendKeys("2380 W US Hwy 89A");
            logger.Debug("Street address entered...");

            IWebElement cityField = FindElement(By.XPath(".//*[@id='city']"), logger);
            cityField.SendKeys("2380 W US Hwy 89A");
            logger.Debug("City entered...");

            IWebElement postcodeField = FindElement(By.XPath(".//*[@id='zipCode']"), logger);
            postcodeField.SendKeys("86336");
            logger.Debug("Postcode field entered...");

            IWebElement stateField = FindElement(By.XPath(".//*[@for='state']"), logger);
            stateField.Click();
            logger.Debug("State clicked...");

            Thread.Sleep(100);

            IWebElement stateChoosen = FindElement(By.XPath(".//*[@data-option-array-index='4']"), logger);
            stateChoosen.Click();
            logger.Debug("State choosen...");

            IWebElement countryField = FindElement(By.XPath(".//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch']"), logger);
            countryField.Click();
            logger.Debug("Country clicked...");

            Thread.Sleep(100);

            IWebElement countryChoosen = FindElement(By.XPath("(.//*[@class='chosen-results'])[2]"), logger);
            countryChoosen.Click();
            logger.Debug("Country choosen...");

            IWebElement saveChangesButton = FindElement(By.XPath(".//*[@class='button button-black activable-after-change']"), logger);
            saveChangesButton.Click();
            logger.Debug("Save changes clicked...");

            IWebElement logoButton = FindElement(By.XPath(".//*[@class='logo-container']"), logger);
            logoButton.Click();
            logger.Debug("Logo button clicked...");

            Thread.Sleep(100);

            //IWebElement categoryButton = FindElement(By.XPath(".//*[@data-tab='#FAL_Women']"), logger);
            //Actions action = new Actions(driver);
            //action.MoveToElement(categoryButton).Perform();
            
            //Thread.Sleep(1000);

            //IWebElement subCategoryButton = FindElement(By.XPath(".//*[@data-href='/us/women/clothing/']"), logger);
            //subCategoryButton.Click();
            //logger.Debug("Sub category button clicked...");

            //Thread.Sleep(1000);

            //IWebElement secondSubCategoryButton = FindElement(By.XPath("(.//*[@href='/us/women/clothing/skirts/'])[1]"), logger);
            //secondSubCategoryButton.Click();
            //logger.Debug("Second sub category button clicked...");

            driver.Url = "https://test.falconeri.com/us/women/clothing/skirts/";

           
            //driver.Navigate().GoToUrl("http://www.google.com");

            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Falconeri testing");

        }
        [Test()]
        public void OpenYopmail()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("window.open('https://yopmail.com/en/wm', 'Newest Window', null)");
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            Thread.Sleep(1000);
            IWebElement cookies = FindElement(By.XPath(".//*[@id='accept']"), logger);
            cookies.Click();

            Thread.Sleep(1000);
            driver.SwitchTo().Window(driver.WindowHandles.First());
            Thread.Sleep(1000);

            driver.SwitchTo().Window(driver.WindowHandles.Last());
            IWebElement input = FindElement(By.XPath(".//*[@class='ycptinput']"), logger);
            input.SendKeys("soldato" + Keys.Enter);


            Thread.Sleep(1000);
            driver.SwitchTo().Window(driver.WindowHandles.First());

            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Falconeri testing");
        }

        [Test()]
        public void Ordering()
        {
            bool isLoggedIn = false;
            Actions action = new Actions(driver);
            //https://test.falconeri.com/us/product/DAL449A++8521M.html
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var csv = InitializeOrderInfoCSV("C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orderingCsv/orderInfo.csv", ";");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

            #region LanguageButton
            IWebElement languageButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='setInputLocaleCountry']")));
            languageButton.Click();
            logger.Debug("Language selected...");
            #endregion

            //#region AlreadyRegistered
            //Thread.Sleep(1000);
            //IWebElement loginButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@data-tab='tab-login-cell']")));
            //loginButton.Click();
            //logger.Debug("Login button clicked...");
            //#endregion

            //#region EmailFieldForLogin
            //IWebElement emailField = FindElement(By.XPath(".//*[@id='login-form-email']"), logger);
            //emailField.SendKeys("KTeyGGrWE170@yopmail.com");
            //logger.Debug("Email field filled out...");
            //#endregion

            //#region PasswordFieldForLogin
            //IWebElement passwordField = FindElement(By.XPath(".//*[@id='login-form-password']"), logger);
            //passwordField.SendKeys("Test??170");
            //logger.Debug("Password field filled out...");
            //#endregion

            //#region FinalButtonForLogin
            //IWebElement finalLoginButton = FindElement(By.XPath(".//*[@class='button button-black mobile-extended wide']"), logger);
            //finalLoginButton.Click();
            //logger.Debug("Final Login button clicked...");
            ////isLoggedIn = true;
            //#endregion

            #region AddingProductsToBag

            Thread.Sleep(1000);
            for (int i = 0; i < 1; i++)
            {
                Thread.Sleep(1000);
                driver.Navigate().GoToUrl($"https://test.falconeri.com/us/product/{csv[i].Sku}++{csv[i].Options}.html");
                Thread.Sleep(1000);
                IWebElement addToBag = FindElement(By.XPath($".//*[@class='cell auto add-to-cart button button-addtocart']"), logger);
                Thread.Sleep(1000);
                addToBag.Click();
            }

            #endregion

            #region ClickingOnShoppingBag
            Thread.Sleep(1000);
            IWebElement bag = FindElement(By.XPath(".//*[@class='button extended uppercase button-black minicart-checkout-button']"), logger);
            Thread.Sleep(1000);
            bag.Click();
            #endregion

            #region PopUpForDifferentStore
            Thread.Sleep(1000);
            TryAndClick(".//*[@id='geoblock-close']", 2);
            #endregion

            #region ContinueToCheckOut
            Thread.Sleep(1000);
            IWebElement checkout = FindElement(By.XPath(".//*[@class='button button-black checkout-btn']"), logger);
            Thread.Sleep(1000);
            checkout.Click();
            #endregion

            #region ContinueToShippingMethod
            Thread.Sleep(1000);
            IWebElement continueButton = FindElement(By.XPath(".//*[@class='button button-black submit-shipping wide fwidth-padding']"), logger);
            Thread.Sleep(1000);
            continueButton.Click();
            #endregion

            #region IsLoggedIn 

            if (isLoggedIn)
            {
                #region AddressComformation
                Thread.Sleep(1000);
                IWebElement addressChoice = FindElement(By.XPath("(.//*[@name='submit'])[1]"), logger);
                Thread.Sleep(1000);
                addressChoice.Click();
                #endregion

                #region NameOnCreditCard
                Thread.Sleep(1000);
                IWebElement nameOnCardInput = FindElement(By.XPath(".//*[@id='cardOwner']"), logger);
                nameOnCardInput.SendKeys("Kar");
                #endregion

                #region CardNumber
                Thread.Sleep(1000);
                IWebElement cardNumberInput = FindElement(By.XPath(".//*[@id='cardNumber']"), logger);
                cardNumberInput.SendKeys("4775718800002026");
                #endregion

                #region MonthForCreditCard
                Thread.Sleep(1000);
                IWebElement monthChoice = FindElement(By.XPath(".//*[@for='expirationMonth']"), logger);
                Thread.Sleep(1000);
                monthChoice.Click();

                if (!isLoggedIn)
                {
                    IWebElement monthButton = FindElement(By.XPath("(.//*[@data-option-array-index='5'])[2]"), logger);
                    action.MoveToElement(monthButton).Perform();
                    monthButton.Click();
                }
                else
                {
                    IWebElement monthButton = FindElement(By.XPath("(.//*[@data-option-array-index='5'])[1]"), logger);
                    action.MoveToElement(monthButton).Perform();
                    monthButton.Click();
                }

                #endregion

                #region YearForCreditCard

                Thread.Sleep(1000);
                IWebElement yearChoice = FindElement(By.XPath(".//*[@for='expirationYear']"), logger);
                Thread.Sleep(1000);
                yearChoice.Click();


                IWebElement yearButton = FindElement(By.XPath("(.//*[@data-option-array-index='3'])[2]"), logger);
                Thread.Sleep(1000);
                yearButton.Click();



                #endregion

                #region CVVForCreditCard
                Thread.Sleep(1000);
                IWebElement cardCVVInput = FindElement(By.XPath(".//*[@id='securityCode']"), logger);
                cardCVVInput.SendKeys("123");

                #endregion

                #region AcceptingTermsForCreditCard
                Thread.Sleep(1000);
                IWebElement acceptingTermsButton = FindElement(By.XPath("(.//*[@class='checkbox-input'])[7]"), logger);
                Thread.Sleep(1000);
                acceptingTermsButton.Click();
                #endregion

                #region FinalButtonOnOrdering
                Thread.Sleep(1000);
                IWebElement sendOrderButton = FindElement(By.XPath("(.//*[@name='submit'])[3]"), logger);
                Thread.Sleep(1000);
                sendOrderButton.Click();

                #endregion

            }
            #endregion

            #region !IsLoggedIn

            if (!isLoggedIn)
            {
                Thread.Sleep(1000);
                IWebElement emailInput = FindElement(By.XPath(".//*[@id='shippingEmail']"), logger);
                Thread.Sleep(1000);
                emailInput.SendKeys("KTeyGGrWE170@yopmail.com");

                Thread.Sleep(1000);
                IWebElement numberPrefix = FindElement(By.XPath(".//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch']"), logger);
                Thread.Sleep(1000);
                numberPrefix.Click();

                Thread.Sleep(1000);
                IWebElement numberSelect = FindElement(By.XPath(".//*[@data-option-array-index='2']"), logger);
                Thread.Sleep(1000);
                numberSelect.Click();

                Thread.Sleep(1000);
                IWebElement numberInput = FindElement(By.XPath(".//*[@id='shippingPhoneNumber']"), logger);
                Thread.Sleep(1000);
                numberInput.SendKeys("123456958");

                Thread.Sleep(1000);
                IWebElement newsButton = FindElement(By.XPath("(.//*[@class='slider round'])[1]"), logger);
                Thread.Sleep(1000);
                newsButton.Click();

                Thread.Sleep(1000);
                IWebElement newsButtonWithProfile = FindElement(By.XPath("(.//*[@class='slider round'])[2]"), logger);
                Thread.Sleep(1000);
                newsButtonWithProfile.Click();

                Thread.Sleep(1000);
                IWebElement firstNameInput = FindElement(By.XPath(".//*[@id='shippingFirstName']"), logger);
                Thread.Sleep(1000);
                firstNameInput.SendKeys("KTeyG");

                Thread.Sleep(1000);
                IWebElement lastNameInput = FindElement(By.XPath(".//*[@id='shippingLastName']"), logger);
                Thread.Sleep(1000);
                lastNameInput.SendKeys("KTeyG");

                Thread.Sleep(500);
                IWebElement addressInput = FindElement(By.XPath(".//*[@id='shippingAddressOne']"), logger);
                Thread.Sleep(500);
                addressInput.SendKeys("10447 Kenai Spur Hwy");

                Thread.Sleep(500);
                IWebElement addressOtherInfoInput = FindElement(By.XPath(".//*[@id='shippingAddressTwo']"), logger);
                Thread.Sleep(500);
                addressOtherInfoInput.SendKeys("Mi 2");

                Thread.Sleep(500);
                IWebElement townInput = FindElement(By.XPath(".//*[@id='shippingAddressCity']"), logger);
                Thread.Sleep(500);
                townInput.SendKeys("Kenai");

                Thread.Sleep(500);
                IWebElement zipInput = FindElement(By.XPath(".//*[@id='shippingZipCode']"), logger);
                Thread.Sleep(500);
                zipInput.SendKeys("99611");

                Thread.Sleep(500);
                IWebElement stateButton = FindElement(By.XPath(".//*[@for='shippingState']"), logger);
                Thread.Sleep(500);
                stateButton.Click();

                Thread.Sleep(1000);
                IWebElement stateChoiceButton = FindElement(By.XPath("(.//*[@data-option-array-index='2'])[2]"), logger);
                Thread.Sleep(2000);
                action.MoveToElement(stateChoiceButton).Perform();
                Thread.Sleep(2000);
                stateButton.Click();

                Thread.Sleep(500);
                IWebElement countryButton = FindElement(By.XPath(".//*[@for='shippingCountry']"), logger);
                Thread.Sleep(500);
                countryButton.Click();

                IWebElement countryChoiceButton = FindElement(By.XPath("(.//*[@data-option-array-index='1'])[3]"), logger);
                action.MoveToElement(countryChoiceButton).Perform();
                Thread.Sleep(1000);
                countryChoiceButton.Click();

                IWebElement continueButtonOntoPayment = FindElement(By.XPath(".//*[@name='submit']"), logger);
                continueButtonOntoPayment.Click();

                IWebElement orderText = FindElement(By.XPath(".//*[@class='cell order-thank-you-msg h4 side-margins receipt-title']"), logger);

                string str = orderText.Text;

                InsertOrder($"C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orders/orders-{ DateTime.UtcNow.ToFileTime()}.csv", GetOrderNumber(str), "Soldato");

                IWebElement PasswordInput = driver.FindElement(By.XPath(".//*[@id='login-form-password']"));
                PasswordInput.SendKeys("Test??170");

            }
            #endregion

            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Falconeri testing");
            //.//*[@data-pid="DAL449A  8521M" and @class="cell auto add-to-cart button button-addtocart"]
        }


        [Test()]
        public void Json()
        {
            var soldoJson = new SoldoJson
            {
                Id = "0",
                Name = "Andrej",
                LastName = "Soldo"
            };



            string jsonSoldo = JsonSerializer.Serialize(soldoJson);
            Console.WriteLine(jsonSoldo);



            SoldoJson jsonDeserialize = JsonSerializer.Deserialize<SoldoJson>(jsonSoldo);
            Console.WriteLine(jsonDeserialize.Id);
            Console.WriteLine(jsonDeserialize.Name);
            Console.WriteLine(jsonDeserialize.LastName);



            //Loaded data from a file



            SoldoJson desJson = InitializeJson("soldoJson.json");



            Console.WriteLine(desJson.Name);
            Console.WriteLine(InitializeJson("soldoJson.json").Id);
            Console.WriteLine(InitializeJson("soldoJson.json").Name);
            Console.WriteLine(InitializeJson("soldoJson.json").LastName);
            Console.WriteLine(InitializeJson("soldoJson.json").Products[0]);
            Console.WriteLine(InitializeJson("soldoJson.json").Locales[0]);
            Console.WriteLine(InitializeJson("soldoJson.json").ObjectHolder["id"]);
            Console.WriteLine(InitializeJson("soldoJson.json").ObjectHolder["name"]);



            string newJson = JsonSerializer.Serialize(InitializeJson("soldoJson.json"));
            Console.WriteLine(newJson);
            Console.WriteLine("#---------------#");
            JsonDocument doc = JsonDocument.Parse(newJson);
            JsonElement root = doc.RootElement;



            Console.WriteLine(root);



            Console.WriteLine(root.GetProperty("Products")[2]);
        }

        [Test()]
        public void RegistrationWithCSVFile()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            var csvUserInformation = InitializeUsersCSV("CsvFiles/TeasUsersSecond.csv", ";");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;


            //csvUserInformation.Count
            for (int i = 1; i < 3; i++)
            {
                if (i == 1)
                {
                    IWebElement languageButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='setInputLocaleCountry']")));
                    languageButton.Click();
                    logger.Debug("Language selected...");
                }



                Thread.Sleep(1000);
                IWebElement selectEmailButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='registration-form-email']")));
                selectEmailButton.Click();
                selectEmailButton.SendKeys(csvUserInformation[i].Email);
                logger.Debug("Email entered...");



                IWebElement registerButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@class='button mobile-extended wide button-black button-register-margin-40']")));
                registerButton.Click();
                logger.Debug("Register button clicked...");



                IWebElement numberSelectorButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch']")));
                numberSelectorButton.Click();
                logger.Debug("Number selector clicked...");



                IWebElement numberSelect = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@data-option-array-index='1']")));
                numberSelect.Click();
                logger.Debug("Number choses");



                IWebElement phoneNumber = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='registration-form-phone']")));
                phoneNumber.SendKeys(csvUserInformation[i].Phone);
                logger.Debug("Phone number entered...");



                IWebElement goButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@class='button wide mobile-extended button-black registerPhoneButton']")));
                goButton.Click();
                logger.Debug("Go button clicked...");



                IWebElement genderButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@for='gender-4']")));
                genderButton.Click();
                logger.Debug("Gender selected...");



                IWebElement firstName = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='registration-form-fname']")));
                firstName.SendKeys(csvUserInformation[i].FirstName);
                logger.Debug("First name entered...");



                IWebElement lastName = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='registration-form-lname']")));
                lastName.SendKeys(csvUserInformation[i].LastName);
                logger.Debug("Last name entered...");



                IWebElement password = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='registration-form-password']")));
                password.SendKeys(csvUserInformation[i].Password);
                logger.Debug("Password entered...");



                IWebElement passwordRepeat = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='registration-form-password-confirm']")));
                passwordRepeat.SendKeys(csvUserInformation[i].Password);
                logger.Debug("Repeated password entered...");



                IWebElement recevingNews = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@for='add-to-email-list-no']")));
                recevingNews.Click();
                logger.Debug("Receving news choice selected...");



                IWebElement registrationForFalconeriLoyalty = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@for='loyalty-no']")));
                registrationForFalconeriLoyalty.Click();
                logger.Debug("Subscribing for Falconeri Loyalty...");



                IWebElement registrationButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@class='button wide button-register-final button-black']")));
                Actions action = new Actions(driver);
                action.MoveToElement(registrationButton).Perform();
                registrationButton.Click();
                logger.Debug("registration button clicked...");



                Thread.Sleep(1000);
                driver.Navigate().GoToUrl($"https://test.falconeri.com/us/login");
            }



            Thread.Sleep(1000);



            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Falconeri testing");
        }

        [Test()]
        public void AcceptingRegistration()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            var csvUserInformation = InitializeUsersCSV("CsvFiles/TeasUsersSecond.csv", ";");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            js.ExecuteScript("window.open('https://yopmail.com/en/wm', 'Newest Window', null)");
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            Thread.Sleep(1000);
            IWebElement cookies = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='accept']")));
            Thread.Sleep(1000);
            cookies.Click();

            for (int i = 0; i < csvUserInformation.Count; i++)
            {
                Thread.Sleep(2000);
                IWebElement input = FindElement(By.XPath(".//*[@class='ycptinput']"), logger);
                input.SendKeys(csvUserInformation[i].Email + Keys.Enter);



                Thread.Sleep(1000);
                IWebElement document = driver.FindElement(By.XPath(".//iframe[@id='ifmail']"));
                driver.SwitchTo().Frame(document);



                IWebElement accountButton = driver.FindElement(By.XPath(".//*[@valign='middle']//a"));



                accountButton.Click();
                driver.SwitchTo().Window(driver.WindowHandles.Last());
                js.ExecuteScript("window.close();");



                driver.SwitchTo().Window(driver.WindowHandles.Last());
                driver.Navigate().GoToUrl($"https://yopmail.com/en/");
                //Thread.Sleep(1000);
            }


        }
           
        public string GetOrderNumber(string text)
        {
            // Create a pattern for a word that starts with letter "M"  
            string pattern = @"[A-Z a-z !]+";
            string orderText = text;
            //"Thank you for your order 123213213213!"
            
            // Create a Regex
            Regex rg = new Regex(pattern);
            string orderNumber = Regex.Replace(orderText, pattern, "");
            Console.WriteLine(orderNumber);
            return orderNumber;
        }

        [Test()]
        public void OrderingWithCreditCard()
        {
            bool isLoggedIn = true;
            Actions action = new Actions(driver);
            //https://test.falconeri.com/us/product/DAL449A++8521M.html
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var csv = InitializeOrderInfoCSV("C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orderingCsv/orderInfo.csv", ";");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

            #region LanguageButton
            IWebElement languageButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='setInputLocaleCountry']")));
            languageButton.Click();
            logger.Debug("Language selected...");
            #endregion

            #region AddingProductsToBag

            Thread.Sleep(1000);
            for (int i = 0; i < 1; i++)
            {
                Thread.Sleep(1000);
                driver.Navigate().GoToUrl($"https://test.falconeri.com/us/product/{csv[i].Sku}++{csv[i].Options}.html");
                Thread.Sleep(1000);
                IWebElement addToBag = FindElement(By.XPath($".//*[@class='cell auto add-to-cart button button-addtocart']"), logger);
                Thread.Sleep(1000);
                addToBag.Click();
            }

            #endregion

            #region ClickingOnShoppingBag
            Thread.Sleep(1000);
            IWebElement bag = FindElement(By.XPath(".//*[@class='button extended uppercase button-black minicart-checkout-button']"), logger);
            Thread.Sleep(1000);
            bag.Click();
            #endregion

            #region PopUpForDifferentStore
            Thread.Sleep(1000);
            TryAndClick(".//*[@id='geoblock-close']", 2);
            #endregion

            #region ContinueToCheckOut
            Thread.Sleep(1000);
            IWebElement checkout = FindElement(By.XPath(".//*[@class='button button-black checkout-btn']"), logger);
            Thread.Sleep(1000);
            checkout.Click();
            #endregion

            #region ContinueToShippingMethod
            Thread.Sleep(1000);
            IWebElement continueButton = FindElement(By.XPath(".//*[@class='button button-black submit-shipping wide fwidth-padding']"), logger);
            Thread.Sleep(1000);
            continueButton.Click();
            #endregion

            #region EmailForShipping
            Thread.Sleep(1000);
            IWebElement emailInput = FindElement(By.XPath(".//*[@id='shippingEmail']"), logger);
            Thread.Sleep(1000);
            emailInput.SendKeys("KTeyGGrWE170@yopmail.com");
            #endregion

            #region NumberPrefixClicked
            Thread.Sleep(1000);
            IWebElement numberPrefix = FindElement(By.XPath(".//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch']"), logger);
            Thread.Sleep(1000);
            numberPrefix.Click();
            #endregion

            #region NumberPrefixChoosen
            Thread.Sleep(1000);
            IWebElement numberSelect = FindElement(By.XPath(".//*[@data-option-array-index='2']"), logger);
            Thread.Sleep(1000);
            numberSelect.Click();
            #endregion

            #region NumberInput
            Thread.Sleep(1000);
            IWebElement numberInput = FindElement(By.XPath(".//*[@id='shippingPhoneNumber']"), logger);
            Thread.Sleep(1000);
            numberInput.SendKeys("123456958");
            #endregion

            #region NewsButton
            Thread.Sleep(1000);
            IWebElement newsButton = FindElement(By.XPath("(.//*[@class='slider round'])[1]"), logger);
            Thread.Sleep(1000);
            newsButton.Click();
            #endregion

            #region SecondNewsButton
            Thread.Sleep(1000);
            IWebElement newsButtonWithProfile = FindElement(By.XPath("(.//*[@class='slider round'])[2]"), logger);
            Thread.Sleep(1000);
            newsButtonWithProfile.Click();
            #endregion

            #region FirstName
            Thread.Sleep(1000);
            IWebElement firstNameInput = FindElement(By.XPath(".//*[@id='shippingFirstName']"), logger);
            Thread.Sleep(1000);
            firstNameInput.SendKeys("KTeyG");
            #endregion

            #region LastName
            Thread.Sleep(1000);
            IWebElement lastNameInput = FindElement(By.XPath(".//*[@id='shippingLastName']"), logger);
            Thread.Sleep(1000);
            lastNameInput.SendKeys("KTeyG");
            #endregion

            #region StreetAddress
            Thread.Sleep(500);
            IWebElement addressInput = FindElement(By.XPath(".//*[@id='shippingAddressOne']"), logger);
            Thread.Sleep(500);
            addressInput.SendKeys("10447 Kenai Spur Hwy");
            #endregion

            #region OtherAddressInformation
            Thread.Sleep(500);
            IWebElement addressOtherInfoInput = FindElement(By.XPath(".//*[@id='shippingAddressTwo']"), logger);
            Thread.Sleep(500);
            addressOtherInfoInput.SendKeys("Mi 2");
            #endregion

            #region Town
            Thread.Sleep(500);
            IWebElement townInput = FindElement(By.XPath(".//*[@id='shippingAddressCity']"), logger);
            Thread.Sleep(500);
            townInput.SendKeys("Kenai");
            #endregion

            #region ZIPInfromation 
            Thread.Sleep(500);
            IWebElement zipInput = FindElement(By.XPath(".//*[@id='shippingZipCode']"), logger);
            Thread.Sleep(500);
            zipInput.SendKeys("99611");
            #endregion

            #region StateClicked
            Thread.Sleep(500);
            IWebElement stateButton = FindElement(By.XPath(".//*[@id='shippingState_chosen']"), logger);
            Thread.Sleep(500);
            stateButton.Click();
            #endregion

            #region StateChoosen 
            Thread.Sleep(1000);
            IWebElement stateChoiceButton = FindElement(By.XPath("(.//*[@data-option-array-index='2'])[2]"), logger);
            Thread.Sleep(2000);
            action.MoveToElement(stateChoiceButton).Perform();
            Thread.Sleep(4000);
            stateChoiceButton.Click();
            #endregion

            #region CountryClicked
            Thread.Sleep(500);
            IWebElement countryButton = FindElement(By.XPath(".//*[@for='shippingCountry']"), logger);
            Thread.Sleep(500);
            countryButton.Click();
            #endregion

            #region CountryChoosen
            IWebElement countryChoiceButton = FindElement(By.XPath("(.//*[@data-option-array-index='0'])[3]"), logger);
            Thread.Sleep(2000);
            action.MoveToElement(countryChoiceButton).Perform();
            Thread.Sleep(4000);
            countryChoiceButton.Click(); 

            #endregion

            #region ContinueToPayment
            IWebElement continueButtonOntoPayment = FindElement(By.XPath(".//*[@name='submit']"), logger);
            action.MoveToElement(continueButtonOntoPayment).Perform();
            continueButtonOntoPayment.Click();
            #endregion

            #region NameOnCreditCard
            Thread.Sleep(1000);
            IWebElement nameOnCardInput = FindElement(By.XPath(".//*[@id='cardOwner']"), logger);
            nameOnCardInput.SendKeys("Kar");
            #endregion

            #region CardNumber
            Thread.Sleep(1000);
            IWebElement cardNumberInput = FindElement(By.XPath(".//*[@id='cardNumber']"), logger);
            cardNumberInput.SendKeys("4775718800002026");
            #endregion

            #region MonthForCreditCard
            Thread.Sleep(1000);
            IWebElement monthChoice = FindElement(By.XPath(".//*[@for='expirationMonth']"), logger);
            Thread.Sleep(1000);
            monthChoice.Click();


            Thread.Sleep(3000);
            IWebElement monthButton = FindElement(By.XPath("(.//*[@data-option-array-index='1'])[3]"), logger);
            Thread.Sleep(3000); 
            //action.MoveToElement(monthButton).Perform();
            //Thread.Sleep(2000); 
            monthButton.Click();
           
            #endregion

            #region YearForCreditCard

            Thread.Sleep(1000);
            IWebElement yearChoice = FindElement(By.XPath(".//*[@for='expirationYear']"), logger);
            Thread.Sleep(1000);
            yearChoice.Click();


            IWebElement yearButton = FindElement(By.XPath("(.//*[@data-option-array-index='2'])[4]"), logger);
            Thread.Sleep(1000);
            yearButton.Click();

            #endregion

            #region CVVForCreditCard
            Thread.Sleep(1000);
            IWebElement cardCVVInput = FindElement(By.XPath(".//*[@id='securityCode']"), logger);
            cardCVVInput.SendKeys("123");

            #endregion

            #region AcceptingTermsForCreditCard
            Thread.Sleep(1000);
            IWebElement acceptingTermsButton = FindElement(By.XPath("(.//*[@class='checkbox-input'])[6]"), logger);
            Thread.Sleep(1000);
            acceptingTermsButton.Click();
            #endregion

            #region FinalButtonOnOrdering
            Thread.Sleep(1000);
            IWebElement sendOrderButton = FindElement(By.XPath("(.//*[@name='submit'])[3]"), logger);
            Thread.Sleep(1000);
            sendOrderButton.Click();

            #endregion

            #region SavingOrderInfo

            IWebElement orderText = FindElement(By.XPath(".//*[@class='cell order-thank-you-msg h4 side-margins receipt-title']"), logger);
            string str = orderText.Text;
            InsertOrder($"C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orders/creditcard/orders-creditcard-{ DateTime.UtcNow.ToFileTime()}.csv", GetOrderNumber(str), "Soldato", "Credit card");
            #endregion

            #region Login 

            if (isLoggedIn)
            {
                #region PasswordForLogin
                IWebElement passwordForLogin = FindElement(By.XPath("(.//*[@name='loginPassword'])[2]"), logger);
                Thread.Sleep(1000);
                passwordForLogin.SendKeys("Test??170");
                #endregion

                #region LoginClicked
                IWebElement loginButton = FindElement(By.XPath(".//*[@id='login']"), logger);
                Thread.Sleep(1000);
                loginButton.Click();
                #endregion
            }

            #endregion

            #region TestPassed
            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Falconeri testing");
            #endregion
        }

        [Test()]
        public void OrderingWithCreditCardExsp()
        {
            bool isLoggedIn = true;
            //https://test.falconeri.com/us/product/DAL449A++8521M.html
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var csv = InitializeOrderInfoCSV("C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orderingCsv/orderInfo.csv", ";");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

            #region LogingScreenFindElements
            IWebElement languageButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='setInputLocaleCountry']")));
            #endregion

            #region LogingScreenClick
            languageButton.Click();
            logger.Debug("Language selected...");
            #endregion

            #region AddingProductsToBag

            Thread.Sleep(1000);
            for (int i = 0; i < 1; i++)
            {
                driver.Navigate().GoToUrl($"https://test.falconeri.com/us/product/{csv[i].Sku}++{csv[i].Options}.html");

                #region FindElementInProducts
                IWebElement addToBag = FindElement(By.XPath($".//*[@class='cell auto add-to-cart button button-addtocart']"), logger);
                #endregion

                #region ClickElementInProducts
                addToBag.Click();
                #endregion
            }

            #endregion

            #region FindElementShoppingBag
            IWebElement bag = FindElement(By.XPath(".//*[@class='button extended uppercase button-black minicart-checkout-button']"), logger);
            #endregion

            #region ClickingElementShoppingBag
            bag.Click();
            #endregion

            #region FindAndClickPopUp
            TryAndClick(".//*[@id='geoblock-close']", 2);
            #endregion

            #region FindElementContinueToCheckOut
            IWebElement checkout = FindElement(By.XPath(".//*[@class='button button-black checkout-btn']"), logger);
            #endregion

            #region ClickElementContinueToCheckOut
            checkout.Click();
            #endregion

            #region FindContinueToShippingMethod
            IWebElement continueButton = FindElement(By.XPath(".//*[@class='button button-black submit-shipping wide fwidth-padding']"), logger);
            #endregion

            #region ClickContinueToShippingMethod
            continueButton.Click();
            #endregion

            #region FindElementsFirstScreen
            IWebElement emailInput = FindElement(By.XPath(".//*[@id='shippingEmail']"), logger);
            IWebElement numberInput = FindElement(By.XPath(".//*[@id='shippingPhoneNumber']"), logger);
            IWebElement newsButton = FindElement(By.XPath("(.//*[@class='slider round'])[1]"), logger);
            IWebElement newsButtonWithProfile = FindElement(By.XPath("(.//*[@class='slider round'])[2]"), logger);
            IWebElement firstNameInput = FindElement(By.XPath(".//*[@id='shippingFirstName']"), logger);
            IWebElement lastNameInput = FindElement(By.XPath(".//*[@id='shippingLastName']"), logger);
            IWebElement addressInput = FindElement(By.XPath(".//*[@id='shippingAddressOne']"), logger);
            IWebElement addressOtherInfoInput = FindElement(By.XPath(".//*[@id='shippingAddressTwo']"), logger);
            IWebElement townInput = FindElement(By.XPath(".//*[@id='shippingAddressCity']"), logger);
            IWebElement zipInput = FindElement(By.XPath(".//*[@id='shippingZipCode']"), logger);
            IWebElement continueButtonOntoPayment = FindElement(By.XPath(".//*[@name='submit']"), logger);

            #endregion

            #region ClickingAndSendingKeysFirstScreen
            emailInput.SendKeys("KTeyGGrWE170@yopmail.com");
            IWebElement numberPrefix = FindElement(By.XPath(".//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch']"), logger);
            numberPrefix.Click();
            IWebElement numberSelect = FindElement(By.XPath(".//*[@data-option-array-index='2']"), logger);
            numberSelect.Click();
            numberInput.SendKeys("123456958");
            newsButton.Click();
            newsButtonWithProfile.Click();
            firstNameInput.SendKeys("KTeyG");
            lastNameInput.SendKeys("KTeyG");
            addressInput.SendKeys("10447 Kenai Spur Hwy");
            addressOtherInfoInput.SendKeys("Mi 2");
            townInput.SendKeys("Kenai");
            zipInput.SendKeys("99611");
            IWebElement stateButton = FindElement(By.XPath(".//*[@id='shippingState_chosen']"), logger);
            stateButton.Click();
            IWebElement stateChoiceButton = FindElement(By.XPath("(.//*[@data-option-array-index='2'])[2]"), logger);
            stateChoiceButton.Click();
            IWebElement countryButton = FindElement(By.XPath(".//*[@for='shippingCountry']"), logger);
            countryButton.Click();
            IWebElement countryChoiceButton = FindElement(By.XPath("(.//*[@data-option-array-index='0'])[3]"), logger);
            countryChoiceButton.Click();
            continueButtonOntoPayment.Click();

            #endregion


            #region FindElementsSecondScreen
            IWebElement nameOnCardInput = FindElement(By.XPath(".//*[@id='cardOwner']"), logger);
            IWebElement cardNumberInput = FindElement(By.XPath(".//*[@id='cardNumber']"), logger);
            IWebElement cardCVVInput = FindElement(By.XPath(".//*[@id='securityCode']"), logger);
            IWebElement acceptingTermsButton = FindElement(By.XPath("(.//*[@class='checkbox-input'])[6]"), logger);
            IWebElement sendOrderButton = FindElement(By.XPath("(.//*[@name='submit'])[3]"), logger);

            #endregion

            #region ClickingAndSendingKeysSecondScreen
            nameOnCardInput.SendKeys("Kar");
            cardNumberInput.SendKeys("4775718800002026");
            IWebElement monthChoice = FindElement(By.XPath(".//*[@for='expirationMonth']"), logger);
            monthChoice.Click();
            IWebElement monthButton = FindElement(By.XPath("(.//*[@data-option-array-index='1'])[3]"), logger); 
            monthButton.Click();
            IWebElement yearChoice = FindElement(By.XPath(".//*[@for='expirationYear']"), logger);
            yearChoice.Click();
            IWebElement yearButton = FindElement(By.XPath("(.//*[@data-option-array-index='2'])[4]"), logger);
            yearButton.Click();
            cardCVVInput.SendKeys("123");
            acceptingTermsButton.Click();
            sendOrderButton.Click();
            #endregion

            #region SavingOrderInfo

            IWebElement orderText = FindElement(By.XPath(".//*[@class='cell order-thank-you-msg h4 side-margins receipt-title']"), logger);
            string str = orderText.Text;
            InsertOrder($"C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orders/creditcard/orders-creditcard-{ DateTime.UtcNow.ToFileTime()}.csv", GetOrderNumber(str), "Soldato", "Credit card");
            #endregion

            #region Login 

            if (isLoggedIn)
            {
                #region FindElementsThirdScreen
                IWebElement passwordForLogin = FindElement(By.XPath("(.//*[@name='loginPassword'])[2]"), logger);
                IWebElement loginButton = FindElement(By.XPath(".//*[@id='login']"), logger);
                #endregion

                #region ClickingAndSendingKeysThirdScreen
                passwordForLogin.SendKeys("Test??170");
                loginButton.Click();

                #endregion
            }

            #endregion

            #region TestPassed
            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Falconeri testing");
            #endregion
        }


        [Test()]
        public void OrderingWithPayPal()
        {
            bool isLoggedIn = true; 
            Actions action = new Actions(driver);
            //https://test.falconeri.com/us/product/DAL449A++8521M.html
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var csv = InitializeOrderInfoCSV("C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orderingCsv/orderInfo.csv", ";");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

            #region LanguageButton
            IWebElement languageButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='setInputLocaleCountry']")));
            languageButton.Click();
            logger.Debug("Language selected...");
            #endregion

            #region AddingProductsToBag

            Thread.Sleep(1000);
            for (int i = 0; i < 1; i++)
            {
                Thread.Sleep(1000);
                driver.Navigate().GoToUrl($"https://test.falconeri.com/us/product/{csv[i].Sku}++{csv[i].Options}.html");
                Thread.Sleep(1000);
                IWebElement addToBag = FindElement(By.XPath($".//*[@class='cell auto add-to-cart button button-addtocart']"), logger);
                Thread.Sleep(1000);
                addToBag.Click();
            }

            #endregion

            #region ClickingOnShoppingBag
            Thread.Sleep(1000);
            IWebElement bag = FindElement(By.XPath(".//*[@class='button extended uppercase button-black minicart-checkout-button']"), logger);
            Thread.Sleep(1000);
            bag.Click();
            #endregion

            #region PopUpForDifferentStore
            Thread.Sleep(1000);
            TryAndClick(".//*[@id='geoblock-close']", 2);
            #endregion

            #region ContinueToCheckOut
            Thread.Sleep(1000);
            IWebElement checkout = FindElement(By.XPath(".//*[@class='button button-black checkout-btn']"), logger);
            Thread.Sleep(1000);
            checkout.Click();
            #endregion

            #region ContinueToShippingMethod
            Thread.Sleep(1000);
            IWebElement continueButton = FindElement(By.XPath(".//*[@class='button button-black submit-shipping wide fwidth-padding']"), logger);
            Thread.Sleep(1000);
            continueButton.Click();
            #endregion

            #region EmailForShipping
            Thread.Sleep(1000);
            IWebElement emailInput = FindElement(By.XPath(".//*[@id='shippingEmail']"), logger);
            Thread.Sleep(1000);
            emailInput.SendKeys("KTeyGGrWE170@yopmail.com");
            #endregion

            #region NumberPrefixClicked
            Thread.Sleep(1000);
            IWebElement numberPrefix = FindElement(By.XPath(".//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch']"), logger);
            Thread.Sleep(1000);
            numberPrefix.Click();
            #endregion

            #region NumberPrefixChoosen
            Thread.Sleep(1000);
            IWebElement numberSelect = FindElement(By.XPath(".//*[@data-option-array-index='2']"), logger);
            Thread.Sleep(1000);
            numberSelect.Click();
            #endregion

            #region NumberInput
            Thread.Sleep(1000);
            IWebElement numberInput = FindElement(By.XPath(".//*[@id='shippingPhoneNumber']"), logger);
            Thread.Sleep(1000);
            numberInput.SendKeys("123456958");
            #endregion

            #region NewsButton
            Thread.Sleep(1000);
            IWebElement newsButton = FindElement(By.XPath("(.//*[@class='slider round'])[1]"), logger);
            Thread.Sleep(1000);
            newsButton.Click();
            #endregion

            #region SecondNewsButton
            Thread.Sleep(1000);
            IWebElement newsButtonWithProfile = FindElement(By.XPath("(.//*[@class='slider round'])[2]"), logger);
            Thread.Sleep(1000);
            newsButtonWithProfile.Click();
            #endregion



            #region FirstName
            Thread.Sleep(1000);
            IWebElement firstNameInput = FindElement(By.XPath(".//*[@id='shippingFirstName']"), logger);
            Thread.Sleep(1000);
            firstNameInput.SendKeys("KTeyG");
            #endregion

            #region LastName
            Thread.Sleep(1000);
            IWebElement lastNameInput = FindElement(By.XPath(".//*[@id='shippingLastName']"), logger);
            Thread.Sleep(1000);
            lastNameInput.SendKeys("KTeyG");
            #endregion

            #region StreetAddress
            Thread.Sleep(500);
            IWebElement addressInput = FindElement(By.XPath(".//*[@id='shippingAddressOne']"), logger);
            Thread.Sleep(500);
            addressInput.SendKeys("10447 Kenai Spur Hwy");
            #endregion

            #region OtherAddressInformation
            Thread.Sleep(500);
            IWebElement addressOtherInfoInput = FindElement(By.XPath(".//*[@id='shippingAddressTwo']"), logger);
            Thread.Sleep(500);
            addressOtherInfoInput.SendKeys("Mi 2");
            #endregion

            #region Town
            Thread.Sleep(500);
            IWebElement townInput = FindElement(By.XPath(".//*[@id='shippingAddressCity']"), logger);
            Thread.Sleep(500);
            townInput.SendKeys("Kenai");
            #endregion

            #region ZIPInfromation 
            Thread.Sleep(500);
            IWebElement zipInput = FindElement(By.XPath(".//*[@id='shippingZipCode']"), logger);
            Thread.Sleep(500);
            zipInput.SendKeys("99611");
            #endregion

            #region StateClicked
            Thread.Sleep(500);
            IWebElement stateButton = FindElement(By.XPath(".//*[@id='shippingState_chosen']"), logger);
            Thread.Sleep(500);
            stateButton.Click();
            #endregion

            #region StateChoosen 
            Thread.Sleep(1000);
            IWebElement stateChoiceButton = FindElement(By.XPath("(.//*[@data-option-array-index='2'])[2]"), logger);
            Thread.Sleep(2000);
            action.MoveToElement(stateChoiceButton).Perform();
            Thread.Sleep(4000);
            stateChoiceButton.Click();
            #endregion

            #region CountryClicked
            Thread.Sleep(500);
            IWebElement countryButton = FindElement(By.XPath(".//*[@for='shippingCountry']"), logger);
            Thread.Sleep(500);
            countryButton.Click();
            #endregion

            #region CountryChoosen
            IWebElement countryChoiceButton = FindElement(By.XPath("(.//*[@data-option-array-index='0'])[3]"), logger);
            Thread.Sleep(2000);
            action.MoveToElement(countryChoiceButton).Perform();
            Thread.Sleep(4000);
            countryChoiceButton.Click();

            #endregion

            #region ContinueToPayment
            IWebElement continueButtonOntoPayment = FindElement(By.XPath(".//*[@name='submit']"), logger);
            action.MoveToElement(continueButtonOntoPayment).Perform();
            continueButtonOntoPayment.Click();
            #endregion

            #region ClickingOnPayPal
            Thread.Sleep(1000);
            IWebElement paypalButton = FindElement(By.XPath("(.//*[@class='radio-input'])[12]"), logger);
            paypalButton.Click();
            #endregion

            #region AcceptingTerms
            Thread.Sleep(1000);
            IWebElement acceptingTermsButton = FindElement(By.XPath("(.//*[@class='checkbox-input'])[7]"), logger);
            Thread.Sleep(1000);
            acceptingTermsButton.Click();
            #endregion

            #region PayButton
            Thread.Sleep(1000); 
            IWebElement payButton = FindElement(By.XPath(".//*[@class='paypal-checkout-button js_paypal_button_on_billing_form']"), logger);
            payButton.Click();
            #endregion

            Thread.Sleep(1000);
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            driver.Manage().Window.Maximize();
            Thread.Sleep(1000);

            #region PayPalEmail
            Thread.Sleep(1000);
            IWebElement payPalEmail = FindElement(By.XPath(".//*[@id='email']"), logger);
            payPalEmail.Clear();
            Thread.Sleep(1000); 
            payPalEmail.SendKeys("calzedonia.test@calzedonia.it");
            #endregion

            #region PayPalPassword
            Thread.Sleep(1000);
            IWebElement payPalPassword = FindElement(By.XPath(".//*[@id='password']"), logger); 
            payPalPassword.SendKeys("test$prova");
            #endregion

            #region LoginToPaypal 
            Thread.Sleep(1000);
            IWebElement loginToPaypal = FindElement(By.XPath(".//*[@id='btnLogin']"), logger);
            loginToPaypal.Click();
            #endregion

            #region AcceptingCookies
            Thread.Sleep(2000);
            IWebElement acceptingCookies = FindElement(By.XPath(".//*[@id='acceptAllButton']"), logger);
            acceptingCookies.Click();
            #endregion

            #region ContinueOnPaypal
            Thread.Sleep(1000);
            IWebElement continueButtonOnPaypal = FindElement(By.XPath(".//*[@id='payment-submit-btn']"), logger);
            IWebElement downButton = FindElement(By.XPath(".//*[@class='CancelLink_container_27tB8 Hermione_cancelLink_2UjcA']"), logger);
            action.MoveToElement(downButton);
            Thread.Sleep(4000);
            continueButtonOnPaypal.Click();
            #endregion

            driver.SwitchTo().Window(driver.WindowHandles.First());

            #region NumberPrefixSecondClicked
            Thread.Sleep(2000);
            IWebElement numberPrefixSecond = FindElement(By.XPath("(.//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch'])[1]"), logger);
            Thread.Sleep(2000);
            numberPrefixSecond.Click();
            #endregion

            #region NumberPrefixSecondChoosen
            Thread.Sleep(1000);
            IWebElement numberSelectSecond = FindElement(By.XPath(".//*[@data-option-array-index='2']"), logger);
            Thread.Sleep(2000);
            numberSelectSecond.Click();
            #endregion

            #region NumberInput
            Thread.Sleep(1000);
            IWebElement numberInputSecond = FindElement(By.XPath(".//*[@class='shippingPhoneNumber']"), logger);
            Thread.Sleep(1000);
            numberInputSecond.SendKeys("123456958");
            Thread.Sleep(1000);
            #endregion

            #region ContinueToPayment
            IWebElement continueButtonToPay = FindElement(By.XPath(".//*[@value='submit-shipping']"), logger);
            Thread.Sleep(3000);
            continueButtonToPay.Click();
            #endregion

            #region FinalButtonOnOrdering
            Thread.Sleep(1000);
            IWebElement sendOrderButton = FindElement(By.XPath(".//*[@class='button button-black wide submit-payment fwidth-padding']"), logger);
            Thread.Sleep(1000);
            sendOrderButton.Click();
            #endregion

            #region Login 

            if (isLoggedIn)
            {
                #region PasswordForLogin
                IWebElement passwordForLogin = FindElement(By.XPath("(.//*[@name='loginPassword'])[2]"), logger);
                Thread.Sleep(1000);
                passwordForLogin.SendKeys("Test??170");
                #endregion

                #region LoginClicked
                IWebElement loginButton = FindElement(By.XPath(".//*[@id='login']"), logger);
                Thread.Sleep(1000);
                loginButton.Click();
                #endregion
            }

            #endregion


            #region SavingOrderInfo

            IWebElement orderText = FindElement(By.XPath(".//*[@class='cell order-thank-you-msg h4 side-margins receipt-title']"), logger);
            string str = orderText.Text;
            InsertOrder($"C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orders/paypal/orders-paypal-{ DateTime.UtcNow.ToFileTime()}.csv", GetOrderNumber(str), "Soldato","PayPal");
            #endregion

            #region TestPassed
            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Falconeri testing");
            #endregion
        }


    }
}