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

namespace SeleniumProj
{
    public class UsersIntimissimi
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhonePrefix { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string userNewsChoice { get; set; }
        public string userPersonalizedNews { get; set; }
        public string LoyaltySubscription { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string BirthDate { get; set; }

    }
    public class JsonOrderIntimissimiInfo
    {
        public List<string> locales { get; set; }
        public List<string> products { get; set; }
        public List<bool> userChoiceNewsletter { get; set; }
        public bool isRegistered { get; set; }
    }
    public class JsonIntimissimiSetup
    {
        public List<JsonOrderInfo> orderSetup { get; set; }
        public List<IDictionary<string, string>> credentials { get; set; }
    }
    public class JsonRegistrationSetup
    {
        public List<IDictionary<string, string>> registrationSetup { get; set; }
    }


    [TestFixture()]
    public class Intimissimi : BaseIntimissimi
    {
        public JsonSetup InitializeIntimissimiSetup(string name)
        {
            StreamReader fstream = new StreamReader("C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/JsonFiles/" + name);


            var myjson = JsonSerializer.Deserialize<JsonSetup>(fstream.ReadToEnd(), new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

            return myjson;
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
                   
                    using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture) { HasHeaderRecord = records, Delimiter = ";" }))
                    {
                        string listOfSkusAndAttributes = "";
                        listOfSkusAndAttributes = string.Join(",", skuAndAttribute.ToArray());
                        var order = Orders.AddOrder(brand, locale, orderNum, lastName, shippingMethod, paymentMethod, paymentAmount, listOfSkusAndAttributes, isRegistered);
                        csvWriter.WriteRecords(order);
                    }
                }
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

        public void ImmediateButtonClickJS(IJavaScriptExecutor js, IWebElement element)
        {
            js.ExecuteScript("arguments[0].click();", element);
        }

        public JsonRegistrationSetup InitializeIntimissimiRegistration(string name)
        {
            StreamReader fstream = new StreamReader("C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/JsonFiles/" + name);


            var myjson = JsonSerializer.Deserialize<JsonRegistrationSetup>(fstream.ReadToEnd(), new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping });

