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
using System.Text.RegularExpressions;

//using NunitVideoRecorder;

namespace SeleniumProj
{

    public class OrderInfo
    {
        public string Sku { get; set; }
        public string Options { get; set; }
    }

    public class JsonSetup
    {
        public List<JsonOrderInfo> orderSetup { get; set; }
        public List<IDictionary<string, string>> credentials { get; set; }
    }

    public class JsonOrderInfo
    {
        public List<string> locales { get; set; }
        public List<string> products { get; set; }
        public List<bool> userChoiceNewsletter { get; set; }
        public bool isRegistered { get; set; }
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
                Console.WriteLine("Error while finding element: {0}", xpath);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
            }
        }

        public void ImmediateButtonClickJS(IJavaScriptExecutor js,IWebElement element)
        {
            js.ExecuteScript("arguments[0].click();", element);
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

        public JsonSetup InitializeFalconeriSetup(string name)
        {
            StreamReader fstream = new StreamReader("C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/JsonFiles/" + name);


            var myjson = JsonSerializer.Deserialize<JsonSetup>(fstream.ReadToEnd(), new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

            return myjson;
        }

        public void InsertOrder(string csvp, string brand, string locale, string orderNum, string lastName, string shippingMethod, string paymentMethod, string paymentAmount, List<string> skuAndAttribute, bool isRegistered)
        {
            bool records = true;

            using (var stream = File.Open(csvp, FileMode.Append))
            {
                FileInfo fi = new FileInfo(csvp);
                if (fi.Length > 0)
                {
                    records = false;
                }
                using (var streamWriter = new StreamWriter(stream))
                {
                    //HasHeaderRecord = false,
                    using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = records, Delimiter = ";" }))
                    {
                        //var orders = Orders.GetOrders();
                        //csvWriter.WriteRecords(orders);
                        string listOfSkusAndAttributes = "";
                        listOfSkusAndAttributes = string.Join(",", skuAndAttribute.ToArray());
                        var order = Orders.AddOrder(brand, locale, orderNum, lastName, shippingMethod, paymentMethod, paymentAmount, listOfSkusAndAttributes, isRegistered);
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

        
        [Test()]
        public void EditProfile()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            var jsonSetup = InitializeFalconeriSetup("FalconeriSetupJson.json");
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            Actions action = new Actions(driver);

            for (int i = 0; i < jsonSetup.credentials.Count;i++)
            {
                if (i == 0)
                {
                    IWebElement languageButton = FindElement(By.XPath(".//*[@id='setInputLocaleCountry']"), logger);
                    languageButton.Click();
                    logger.Debug("Country selected...");
                    Thread.Sleep(1000);
                }

                TryAndClick(".//*[@data-tab='tab-login-cell']", 15);
                IWebElement emailText = FindElement(By.XPath(".//*[@id='login-form-email']"), logger);
                IWebElement passwordText = FindElement(By.XPath(".//label[@class='label-inside']/input[@type='password']"), logger);
                IWebElement loginButton = FindElement(By.XPath(".//*[@class='button button-black mobile-extended wide']"), logger);

                logger.Debug("Already registered clicked...");
                emailText.SendKeys(jsonSetup.credentials[i]["userEmail"]);
                logger.Debug("Mail entered...");
                passwordText.SendKeys(jsonSetup.credentials[i]["userPassword"]);
                logger.Debug("Password entered...");
                loginButton.Click();
                logger.Debug("Loging in...");

                IWebElement myData = FindElement(By.XPath(".//a[@href ='/us/myprofile/']/div"), logger);
                myData.Click();
                logger.Debug("My data clicked...");

                IWebElement modifyData = FindElement(By.XPath(".//*[@class='show-for-large navigation-links']"),logger);
                modifyData.Click(); 

                IWebElement userGender = FindElement(By.XPath($".//label[@for='gender-{jsonSetup.credentials[i]["userGender"]}']/span[@class='radio-input']"), logger);
                IWebElement userName = FindElement(By.XPath(".//*[@id='firstName']"), logger);
                IWebElement userLastName = FindElement(By.XPath(".//*[@id='lastName']"), logger);
                IWebElement userNumber = FindElement(By.XPath(".//*[@id='phone']"), logger);
                IWebElement userMonthClicked = FindElement(By.XPath("(.//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch'])[1]"), logger);
                IWebElement userDayClicked= FindElement(By.XPath("(.//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch'])[2]"), logger);
                IWebElement userYearClicked = FindElement(By.XPath("(.//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch'])[3]"), logger);

                userGender.Click();
                userName.Clear();
                userName.SendKeys(jsonSetup.credentials[i]["userName"]);
                userLastName.Clear();
                userLastName.SendKeys(jsonSetup.credentials[i]["userLastName"]);
                userNumber.Clear(); 
                userNumber.SendKeys(jsonSetup.credentials[i]["userNumber"]);
                userMonthClicked.Click();
                IWebElement userBirthMonth = FindElement(By.XPath($"(.//li[@data-option-array-index='{jsonSetup.credentials[i]["userBirthMonth"]}'])[1]"), logger);
                userBirthMonth.Click();
                userDayClicked.Click(); 
                IWebElement userBirthDay = FindElement(By.XPath($"(.//li[@data-option-array-index='{jsonSetup.credentials[i]["userBirthDay"]}'])[2]"), logger);
                userBirthDay.Click();

                userYearClicked.Click();
                IWebElement userBirthYear= FindElement(By.XPath($"(.//li[@data-option-array-index='{jsonSetup.credentials[i]["userBirthYear"]}'])[2]"), logger);
                userBirthYear.Click(); 

                IWebElement SaveButton = driver.FindElement(By.XPath(".//*[@name='save']"));
                SaveButton.Click();

            }
            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Falconeri testing");
        } 
       

        [Test()]
        public void RegistrationWithCSVFile()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            var csvUserInformation = InitializeUsersCSV($"C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/CsvFiles/TeasUsersSecond.csv", ";");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            for (int i = 2; i < 3; i++)
            {
                if (i == 2)
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
                IWebElement phoneNumber = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='registration-form-phone']")));
                IWebElement goButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@class='button wide mobile-extended button-black registerPhoneButton']")));
                numberSelectorButton.Click();
                logger.Debug("Number selector clicked...");
                IWebElement numberSelect = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@data-option-array-index='1']")));
                numberSelect.Click();
                logger.Debug("Number chosen...");
                phoneNumber.SendKeys(csvUserInformation[i].Phone);
                logger.Debug("Phone number entered...");
                goButton.Click();
                logger.Debug("Go button clicked...");

                IWebElement genderButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@for='gender-4']")));
                IWebElement firstName = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='registration-form-fname']")));
                IWebElement lastName = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='registration-form-lname']")));
                IWebElement password = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='registration-form-password']")));
                IWebElement passwordRepeat = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='registration-form-password-confirm']")));
                IWebElement recevingNews = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@for='add-to-email-list-no']")));
                IWebElement registrationForFalconeriLoyalty = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@for='loyalty-no']")));
                IWebElement registrationButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@class='button wide button-register-final button-black']")));
               
                genderButton.Click();
                logger.Debug("Gender selected...");
                firstName.SendKeys(csvUserInformation[i].FirstName);
                logger.Debug("First name entered...");
                lastName.SendKeys(csvUserInformation[i].LastName);
                logger.Debug("Last name entered...");
                password.SendKeys(csvUserInformation[i].Password);
                logger.Debug("Password entered...");
                passwordRepeat.SendKeys(csvUserInformation[i].Password);
                logger.Debug("Repeated password entered...");
                recevingNews.Click();
                logger.Debug("Receving news choice selected...");
                registrationForFalconeriLoyalty.Click();
                logger.Debug("Subscribing for Falconeri Loyalty...");
                Actions action = new Actions(driver);
                action.MoveToElement(registrationButton).Perform();
                registrationButton.Click();
                logger.Debug("registration button clicked...");

                Thread.Sleep(1000);
                driver.Navigate().GoToUrl($"https://test.falconeri.com/us/login");
            }

            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Falconeri testing");
        }

        [Test()]
        public void AcceptingRegistration()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            var csvUserInformation = InitializeUsersCSV($"C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/CsvFiles/TeasUsersSecond.csv", ";");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            js.ExecuteScript("window.open('https://yopmail.com/en/wm', 'Newest Window', null)");
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            Thread.Sleep(1000);
            IWebElement cookies = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='accept']")));
            ImmediateButtonClickJS(js, cookies);

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
            string pattern = @"[A-Z a-z ü !]+";
            string orderText = text;
            //"Thank you for your order 123213213213!"

            // Create a Regex
            Regex rg = new Regex(pattern);
            string orderNumber = Regex.Replace(orderText, pattern, "");
            return orderNumber;
        }

        public string GetOrderAmount(string text)
        {
            // Create a pattern for a word that starts with letter "M"  
            string pattern = @"[0-9,0-9 $ € !]+";
            string orderText = text;
            //"Thank you for your order 123213213213!"

            // Create a Regex
            Regex rg = new Regex(pattern);
            string orderAmount = Regex.Replace(orderText, pattern, "");
            Console.WriteLine(orderAmount);
            return orderAmount;
        }

        [Test()]
        public void OrderingWithCreditCardExsp()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var csv = InitializeOrderInfoCSV("C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orderingCsv/orderInfo.csv", ";");
            var jsonSetup = InitializeFalconeriSetup("FalconeriSetupJson.json");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            List<string> skuAndAttributes = new List<string>();

            for (int i = 0; i < jsonSetup.orderSetup.Count; i++)
            {
                bool isLoggedIn = jsonSetup.orderSetup[i].isRegistered;
                DateTime currentTime = DateTime.Now;

                for (int j = 0; j < jsonSetup.orderSetup[i].locales.Count; j++)
                {
                    for (int k= 0; k < jsonSetup.credentials.Count; k++)
                    {
                        skuAndAttributes.Clear();
                        if (i == 0 && j == 0 && k == 0)
                        {
                            #region LogingScreenFindElements
                            IWebElement languageButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='setInputLocaleCountry']")));
                            #endregion

                            #region LogingScreenClick
                            languageButton.Click();
                            logger.Debug("Language selected...");
                            #endregion
                        }
                        #region AddingProductsToBag

                        Thread.Sleep(1000);
                        //products.Count
                        for (int l = 0; l < jsonSetup.orderSetup[i].products.Count; l++)
                        {
                            //driver.Navigate().GoToUrl($"https://test.falconeri.com/{locales[j]}/product/{csv[l].Sku}++{csv[l].Options}.html");
                            driver.Navigate().GoToUrl($"https://test.falconeri.com/{jsonSetup.orderSetup[i].locales[j]}/product/{jsonSetup.orderSetup[i].products[l]}.html");
                            #region FindElementInProducts
                            IWebElement addToBag = FindElement(By.XPath($".//*[@class='cell auto add-to-cart button button-addtocart']"), logger);
                            #endregion

                            #region ClickElementInProducts
                            addToBag.Click();
                            #endregion

                            skuAndAttributes.Add($"{jsonSetup.orderSetup[i].products[l]}");
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
                        Thread.Sleep(500);
                        TryAndClick(".//*[@class='button button-black submit-shipping wide fwidth-padding']", 5);
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
                        IWebElement shippingState = FindElement(By.XPath(".//*[@id='shippingState_chosen']"), logger);

                        #endregion

                        #region ClickingAndSendingKeysFirstScreen

                        emailInput.SendKeys(jsonSetup.credentials[k]["userEmail"]);
                        TryAndClick(".//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch']", 10);
                        TryAndClick($".//*[@data-option-array-index='{jsonSetup.credentials[k]["userNumberPrefix"]}']", 10);
                        TryAndClick(".//*[@id='shippingPhoneNumber']", 10);
                        numberInput.SendKeys(jsonSetup.credentials[k]["userNumber"]);

                        if (jsonSetup.orderSetup[i].userChoiceNewsletter[0])
                        {
                            newsButton.Click();
                            newsButtonWithProfile.Click();
                        }

                        firstNameInput.SendKeys(jsonSetup.credentials[k]["userName"]);
                        lastNameInput.SendKeys(jsonSetup.credentials[k]["userSurname"]);
                        addressInput.SendKeys(jsonSetup.credentials[k]["userAddress"]);
                        addressOtherInfoInput.SendKeys(jsonSetup.credentials[k]["userAdditionalAddressInfo"]);
                        townInput.SendKeys(jsonSetup.credentials[k]["userTown"]);
                        zipInput.SendKeys(jsonSetup.credentials[k]["userZIP"]);

                        shippingState.Click();
                        shippingState.Click();
                        TryAndClick($"(.//*[@data-option-array-index='{jsonSetup.credentials[k]["userState"]}'])[2]", 15);

                        IWebElement countryButton = FindElement(By.XPath(".//*[@for='shippingCountry']"), logger);
                        countryButton.Click();
                        TryAndClick($"(.//*[@data-option-array-index='{jsonSetup.credentials[k]["userCountry"]}'])[3]", 15);
                        continueButtonOntoPayment.Click();
                        #endregion


                        #region FindElementsSecondScreen
                        IWebElement nameOnCardInput = FindElement(By.XPath(".//*[@id='cardOwner']"), logger);
                        IWebElement cardNumberInput = FindElement(By.XPath(".//*[@id='cardNumber']"), logger);
                        IWebElement cardCVVInput = FindElement(By.XPath(".//*[@id='securityCode']"), logger);
                        IWebElement acceptingTermsButton = FindElement(By.XPath("(.//*[@class='checkbox-input'])[6]"), logger);
                      

                        #endregion

                        #region ClickingAndSendingKeysSecondScreen

                        nameOnCardInput.SendKeys(jsonSetup.credentials[k]["creditCardName"]);
                        cardNumberInput.SendKeys(jsonSetup.credentials[k]["creditCardNumber"]);
                        TryAndClick(".//*[@for='expirationMonth']", 15);
                        TryAndClick($"(.//*[@data-option-array-index='{jsonSetup.credentials[k]["creditCardMonth"]}'])[3]", 15);
                        IWebElement yearChoice = FindElement(By.XPath(".//*[@for='expirationYear']"), logger);
                        yearChoice.Click();
                        IWebElement yearButton = FindElement(By.XPath($"(.//*[@data-option-array-index='{jsonSetup.credentials[k]["creditCardYear"]}'])[4]"), logger);
                        yearButton.Click();
                        cardCVVInput.SendKeys(jsonSetup.credentials[k]["creditCardCVV"]);
                        acceptingTermsButton.Click();
                        TryAndClick("(.//*[@name='submit'])[3]", 10);

                        #endregion

                        #region Login 

                        if (isLoggedIn)
                        {
                            #region FindElementsThirdScreen
                            IWebElement passwordForLogin = FindElement(By.XPath("(.//*[@name='loginPassword'])[2]"), logger);
                            IWebElement loginButton = FindElement(By.XPath(".//*[@id='login']"), logger);
                            #endregion

                            #region ClickingAndSendingKeysThirdScreen
                            passwordForLogin.SendKeys(jsonSetup.credentials[k]["userPassword"]);
                            loginButton.Click();

                            #endregion
                        }


                        #endregion

                        #region SavingOrderInfo

                        IWebElement orderText = FindElement(By.XPath(".//*[@class='cell order-thank-you-msg h4 side-margins receipt-title']"), logger);
                        string str = orderText.Text;

                        IWebElement orderTextAmount = FindElement(By.XPath(".//*[@class='grand-total-sum']"), logger);
                        string orderPaymentAmountText = orderTextAmount.Text;

                        InsertOrder($"C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orders/creditcard/orders-creditcard-{currentTime.Hour}-{currentTime.Minute}-{currentTime.Second}--{currentTime.Day}-{currentTime.Month}-{currentTime.Year}.csv", "Falconeri", jsonSetup.orderSetup[i].locales[j], GetOrderNumber(str), jsonSetup.credentials[k]["userSurname"], "Standard Shipping", "Credit card", orderPaymentAmountText, skuAndAttributes, isLoggedIn);
                        #endregion
                    }
                }
            }
            #region TestPassed
            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Falconeri testing");
            #endregion
        }


        [Test()]
        public void OrderingWithPayPalExsp()
        {
            #region Init
            Actions action = new Actions(driver);
            //https://test.falconeri.com/us/product/DAL449A++8521M.html
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var csv = InitializeOrderInfoCSV("C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orderingCsv/orderInfo.csv", ";");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            var jsonSetup = InitializeFalconeriSetup("FalconeriSetupJson.json");
            #endregion
            List<string> skuAndAttributes = new List<string>();

            for (int i = 0; i < jsonSetup.orderSetup.Count; i++)
            {

                bool isLoggedIn = jsonSetup.orderSetup[i].isRegistered;
                DateTime currentTime = DateTime.Now;

                for (int j = 0; j < jsonSetup.orderSetup[i].locales.Count; j++)  
                {
                    for (int k = 0; k < jsonSetup.credentials.Count; k++)
                    {
                        skuAndAttributes.Clear();
                        if (i == 0 && j == 0 && k==0)
                        {
                            #region LogingScreenFindElements
                            IWebElement languageButton = wait.Until(ExpectedConditions.ElementExists(By.XPath(".//*[@id='setInputLocaleCountry']")));
                            #endregion

                            #region LogingScreenClick
                            languageButton.Click();
                            logger.Debug("Language selected...");
                            #endregion
                        }

                        #region AddingProductsToBag

                        for (int l = 0; l < jsonSetup.orderSetup[i].products.Count; l++)
                        {
                            Thread.Sleep(1000);
                            driver.Navigate().GoToUrl($"https://test.falconeri.com/{jsonSetup.orderSetup[i].locales[j]}/product/{jsonSetup.orderSetup[i].products[l]}.html");
                            #region FindElementInProducts
                            IWebElement addToBag = FindElement(By.XPath($".//*[@class='cell auto add-to-cart button button-addtocart']"), logger);
                            #endregion

                            #region ClickElementInProducts
                            addToBag.Click();
                            #endregion
                            skuAndAttributes.Add($"{jsonSetup.orderSetup[i].products[l]}");
                        }

                        #endregion

                        #region FindElementShoppingBag
                        IWebElement bag = FindElement(By.XPath(".//*[@class='button extended uppercase button-black minicart-checkout-button']"), logger);
                        #endregion

                        #region ClickingElementShoppingBag
                        bag.Click();
                        #endregion

                        #region FindElementContinueToCheckOut
                        IWebElement checkout = FindElement(By.XPath(".//*[@class='button button-black checkout-btn']"), logger);
                        #endregion

                        #region ClickElementContinueToCheckOut
                        checkout.Click();
                        #endregion

                        #region FindContinueToShippingMethod
                        Thread.Sleep(500);
                        TryAndClick(".//*[@class='button button-black submit-shipping wide fwidth-padding']", 10);
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
                        IWebElement shippingState = FindElement(By.XPath(".//*[@id='shippingState_chosen']"), logger);
                        #endregion

                        #region ClickingAndSendingKeysFirstScreen
                        emailInput.SendKeys(jsonSetup.credentials[k]["userEmail"]);
                        TryAndClick(".//*[@class='chosen-container chosen-container-single chosen-container-single-nosearch']", 10);
                        Thread.Sleep(500);
                        TryAndClick($".//*[@data-option-array-index='{jsonSetup.credentials[k]["userNumberPrefix"]}']", 10);
                        TryAndClick(".//*[@id='shippingPhoneNumber']", 10);
                        numberInput.SendKeys(jsonSetup.credentials[k]["userNumber"]);

                        if (jsonSetup.orderSetup[i].userChoiceNewsletter[0])
                        {
                            newsButton.Click();
                            newsButtonWithProfile.Click();
                        }

                        firstNameInput.SendKeys(jsonSetup.credentials[k]["userName"]);
                        lastNameInput.SendKeys(jsonSetup.credentials[k]["userSurname"]);
                        addressInput.SendKeys(jsonSetup.credentials[k]["userAddress"]);
                        addressOtherInfoInput.SendKeys(jsonSetup.credentials[k]["userAdditionalAddressInfo"]);
                        townInput.SendKeys(jsonSetup.credentials[k]["userTown"]);
                        zipInput.SendKeys(jsonSetup.credentials[k]["userZIP"]);
                        shippingState.Click();
                        shippingState.Click();
                        TryAndClick($"(.//*[@data-option-array-index='{jsonSetup.credentials[k]["userState"]}'])[2]", 10);
                        IWebElement countryButton = FindElement(By.XPath(".//*[@for='shippingCountry']"), logger);
                        countryButton.Click();
                        IWebElement countryChoiceButton = FindElement(By.XPath($"(.//*[@data-option-array-index='{jsonSetup.credentials[k]["userCountry"]}'])[3]"), logger);
                        countryChoiceButton.Click();
                        continueButtonOntoPayment.Click();
                        #endregion

                        #region FindPayPal
                        //TryAndClick(".//label[@for='PayPal']/span[@class='radio-input']", 15);
                        IWebElement paypalButton = FindElement(By.XPath(".//label[@for='PayPal']/span[@class='radio-input']"), logger);
                        #endregion

                        #region ClickPaypal
                        ImmediateButtonClickJS(js, paypalButton);
                        #endregion

                        #region ClickElementsInPayPal
                        Thread.Sleep(500);
                        TryAndClick("(.//*[@class='checkbox-input'])[7]", 15);
                        TryAndClick(".//*[@class='paypal-checkout-button js_paypal_button_on_billing_form']", 15);
                        #endregion

                        driver.SwitchTo().Window(driver.WindowHandles.Last());
                        driver.Manage().Window.Maximize();
                        Thread.Sleep(1000);

                        if (i == 0 && j==0 && k == 0)
                        {
                            #region FindElementsThirdScreen
                            IWebElement acceptingCookies = FindElement(By.XPath(".//*[@id='acceptAllButton']"), logger);
                            IWebElement payPalEmail = FindElement(By.XPath(".//*[@id='email']"), logger);
                            IWebElement payPalPassword = FindElement(By.XPath(".//*[@id='password']"), logger);
                            #endregion

                            #region ClickingAndSendingKeysThirdScreen
                            payPalEmail.Clear();
                            payPalEmail.SendKeys(jsonSetup.credentials[k]["paypalEmail"]);
                            payPalPassword.SendKeys(jsonSetup.credentials[k]["paypalPassword"]);
                            ImmediateButtonClickJS(js, acceptingCookies);
                            TryAndClick(".//*[@id='btnLogin']", 15);
                            #endregion
                        }


                        #region ClickingForthScreen

                        IWebElement continueButton = wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(".//*[@id='payment-submit-btn']")));

                        js.ExecuteScript("scroll(800, 800)");
                        ImmediateButtonClickJS(js, continueButton);
                        #endregion

                        driver.SwitchTo().Window(driver.WindowHandles.First());

                        #region Login 

                        if (isLoggedIn)
                        {
                            #region FindElementsFinalScreen
                            IWebElement passwordForLogin = FindElement(By.XPath("(.//*[@name='loginPassword'])[2]"), logger);
                            #endregion

                            #region ClickingAndSendingKeysFinalScreen
                            passwordForLogin.SendKeys(jsonSetup.credentials[k]["userPassword"]);
                            TryAndClick(".//*[@id='login']", 15);

                            #endregion
                        }

                        #endregion


                        #region SavingOrderInfo

                        IWebElement orderText = FindElement(By.XPath(".//*[@class='cell order-thank-you-msg h4 side-margins receipt-title']"), logger);
                        string str = orderText.Text;

                        IWebElement orderTextAmount = FindElement(By.XPath(".//*[@class='grand-total-sum']"), logger);
                        string orderPaymentAmountText = orderTextAmount.Text;
                        InsertOrder($"C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orders/paypal/orders-paypal-{currentTime.Hour}-{currentTime.Minute}-{currentTime.Second}--{currentTime.Day}-{currentTime.Month}-{currentTime.Year}.csv", "Falconeri", jsonSetup.orderSetup[i].locales[j], GetOrderNumber(str), jsonSetup.credentials[k]["userSurname"], "Standard Shipping", "PayPal", orderPaymentAmountText, skuAndAttributes, isLoggedIn);
                        #endregion
                    }

                }

            }
            #region TestPassed
            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Falconeri testing");
            #endregion

        }
    }
}