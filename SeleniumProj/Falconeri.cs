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
        public string fullPath;

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
            var csv = InitializeCSV("CsvFiles/user.csv");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));


            Console.WriteLine(csv[0].Id);
            Console.WriteLine(csv[0].Name);

            Console.WriteLine(csv[1].Id);
            Console.WriteLine(csv[1].Name);

            Console.WriteLine(csv.Count());

            var csvTea = InitializeTeaCSV("CsvFiles/TeasUsers.csv", ";");

            Console.WriteLine(csvTea[1].Id);
            Console.WriteLine(csvTea[1].FirstName);
            Console.WriteLine(csvTea[1].Email);
            Console.WriteLine(csvTea[1].Password);
            var csvPath = Path.Combine(Environment.CurrentDirectory, $"orders/orders-{DateTime.UtcNow.ToFileTime()}.csv");

            InsertOrder(csvPath, "012345");

            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();

            IWebElement languageButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='setInputLocaleCountry']")));

            //Thread.Sleep(1000);

            InsertOrder(csvPath, "0123456666");

            //IWebElement languageButton = FindElement(By.XPath(".//*[@id='setInputLocaleCountry']"), logger);
            languageButton.Click();
            logger.Debug("Language selected...");

            Thread.Sleep(100);
            IWebElement loginButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@data-tab='tab-login-cell']")));

            //IWebElement loginButton = FindElement(By.XPath(".//*[@class='force-hover font-normal js-login-form-show']"), logger);
            loginButton.Click();
            logger.Debug("Login button clicked...");
            
            IWebElement emailField = FindElement(By.XPath(".//*[@id='login-form-email']"), logger);
            emailField.SendKeys("test121233@yopmail.com");
            logger.Debug("Email field filled out...");

            InsertOrder(csvPath, "012345677777777");

            IWebElement passwordField = FindElement(By.XPath(".//*[@id='login-form-password']"), logger);
            passwordField.SendKeys("Test111?");
            logger.Debug("Password field filled out...");

            IWebElement finalLoginButton = FindElement(By.XPath(".//*[@class='button button-black mobile-extended wide']"), logger);
            finalLoginButton.Click();
            logger.Debug("Final Login button clicked...");

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
            //https://test.falconeri.com/us/product/DAL449A++8521M.html
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var csv = InitializeOrderInfoCSV("orderingCsv/orderInfo.csv", ";");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));

            Console.WriteLine(csv[0].Sku);
            Console.WriteLine(csv[0].Options);


            for(int i = 0; i < csv.Count; i++)
            {
                driver.Navigate().GoToUrl($"https://test.falconeri.com/us/product/{csv[i].Sku}++{csv[i].Options}.html");
                Thread.Sleep(1000);
                IWebElement addToBag = FindElement(By.XPath($".//*[@class='cell auto add-to-cart button button-addtocart']"), logger);
                Thread.Sleep(1000);
                addToBag.Click();
            }

            Thread.Sleep(1000);
            IWebElement bag = FindElement(By.XPath(".//*[@class='button extended uppercase button-black minicart-checkout-button']"), logger);
            Thread.Sleep(1000);
            bag.Click();

            Thread.Sleep(1000);
            IWebElement checkout = FindElement(By.XPath(".//*[@class='button button-black checkout-btn']"), logger);
            Thread.Sleep(1000);
            checkout.Click();

            
            Thread.Sleep(1000);
            IWebElement continueButton = FindElement(By.XPath(".//*[@class='button button-black submit-shipping wide fwidth-padding']"), logger);
            Thread.Sleep(1000);
            continueButton.Click();

            
            Thread.Sleep(1000);
            IWebElement emailInput = FindElement(By.XPath(".//*[@id='shippingEmail']"), logger);
            Thread.Sleep(1000);
            emailInput.SendKeys("soldo@soldo.com");

            
            
            Thread.Sleep(1000);
            IWebElement numberPrefix = FindElement(By.XPath(".//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch']"), logger);
            Thread.Sleep(1000);
            numberPrefix.Click();

            Thread.Sleep(1000);
            IWebElement numberSelect = FindElement(By.XPath(".//*[@data-option-array-index='2']"), logger);
            Thread.Sleep(1000);
            numberSelect.Click();


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
            Console.WriteLine(InitializeJson("soldoJson.json").Id);
            Console.WriteLine(InitializeJson("soldoJson.json").Name);
            Console.WriteLine(InitializeJson("soldoJson.json").LastName);
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
    }
}