            return myjson;
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
        public List<UsersIntimissimi> InitializeUsersCSV(string path, string delimiter)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter,
            };



            var reader = new StreamReader(path);
            var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<UsersIntimissimi>().ToList();



            return records;
        }

        [Test()]
        public void RegistrationWithJson()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            var jsonRegistration = InitializeIntimissimiRegistration("registrationINTJson.json");
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            for (int i = 0; i < jsonRegistration.registrationSetup.Count; i++)
            {
                if (i == 0)
                {
                    IWebElement languageButton = FindElement(By.XPath(".//*[@id='setInputLocaleCountry']"), logger);
                   
                    languageButton.Click();
                    logger.Debug("Language selected...");
                    TryAndClick(".//*[@id='cc-approve-button-thissite']", 15);
                    logger.Debug("Accepting cookies clicked...");
                   
                }
                  
                IWebElement registerButton = FindElement(By.XPath(".//*[@class='tab-link js-link-register']"), logger);
                IWebElement emailField = FindElement(By.XPath(".//*[@id='registration-form-email']"), logger);
                //IWebElement secondRegisterButton = FindElement(By.XPath("(.//*[@class='button wide extended button-register-margin-40 ae-button'])[1]"), logger);

                registerButton.Click();
                logger.Debug("Register button clicked...");

                if (i == 0)
                {
                    IWebElement geoPopUp = FindElement(By.XPath(".//*[@id='geoblock-close']"), logger);
                    geoPopUp.Click();
                    logger.Debug("Geo popup clicked...");

                }

                emailField.SendKeys(jsonRegistration.registrationSetup[i]["userEmail"]);
                logger.Debug("Email field filled...");
                TryAndClick("(.//*[@class='button wide extended button-register-margin-40 ae-button'])[1]", 15);
                logger.Debug("Second register button clicked...");

                IWebElement numberPrefix = FindElement(By.XPath(".//*[@id='registration-form-prefix']"), logger);
                IWebElement numberInput = FindElement(By.XPath(".//*[@id='registration-form-phone']"), logger);
                IWebElement continueButton = FindElement(By.XPath(".//*[@class='button wide extended registerPhoneButton ae-button']"), logger);

                numberPrefix.Clear();
                numberPrefix.SendKeys(jsonRegistration.registrationSetup[i]["userNumberPrefix"]);
                logger.Debug("Number prefix filled...");
                numberInput.SendKeys(jsonRegistration.registrationSetup[i]["userNumber"]);
                logger.Debug("Number filled...");
                continueButton.Click();
                logger.Debug("Continue button clicked...");

                IWebElement userGender = FindElement(By.XPath($".//label[@for='gender-{jsonRegistration.registrationSetup[i]["userGender"]}']/span[@class='radio-input']"), logger);
                IWebElement userName = FindElement(By.XPath(".//*[@id='registration-form-fname']"), logger);
                IWebElement userLastName = FindElement(By.XPath(".//*[@id='registration-form-lname']"), logger);
                IWebElement userPassword = FindElement(By.XPath(".//*[@id='registration-form-password']"), logger);
                IWebElement userPasswordConfirm = FindElement(By.XPath(".//*[@id='registration-form-password-confirm']"), logger);
                IWebElement userNewsChoice = FindElement(By.XPath($".//label[@for='add-to-email-list-{jsonRegistration.registrationSetup[i]["userNewsChoice"]}']/span[@class='radio-input']"), logger);
                IWebElement userPersonalizedNews = FindElement(By.XPath($".//label[@for='add-to-personalized-news-list-{jsonRegistration.registrationSetup[i]["userPersonalizedNews"]}']/span[@class='radio-input']"), logger);
                IWebElement userLoyalty = FindElement(By.XPath($".//label[@for='loyalty-{jsonRegistration.registrationSetup[i]["userLoyalty"]}']/span[@class='radio-input']"), logger);
                IWebElement userBirthDate = FindElement(By.XPath(".//*[@id='dpReg']"), logger);
                IWebElement confirmingLoylatyButton = FindElement(By.XPath(".//label[@for='opposition']/span[@class='checkbox-input']"), logger);
                IWebElement lastRegisterButton = FindElement(By.XPath(" .//*[@class='button wide button-register-final ae-button']"), logger);

                userGender.Click();
                logger.Debug("Gender choosen...");
                userName.SendKeys(jsonRegistration.registrationSetup[i]["userName"]);
                logger.Debug("Name entered...");
                userLastName.SendKeys(jsonRegistration.registrationSetup[i]["userLastName"]);
                logger.Debug("Last name entered...");
                userPassword.SendKeys(jsonRegistration.registrationSetup[i]["userPassword"]);
                logger.Debug("Password entered...");
                userPasswordConfirm.SendKeys(jsonRegistration.registrationSetup[i]["userPassword"]);
                logger.Debug("Confirm password entered...");
                userNewsChoice.Click();
                logger.Debug("News choice clicked...");
                userPersonalizedNews.Click();
                logger.Debug("Personolized news choice clicked...");
                userLoyalty.Click();
                logger.Debug("Loyalty choice clicked...");

                if (jsonRegistration.registrationSetup[i]["userLoyalty"].Equals("yes"))
                {
                    userBirthDate.SendKeys(jsonRegistration.registrationSetup[i]["userBirthDate"]);
                    logger.Debug("Birth date entered...");
                    confirmingLoylatyButton.Click();
                    logger.Debug("Confirming loyalty clicked...");
                }

                lastRegisterButton.Click();
                logger.Debug("Last register button clicked...");
                driver.Url = "https://test.intimissimi.com/us/login";
                driver.Navigate().GoToUrl("https://test.intimissimi.com/us/login"); 
            }

            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Intimissimi testing");
        }
        [Test()]
        public void RegistrationWithCSVFile()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            var csvUserInformation = InitializeUsersCSV($"C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/CsvFiles/UsersIntimissimi.csv", ";");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            for (int i = 0; i < csvUserInformation.Count; i++)
            {
                if (i == 0)
                {
                    IWebElement languageButton = FindElement(By.XPath(".//*[@id='setInputLocaleCountry']"), logger);

                    languageButton.Click();
                    logger.Debug("Language selected...");
                    TryAndClick(".//*[@id='cc-approve-button-thissite']", 15);
                    logger.Debug("Accepting cookies clicked...");

                }

                IWebElement registerButton = FindElement(By.XPath(".//*[@class='tab-link js-link-register']"), logger);
                IWebElement emailField = FindElement(By.XPath(".//*[@id='registration-form-email']"), logger);
               
                registerButton.Click();
                logger.Debug("Register button clicked...");

                if (i == 0)
                {
                    IWebElement geoPopUp = FindElement(By.XPath(".//*[@id='geoblock-close']"), logger);
                    geoPopUp.Click();
                    logger.Debug("Geo popup clicked...");

                }

                emailField.SendKeys(csvUserInformation[i].Email);
                logger.Debug("Email field filled...");
                TryAndClick("(.//*[@class='button wide extended button-register-margin-40 ae-button'])[1]", 15);
                logger.Debug("Second register button clicked...");

                IWebElement numberPrefix = FindElement(By.XPath(".//*[@id='registration-form-prefix']"), logger);
                IWebElement numberInput = FindElement(By.XPath(".//*[@id='registration-form-phone']"), logger);
                IWebElement continueButton = FindElement(By.XPath(".//*[@class='button wide extended registerPhoneButton ae-button']"), logger);

                numberPrefix.Clear();
                numberPrefix.SendKeys(csvUserInformation[i].PhonePrefix);
                logger.Debug("Number prefix filled...");
                numberInput.SendKeys(csvUserInformation[i].Phone);
                logger.Debug("Number filled...");
                continueButton.Click();
                logger.Debug("Continue button clicked...");

                IWebElement userGender = FindElement(By.XPath($".//label[@for='gender-{csvUserInformation[i].Gender}']/span[@class='radio-input']"), logger);
                IWebElement userName = FindElement(By.XPath(".//*[@id='registration-form-fname']"), logger);
                IWebElement userLastName = FindElement(By.XPath(".//*[@id='registration-form-lname']"), logger);
                IWebElement userPassword = FindElement(By.XPath(".//*[@id='registration-form-password']"), logger);
                IWebElement userPasswordConfirm = FindElement(By.XPath(".//*[@id='registration-form-password-confirm']"), logger);
                IWebElement userNewsChoice = FindElement(By.XPath($".//label[@for='add-to-email-list-{csvUserInformation[i].userNewsChoice}']/span[@class='radio-input']"), logger);
                IWebElement userPersonalizedNews = FindElement(By.XPath($".//label[@for='add-to-personalized-news-list-{csvUserInformation[i].userPersonalizedNews}']/span[@class='radio-input']"), logger);
                IWebElement userLoyalty = FindElement(By.XPath($".//label[@for='loyalty-{csvUserInformation[i].LoyaltySubscription}']/span[@class='radio-input']"), logger);
                IWebElement userBirthDate = FindElement(By.XPath(".//*[@id='dpReg']"), logger);
                IWebElement confirmingLoylatyButton = FindElement(By.XPath(".//label[@for='opposition']/span[@class='checkbox-input']"), logger);
                IWebElement lastRegisterButton = FindElement(By.XPath(" .//*[@class='button wide button-register-final ae-button']"), logger);

                userGender.Click();
                logger.Debug("Gender choosen...");
                userName.SendKeys(csvUserInformation[i].FirstName);
                logger.Debug("Name entered...");
                userLastName.SendKeys(csvUserInformation[i].LastName);
                logger.Debug("Last name entered...");
                userPassword.SendKeys(csvUserInformation[i].Password);
                logger.Debug("Password entered...");
                userPasswordConfirm.SendKeys(csvUserInformation[i].Password);
                logger.Debug("Confirm password entered...");
                userNewsChoice.Click();
                logger.Debug("News choice clicked...");
                userPersonalizedNews.Click();
                logger.Debug("Personolized news choice clicked...");
                userLoyalty.Click();
                logger.Debug("Loyalty choice clicked...");

                if (csvUserInformation[i].LoyaltySubscription.Equals("yes"))
                {
                    userBirthDate.SendKeys(csvUserInformation[i].BirthDate);
                    logger.Debug("Birth date entered...");
                    confirmingLoylatyButton.Click();
                    logger.Debug("Confirming loyalty clicked...");
                }

                lastRegisterButton.Click();
                logger.Debug("Last register button clicked...");
                driver.Url = "https://test.intimissimi.com/us/login";
                driver.Navigate().GoToUrl("https://test.intimissimi.com/us/login");
            }

            logger.Debug("Test finished!");
            NLog.LogManager.Shutdown();
            Assert.Pass("Intimissimi testing");
        }


        [Test()]
        public void OrderWithCreditCardStandardShipping()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var csv = InitializeOrderInfoCSV("C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orderingCsv/orderInfo.csv", ";");
            var jsonSetup = InitializeIntimissimiSetup("IntimissimiSetupJson.json");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            List<string> skuAndAttributes = new List<string>();

            for(int i = 0; i < jsonSetup.orderSetup.Count; i++)
            {
                bool isLoggedIn = jsonSetup.orderSetup[i].isRegistered;
                DateTime currentTime = DateTime.Now; 

                for(int j = 0; j < jsonSetup.orderSetup[i].locales.Count; j++)
                {
                    for(int k = 0; k < jsonSetup.credentials.Count; k++)
                    {
                        skuAndAttributes.Clear();
                        if (i == 0 && j == 0 && k ==0)
                        {
                            IWebElement languageButton = FindElement(By.XPath(".//*[@id='setInputLocaleCountry']"), logger);
                            languageButton.Click();
                            logger.Debug("Language selected...");
                            TryAndClick(".//*[@id='cc-approve-button-thissite']", 15);
                            logger.Debug("Accepting cookies clicked...");
                            TryAndClick(".//*[@id='geoblock-close']", 15);
                            logger.Debug("GeoLocator clicked...");
                        }

                        for (int l = 0; l < jsonSetup.orderSetup[i].products.Count; l++)
                        {
                            driver.Navigate().GoToUrl($"https://test.intimissimi.com//{jsonSetup.orderSetup[i].locales[j]}/product/{jsonSetup.orderSetup[i].products[l]}.html");
                            #region FindElementInProducts
                            IWebElement addToBag = FindElement(By.XPath(".//*[@class='cell large-10 medium-10 small-5 add-to-cart button ae-button']"), logger);
                            #endregion

                            #region ClickElementInProducts
                            addToBag.Click();
                            #endregion

                            skuAndAttributes.Add($"{jsonSetup.orderSetup[i].products[l]}");
                        }

                        IWebElement shoppingBag = FindElement(By.XPath(".//*[@data-analytic-name='bag']"), logger);
                        shoppingBag.Click();

                        IWebElement continueButtonToShipping = FindElement(By.XPath("(.//*[@id='checkout-button-secure-checkout'])[2]"), logger);
                        ImmediateButtonClickJS(js, continueButtonToShipping);

                        Thread.Sleep(1000);
                        TryAndClick(".//*[@value='submit-shipping']",15);

                        IWebElement emailInput = FindElement(By.XPath(".//*[@id='shippingEmail']"), logger);
                        IWebElement numberPrefixInput = FindElement(By.XPath(".//*[@id='shippingPhoneNumberPrefix']"), logger);
                        IWebElement numberInput = FindElement(By.XPath(".//*[@id='shippingPhoneNumber']"), logger);
                        IWebElement newsButton = FindElement(By.XPath("(.//*[@class='slider round'])[1]"), logger);
                        IWebElement newsButtonWithProfile = FindElement(By.XPath("(.//*[@class='slider round'])[2]"), logger);
                        IWebElement firstNameInput = FindElement(By.XPath(".//*[@id='shippingFirstName']"), logger);
                        IWebElement lastNameInput = FindElement(By.XPath(".//*[@id='shippingLastName']"), logger);
                        IWebElement addressInput = FindElement(By.XPath(".//*[@id='shippingAddressOne']"), logger);
                        IWebElement addressOtherInfoInput = FindElement(By.XPath(".//*[@id='shippingAddressTwo']"), logger);
                        IWebElement townInput = FindElement(By.XPath(".//*[@id='shippingAddressCity']"), logger);
                        IWebElement zipInput = FindElement(By.XPath(".//*[@id='shippingZipCode']"), logger);
                        IWebElement shippingState = FindElement(By.XPath(".//div[@id='shippingState_chosen']/a[@class='chosen-single']"), logger);
                        IWebElement countryButton = FindElement(By.XPath(".//div[@id='shippingCountry_chosen']/a[@class='chosen-single']"), logger);
                        IWebElement continueButtonOntoPayment = FindElement(By.XPath(".//*[@value='submit-shipping']"), logger);

                        emailInput.Clear(); 
                        emailInput.SendKeys(jsonSetup.credentials[k]["userEmail"]);
                        numberPrefixInput.Clear(); 
                        numberPrefixInput.SendKeys(jsonSetup.credentials[k]["userNumberPrefix"]);
                        numberInput.Clear(); 
                        numberInput.SendKeys(jsonSetup.credentials[k]["userNumber"]);

                        if (jsonSetup.orderSetup[i].userChoiceNewsletter[0])
                        {
                            newsButton.Click();
                            if (jsonSetup.orderSetup[i].userChoiceNewsletter[1])
                            {
                                newsButtonWithProfile.Click();
                            }
                        }

                        firstNameInput.Clear();
                        firstNameInput.SendKeys(jsonSetup.credentials[k]["userName"]);
                        lastNameInput.Clear();
                        lastNameInput.SendKeys(jsonSetup.credentials[k]["userSurname"]);
                        addressInput.Clear();
                        addressInput.SendKeys(jsonSetup.credentials[k]["userAddress"]);
                        addressOtherInfoInput.Clear(); 
                        addressOtherInfoInput.SendKeys(jsonSetup.credentials[k]["userAdditionalAddressInfo"]);
                        townInput.Clear();
                        townInput.SendKeys(jsonSetup.credentials[k]["userTown"]);
                        zipInput.Clear(); 
                        zipInput.SendKeys(jsonSetup.credentials[k]["userZIP"]);
                        shippingState.Click();
                        TryAndClick($".//*[@data-option-array-index='{jsonSetup.credentials[k]["userState"]}']", 15);
                        countryButton.Click();
                        TryAndClick($"(.//*[@data-option-array-index='{jsonSetup.credentials[k]["userCountry"]}'])[2]", 15);
                        continueButtonOntoPayment.Click();

                        IWebElement nameOnCardInput = FindElement(By.XPath(".//*[@id='cardOwner']"), logger);
                        IWebElement cardNumberInput = FindElement(By.XPath(".//*[@id='cardNumber']"), logger);
                        IWebElement cardCVVInput = FindElement(By.XPath(".//*[@id='securityCode']"), logger);
                        IWebElement acceptingTermsButton = FindElement(By.XPath("(.//*[@class='checkbox-input'])[6]"), logger);

                        nameOnCardInput.SendKeys(jsonSetup.credentials[k]["creditCardName"]);
                        cardNumberInput.SendKeys(jsonSetup.credentials[k]["creditCardNumber"]);
                        TryAndClick(".//div[@id='expirationMonth_chosen']/a", 15);
                        TryAndClick($"(.//*[@data-option-array-index='{jsonSetup.credentials[k]["creditCardMonth"]}'])[2]", 15);
                        IWebElement yearChoice = FindElement(By.XPath(".//div[@id='expirationYear_chosen']/a"), logger);
                        yearChoice.Click();
                        IWebElement yearButton = FindElement(By.XPath($"(.//*[@data-option-array-index='{jsonSetup.credentials[k]["creditCardYear"]}'])[3]"), logger);
                        yearButton.Click();
                        cardCVVInput.SendKeys(jsonSetup.credentials[k]["creditCardCVV"]);
                        acceptingTermsButton.Click();
                        TryAndClick("(.//*[@name='submit'])[3]", 10);

                        IWebElement orderText = FindElement(By.XPath(".//*[@class='order-thank-you-msg h4 text-center side-margins']"), logger);
                        string str = orderText.Text;

                        IWebElement orderTextAmount = FindElement(By.XPath(".//*[@class='grand-total-sum dark']"), logger);
                        string orderPaymentAmountText = orderTextAmount.Text;

                        InsertOrder($"C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orders/creditcard/orders-creditcard-{currentTime.Hour}-{currentTime.Minute}-{currentTime.Second}--{currentTime.Day}-{currentTime.Month}-{currentTime.Year}.csv", "Falconeri", jsonSetup.orderSetup[i].locales[j], GetOrderNumber(str), jsonSetup.credentials[k]["userSurname"], "Standard Shipping", "Credit card", orderPaymentAmountText, skuAndAttributes, isLoggedIn);
                    }
                }

            }
        }

        [Test()]
        public void OrderWithPayPalStandardShipping()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            var csv = InitializeOrderInfoCSV("C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orderingCsv/orderInfo.csv", ";");
            var jsonSetup = InitializeIntimissimiSetup("IntimissimiSetupJson.json");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
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
                        if (i == 0 && j == 0 && k == 0)
                        {
                            IWebElement languageButton = FindElement(By.XPath(".//*[@id='setInputLocaleCountry']"), logger);
                            languageButton.Click();
                            logger.Debug("Language selected...");
                            TryAndClick(".//*[@id='cc-approve-button-thissite']", 15);
                            logger.Debug("Accepting cookies clicked...");
                            TryAndClick(".//*[@id='geoblock-close']", 15);
                            logger.Debug("GeoLocator clicked...");
                        }

                        for (int l = 0; l < jsonSetup.orderSetup[i].products.Count; l++)
                        {
                            driver.Navigate().GoToUrl($"https://test.intimissimi.com//{jsonSetup.orderSetup[i].locales[j]}/product/{jsonSetup.orderSetup[i].products[l]}.html");
                            #region FindElementInProducts
                            IWebElement addToBag = FindElement(By.XPath(".//*[@class='cell large-10 medium-10 small-5 add-to-cart button ae-button']"), logger);
                            #endregion

                            #region ClickElementInProducts
                            ImmediateButtonClickJS(js, addToBag);
                            #endregion

                            skuAndAttributes.Add($"{jsonSetup.orderSetup[i].products[l]}");
                        }

                        IWebElement shoppingBag = FindElement(By.XPath(".//*[@data-analytic-name='bag']"), logger);
                        shoppingBag.Click();

                        IWebElement continueButtonToShipping = FindElement(By.XPath("(.//*[@id='checkout-button-secure-checkout'])[2]"), logger);
                        ImmediateButtonClickJS(js, continueButtonToShipping);

                        Thread.Sleep(500);
                        TryAndClick(".//*[@value='submit-shipping']", 15);

                        IWebElement emailInput = FindElement(By.XPath(".//*[@id='shippingEmail']"), logger);
                        IWebElement numberPrefixInput = FindElement(By.XPath(".//*[@id='shippingPhoneNumberPrefix']"), logger);
                        IWebElement numberInput = FindElement(By.XPath(".//*[@id='shippingPhoneNumber']"), logger);
                        IWebElement newsButton = FindElement(By.XPath("(.//*[@class='slider round'])[1]"), logger);
                        IWebElement newsButtonWithProfile = FindElement(By.XPath("(.//*[@class='slider round'])[2]"), logger);
                        IWebElement firstNameInput = FindElement(By.XPath(".//*[@id='shippingFirstName']"), logger);
                        IWebElement lastNameInput = FindElement(By.XPath(".//*[@id='shippingLastName']"), logger);
                        IWebElement addressInput = FindElement(By.XPath(".//*[@id='shippingAddressOne']"), logger);
                        IWebElement addressOtherInfoInput = FindElement(By.XPath(".//*[@id='shippingAddressTwo']"), logger);
                        IWebElement townInput = FindElement(By.XPath(".//*[@id='shippingAddressCity']"), logger);
                        IWebElement zipInput = FindElement(By.XPath(".//*[@id='shippingZipCode']"), logger);
                        IWebElement shippingState = FindElement(By.XPath(".//div[@id='shippingState_chosen']/a[@class='chosen-single']"), logger);
                        IWebElement countryButton = FindElement(By.XPath(".//div[@id='shippingCountry_chosen']/a[@class='chosen-single']"), logger);
                        IWebElement continueButtonOntoPayment = FindElement(By.XPath(".//*[@value='submit-shipping']"), logger);

                        emailInput.Clear();
                        emailInput.SendKeys(jsonSetup.credentials[k]["userEmail"]);
                        numberPrefixInput.Clear();
                        numberPrefixInput.SendKeys(jsonSetup.credentials[k]["userNumberPrefix"]);
                        numberInput.Clear();
                        numberInput.SendKeys(jsonSetup.credentials[k]["userNumber"]);

                        if (jsonSetup.orderSetup[i].userChoiceNewsletter[0])
                        {
                            newsButton.Click();
                            if (jsonSetup.orderSetup[i].userChoiceNewsletter[1])
                            {
                                newsButtonWithProfile.Click();
                            }
                        }

                        firstNameInput.Clear();
                        firstNameInput.SendKeys(jsonSetup.credentials[k]["userName"]);
                        lastNameInput.Clear();
                        lastNameInput.SendKeys(jsonSetup.credentials[k]["userSurname"]);
                        addressInput.Clear();
                        addressInput.SendKeys(jsonSetup.credentials[k]["userAddress"]);
                        addressOtherInfoInput.Clear();
                        addressOtherInfoInput.SendKeys(jsonSetup.credentials[k]["userAdditionalAddressInfo"]);
                        townInput.Clear();
                        townInput.SendKeys(jsonSetup.credentials[k]["userTown"]);
                        zipInput.Clear();
                        zipInput.SendKeys(jsonSetup.credentials[k]["userZIP"]);
                        shippingState.Click();
                        TryAndClick($".//*[@data-option-array-index='{jsonSetup.credentials[k]["userState"]}']", 15);
                        countryButton.Click();
                        TryAndClick($"(.//*[@data-option-array-index='{jsonSetup.credentials[k]["userCountry"]}'])[2]", 15);
                        continueButtonOntoPayment.Click();

                        IWebElement payPalButton = FindElement(By.XPath(".//label[@for='PayPal']/span[@class='radio-input']"), logger);
                        IWebElement acceptingTermsPayPal = FindElement(By.XPath(".//label[@for='add-to-condition-sale-list_paypal']/span[@class='checkbox-input']"), logger);

                        payPalButton.Click();
                        acceptingTermsPayPal.Click();
                        IWebElement payWithPayPal = FindElement(By.XPath(".//*[@class='paypal-checkout-button js_paypal_button_on_billing_form']"), logger);
                        payWithPayPal.Click();

                        driver.SwitchTo().Window(driver.WindowHandles.Last());
                        driver.Manage().Window.Maximize();
                        Thread.Sleep(1000);

                        if (i == 0 && j == 0 && k == 0)
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

                        IWebElement orderText = FindElement(By.XPath(".//*[@class='order-thank-you-msg h4 text-center side-margins']"), logger);
                        string str = orderText.Text;

                        IWebElement orderTextAmount = FindElement(By.XPath(".//*[@class='grand-total-sum dark']"), logger);
                        string orderPaymentAmountText = orderTextAmount.Text;
                        InsertOrder($"C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/orders/paypal/orders-paypal-{currentTime.Hour}-{currentTime.Minute}-{currentTime.Second}--{currentTime.Day}-{currentTime.Month}-{currentTime.Year}.csv", "Falconeri", jsonSetup.orderSetup[i].locales[j], GetOrderNumber(str), jsonSetup.credentials[k]["userSurname"], "Standard Shipping", "PayPal", orderPaymentAmountText, skuAndAttributes, isLoggedIn);
                    }
                }

            }
        }

        [Test()]
        public void AcceptingRegistration()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            var csvUserInformation = InitializeUsersCSV($"C:/Users/GrabusicT/Documents/SeleniumTesting/SeleniumAutomation/SeleniumProj/bin/Debug/CsvFiles/UsersIntimissimi.csv", ";");
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



                IWebElement accountButton = driver.FindElement(By.XPath("(.//*[@align='center']//a)[3]"));



                accountButton.Click();
                driver.SwitchTo().Window(driver.WindowHandles.Last());
                js.ExecuteScript("window.close();");

                driver.SwitchTo().Window(driver.WindowHandles.Last());
                driver.Navigate().GoToUrl($"https://yopmail.com/en/");
                //Thread.Sleep(1000);
            }
        }

        [Test()]
        public void EditProfile()
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            var jsonSetup = InitializeIntimissimiSetup("IntimissimiSetupJson.json");
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            Actions action = new Actions(driver);

            for (int i = 0; i < jsonSetup.credentials.Count; i++)
            {
                if (i == 0)
                {
                    IWebElement languageButton = FindElement(By.XPath(".//*[@id='setInputLocaleCountry']"), logger);
                    languageButton.Click();
                    logger.Debug("Language selected...");
                    TryAndClick(".//*[@id='cc-approve-button-thissite']", 15);
                    logger.Debug("Accepting cookies clicked...");
                    TryAndClick(".//*[@id='geoblock-close']", 15);
                    logger.Debug("GeoLocator clicked...");
                }

                IWebElement emailField = FindElement(By.XPath("(.//*[@id='login-form-email'])[2]"), logger);
                IWebElement passwordField = FindElement(By.XPath("(.//*[@id='login-form-password'])[2]"), logger);
                IWebElement signInButton = FindElement(By.XPath(".//div[@class='grid-x']//button[@class='button wide extended ae-button' and @data-ae-blurbtype='button']"), logger);

                emailField.SendKeys(jsonSetup.credentials[i]["userEmail"]);
                passwordField.SendKeys(jsonSetup.credentials[i]["userPassword"]);
                ImmediateButtonClickJS(js, signInButton);
                TryAndClick(".//div[@class='grid-x']//button[@class='button wide extended ae-button' and @data-ae-blurbtype='button']", 15);
                signInButton.Click(); 
              
                IWebElement myProfile = FindElement(By.XPath(".//a[@href='/us/myprofile/']//div[@class='account-menu-item  ']"),logger);
                myProfile.Click(); 
            }




        }
    }
}
