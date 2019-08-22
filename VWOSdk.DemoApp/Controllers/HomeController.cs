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
            SettingsFile = SettingsProvider.GetSettings(VWOConfig.AccountId, VWOConfig.SdkKey);
            VWOClient = VWO.Instantiate(SettingsFile, isDevelopmentMode: true, userProfileService: new UserProfileService());
        }

        [HttpGet]
        public IActionResult Index([FromQuery] string user)
        {
            var campaignTestKey = VWOConfig.CampaignTestKey;
            var goalIdentifier = VWOConfig.GoalIdentifier;
            var userId = string.IsNullOrEmpty(user) ? GetRandomName() : user;
            string activateResponse = null, getVariationResponse = null;
            bool trackResponse = false;
            if (VWOClient != null)
            {
                activateResponse = VWOClient.Activate(campaignTestKey, userId);
                getVariationResponse = string.IsNullOrEmpty(activateResponse) ? activateResponse : VWOClient.GetVariation(campaignTestKey, userId);
                trackResponse = string.IsNullOrEmpty(activateResponse) ? false : VWOClient.Track(campaignTestKey, userId, goalIdentifier);
            }
            var json = new IndexViewModel(SettingsFile, userId, campaignTestKey, goalIdentifier, activateResponse, getVariationResponse, trackResponse);
            return View(json);  
        }
    }
}
