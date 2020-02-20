using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VWOSdk.DemoApp.Models;

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
            = new List<string>() { "Ashley", "Bill", "Chris", "Dominic", "Emma", "Faizan",
                    "Gimmy", "Harry", "Ian", "John", "King", "Lisa", "Mona", "Nina",
                    "Olivia", "Pete", "Queen", "Robert", "Sarah", "Tierra", "Una",
                    "Varun", "Will", "Xin", "You", "Zeba" };

        private static Settings SettingsFile { get; set; }
        private static IVWOClient VWOClient { get; }

        static HomeController()
        {
            VWO.Configure(LogLevel.DEBUG);
            VWO.Configure(new CustomLogger());
            SettingsFile = SettingsProvider.GetSettingsFile(VWOConfig.ABCampaignSettings.AccountId, VWOConfig.ABCampaignSettings.SdkKey);
            VWOClient = VWO.CreateInstance(SettingsFile, isDevelopmentMode: false, userStorageService: new UserStorageService());
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
            var customVariables = VWOConfig.ABCampaignSettings.CustomVariables;
            var revenueVariables = VWOConfig.ABCampaignSettings.RevenueVariables;
            string activateResponse = null, getVariationResponse = null;
            bool trackResponse = false;
            if (VWOClient != null)
            {
                activateResponse = VWOClient.Activate(CampaignKey, userId);
                getVariationResponse = string.IsNullOrEmpty(activateResponse) ? activateResponse : VWOClient.GetVariation(CampaignKey, userId);
                trackResponse = string.IsNullOrEmpty(activateResponse) ? false : VWOClient.Track(CampaignKey, userId, goalIdentifier, revenueVariables);
            }
            var json = new ViewModel(SettingsFile, userId, CampaignKey, goalIdentifier, activateResponse, getVariationResponse, trackResponse, customVariables);
            return View(json);
        }

        
        [HttpGet]
        public IActionResult FeatureRollout([FromQuery] string user)
        {
            var userId = string.IsNullOrEmpty(user) ? GetRandomName() : user;
            var customVariables = VWOConfig.FeatureRolloutData.CustomVariables;
            string CampaignKey = VWOConfig.FeatureRolloutData.CampaignKey;
            string campaignType = "Feature-rollout";
            bool activateResponse = false;
            if (VWOClient != null)
            {
                activateResponse = VWOClient.IsFeatureEnabled(CampaignKey, userId, customVariables);
            }
            var json = new ViewModel(SettingsFile, userId, CampaignKey, campaignType, activateResponse, customVariables);
            return View(json);
        }

        [HttpGet]
        public IActionResult FeatureTest([FromQuery] string user)
        {
            var userId = string.IsNullOrEmpty(user) ? GetRandomName() : user;
            var revenueAndCustomVariables = VWOConfig.FeatureTestData.RevenueAndCustomVariables;
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
                
                activateResponse = VWOClient.IsFeatureEnabled(CampaignKey, userId, revenueAndCustomVariables);
                if (activateResponse) {
                  VWOClient.Track(CampaignKey, userId, goalIdentifier, revenueAndCustomVariables);
                }
                stringVariable = VWOClient.GetFeatureVariableValue(CampaignKey, stringVariableKey, userId, revenueAndCustomVariables);
                integerVariable = VWOClient.GetFeatureVariableValue(CampaignKey, integerVariableKey, userId, revenueAndCustomVariables);
                booleanVariable = VWOClient.GetFeatureVariableValue(CampaignKey, booleanVariableKey, userId, revenueAndCustomVariables);
                doubleVariable = VWOClient.GetFeatureVariableValue(CampaignKey, doubleVariableKey, userId, revenueAndCustomVariables);
            }
            var json = new ViewModel(SettingsFile, userId, CampaignKey, goalIdentifier, campaignType, activateResponse, revenueAndCustomVariables, stringVariable, integerVariable, booleanVariable, doubleVariable);
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
    }
}
