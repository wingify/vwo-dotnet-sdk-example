using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VWOSdk.DemoApp.Models;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace VWOSdk.DemoApp.Controllers
{

    public class HomeController : Controller
    {
        private static string GetRandomName()
        {
            int randomNumber = RandomGenerator.Next(0, Names.Count - 1);
            return Names[randomNumber];
        }

        private static Random RandomGenerator { get; set; } = new Random();

        private static List<string> Names
            = new List<string>() {
               "Ashley", "Bill", "Chris", "Dominic", "Emma", "Faizan",
                "Gimmy", "Harry", "Ian", "John", "King", "Lisa", "Mona", "Nina",
                "Olivia", "Pete", "Queen", "Robert", "Sarah", "Tierra", "Una",
                "Will", "Xin", "You", "Zeba",
               };

        private static Settings SettingsFile { get; set; }
        private static IVWOClient VWOClient { get; set; }
        static HomeController()
        {
            VWO.Configure(LogLevel.DEBUG);
            VWO.Configure(new CustomLogger());
            CustomLogger logger = new CustomLogger();
            SettingsFile = SettingsProvider.GetSettingsFile(VWOConfig.SDK.AccountId, VWOConfig.SDK.SdkKey);
            BatchEventData _batchData = new BatchEventData();
            _batchData.EventsPerRequest = Defaults.EventsPerRequest;
            _batchData.RequestTimeInterval = Defaults.RequestTimeInterval;
            _batchData.FlushCallback = new FlushCallback();
            //logger.WriteLog(LogLevel.DEBUG, "BatchEventData : EventsPerRequest-" + Defaults.EventsPerRequest.ToString() +", RequestTimeInterval:" + Defaults.RequestTimeInterval);          
            //VWOClient = VWO.Launch(SettingsFile, batchData: _batchData);

            //logger.WriteLog(LogLevel.DEBUG, "HookManager : IntegrationEventListener onEvent requested ");
            //VWOClient = VWO.Launch(SettingsFile, batchData: _batchData, integrations: new HookManager(){HookCallback = new HookCallback()});

            logger.WriteLog(LogLevel.DEBUG, "BatchEventData,userStorageService,isDevelopmentMode,integrations,shouldTrackReturningUser passed in SDK");
            VWOClient = VWO.Launch(SettingsFile, batchData: _batchData, userStorageService: new UserStorageService(),
                isDevelopmentMode: false, integrations: new HookManager() { HookCallback = new HookCallback() }, shouldTrackReturningUser: false);
        }

        [HttpGet]
        public IActionResult Index([FromQuery] string user)
        {
            return View();
        }
        [HttpGet]
        public IActionResult Ab([FromQuery] string user)
        {
            var userId = string.IsNullOrEmpty(user) ? GetRandomName() : user;
            var CampaignKey = VWOConfig.ABCampaignSettings.CampaignKey;
            var goalIdentifier = VWOConfig.ABCampaignSettings.GoalIdentifier;
            var options = VWOConfig.ABCampaignSettings.Options;
            string activateResponse = null, getVariationResponse = null;
            bool trackResponse = false;

            // Get the user agent and IP address from the HTTP request
            var httpRequest = HttpContext.Request;
            string userAgent = httpRequest.Headers["User-Agent"]; // This is the user agent string of the client's browser.
            string userIpAddress = httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString(); // This is the IP address of the client's computer.
            
            // Add the user agent and IP address to the Options dictionary
            options["userAgent"] = !string.IsNullOrEmpty(userAgent) ? userAgent : ""; // This key-value pair will be used to track the user's browser.
            options["userIpAddress"] = !string.IsNullOrEmpty(userIpAddress) ? userIpAddress : ""; // This key-value pair can be used to track the user's IP address.

            if (VWOClient != null)
            {

                activateResponse = VWOClient.Activate(CampaignKey, userId, options);
                getVariationResponse = string.IsNullOrEmpty(activateResponse) ? activateResponse : VWOClient.GetVariationName(CampaignKey, userId, options);
                // Track all campaigns -
                // VWOClient.Track(userId, goalIdentifier, options);

                // Track multiple campaigns -
                // VWOClient.Track(new List<String>(){ CampaignKey, "Test2" }, userId, goalIdentifier, options);

                trackResponse = string.IsNullOrEmpty(activateResponse) ? false : VWOClient.Track(CampaignKey, userId, goalIdentifier, options);
            }
            var json = new ViewModel(SettingsFile, userId, CampaignKey, goalIdentifier, activateResponse, getVariationResponse, trackResponse, options);
            return View(json);

        }


        [HttpGet]
        public IActionResult FeatureRollout([FromQuery] string user)
        {
            var userId = string.IsNullOrEmpty(user) ? GetRandomName() : user;
            var options = VWOConfig.FeatureRolloutData.Options;
            string CampaignKey = VWOConfig.FeatureRolloutData.CampaignKey;
            string campaignType = "Feature-rollout";
            bool activateResponse = false;

            // Get the user agent and IP address from the HTTP request
            var httpRequest = HttpContext.Request;
            string userAgent = httpRequest.Headers["User-Agent"]; // This is the user agent string of the client's browser.
            string userIpAddress = httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString(); // This is the IP address of the client's computer.
            
            // Add the user agent and IP address to the Options dictionary
            options["visitorUserAgent"] = !string.IsNullOrEmpty(userAgent) ? userAgent : ""; // This key-value pair will be used to track the user's browser.
            options["visitorIp"] = !string.IsNullOrEmpty(userIpAddress) ? userIpAddress : ""; // This key-value pair can be used to track the user's IP address.

            if (VWOClient != null)
            {
                activateResponse = VWOClient.IsFeatureEnabled(CampaignKey, userId, options);
            }
            var json = new ViewModel(SettingsFile, userId, CampaignKey, campaignType, activateResponse, options);
            return View(json);
        }

        [HttpGet]
        public IActionResult FeatureTest([FromQuery] string user)
        {
            var userId = string.IsNullOrEmpty(user) ? GetRandomName() : user;
            var options = VWOConfig.FeatureTestData.Options;
            
            // Get the user agent and IP address from the HTTP request
            var httpRequest = HttpContext.Request;
            string userAgent = httpRequest.Headers["User-Agent"]; // This is the user agent string of the client's browser.
            string userIpAddress = httpRequest.HttpContext.Connection.RemoteIpAddress?.ToString(); // This is the IP address of the client's computer.
            
            // Add the user agent and IP address to the Options dictionary
            options["visitorUserAgent"] = !string.IsNullOrEmpty(userAgent) ? userAgent : ""; // This key-value pair will be used to track the user's browser.
            options["visitorIp"] = !string.IsNullOrEmpty(userIpAddress) ? userIpAddress : ""; // This key-value pair can be used to track the user's IP address.

            string stringVariableKey = VWOConfig.FeatureTestData.StringVariableKey;
            string integerVariableKey = VWOConfig.FeatureTestData.IntegerVariableKey;
            string booleanVariableKey = VWOConfig.FeatureTestData.BooleanVariableKey;
            string doubleVariableKey = VWOConfig.FeatureTestData.DoubleVariableKey;
            string goalIdentifier = VWOConfig.FeatureTestData.GoalIdentifier;
            string CampaignKey = VWOConfig.FeatureTestData.CampaignKey;
            string campaignType = "Feature-test";
            bool activateResponse = false;
            dynamic integerVariable = null;
            dynamic booleanVariable = null;
            dynamic stringVariable = null;
            dynamic doubleVariable = null;
            if (VWOClient != null)
            {

                activateResponse = VWOClient.IsFeatureEnabled(CampaignKey, userId, options);
                if (activateResponse)
                {
                    VWOClient.Track(CampaignKey, userId, goalIdentifier, options);
                }
                stringVariable = VWOClient.GetFeatureVariableValue(CampaignKey, stringVariableKey, userId, options);
                integerVariable = VWOClient.GetFeatureVariableValue(CampaignKey, integerVariableKey, userId, options);
                booleanVariable = VWOClient.GetFeatureVariableValue(CampaignKey, booleanVariableKey, userId, options);
                doubleVariable = VWOClient.GetFeatureVariableValue(CampaignKey, doubleVariableKey, userId, options);
            }
            var json = new ViewModel(SettingsFile, userId, CampaignKey, goalIdentifier, campaignType, activateResponse, options, stringVariable, integerVariable, booleanVariable, doubleVariable);
            return View(json);
        }

        [HttpGet]
        public IActionResult Push([FromQuery] string user)
        {
            var userId = string.IsNullOrEmpty(user) ? GetRandomName() : user;
            string tagKey = VWOConfig.PushData.TagKey;
            dynamic tagValue = VWOConfig.PushData.TagValue;
            bool activateResponse = false;
            if (VWOClient != null)
            {
                activateResponse = VWOClient.Push(tagKey, tagValue, userId);
            }
            var json = new ViewModel(SettingsFile, userId, activateResponse, tagKey, tagValue);
            return View(json);
        }
        [HttpGet]
        public IActionResult Flush()
        {

            if (VWOClient != null)
            {
                CustomLogger logger = new CustomLogger();
                logger.WriteLog(LogLevel.DEBUG, "manual flushEvent called ");
                bool response = VWOClient.FlushEvents();
                logger.WriteLog(LogLevel.DEBUG, "flushEvent response: " + response.ToString());
            }
            return View("Index");
        }
        [Route("/webhook")]
        [HttpPost]
        public async Task<string> webhook()
        {
            CustomLogger logger = new CustomLogger();
            string PayLoad;
            logger.WriteLog(LogLevel.DEBUG, "Post request from vwo app");

            using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
            {
                PayLoad = await reader.ReadToEndAsync();
            }
            logger.WriteLog(LogLevel.DEBUG, "VWO webhook payload: " + PayLoad);
            if (string.IsNullOrEmpty(Defaults.WebhookSecretKey) == false)
            {
                logger.WriteLog(LogLevel.DEBUG, "WebhookSecretKey exists . VWO webhook authentication Checking.");

                if (Request.Headers["x-vwo-auth"].ToString() != Defaults.WebhookSecretKey)
                {
                    logger.WriteLog(LogLevel.DEBUG, "VWO webhook authentication failed. Please check.");
                    return "VWO webhook authentication failed. Please check.";
                }

                if (VWOClient != null)
                {
                    logger.WriteLog(LogLevel.DEBUG, "Authentication passed and GetAndUpdateSettingsFile function is called");
                    await SettingsProvider.GetAndUpdateSettingsFile(VWOConfig.SDK.AccountId, VWOConfig.SDK.SdkKey);
                    logger.WriteLog(LogLevel.DEBUG, "Setting file has been updated");
                }


            }
            else
            {


                if (VWOClient != null)
                {
                    logger.WriteLog(LogLevel.DEBUG, "GetAndUpdateSettingsFile function called");
                    await SettingsProvider.GetAndUpdateSettingsFile(VWOConfig.SDK.AccountId, VWOConfig.SDK.SdkKey);
                    logger.WriteLog(LogLevel.DEBUG, "Setting file has been updated");

                }


            }


            return "";
        }

    }


}
