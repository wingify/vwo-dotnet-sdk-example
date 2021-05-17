using System.Collections.Generic;

namespace VWOSdk.DemoApp
{
    internal class Defaults
    {
        public readonly static long AccountId = 0;
        public readonly static string SdkKey = "your-sdk-key";
        public readonly static string CampaignKey = "your-campaign-key";          ////Assign actual value;
        public readonly static string GoalIdentifier = "your-goal-identifier";          ////Assign actual value
        //SettingsFile Path
        public readonly static string SettingsFilePath = @"Resources/DemoSettingsFile.json";
        //Webhook
        public readonly static string WebhookSecretKey = "your-Webhook-Secret-Key";
        //Batch Event
        public readonly static int? EventsPerRequest = 6; //default 100
        public readonly static int? RequestTimeInterval = 60;  //default:- 10 * 60(secs) = 600 secs i.e. 10 minutes
        public readonly static Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>()
        {
            {
                "revenueValue", 10
            },
            {
                "shouldTrackReturningUser", false
            },
            {
                "goalTypeToTrack", "ALL"
            },
            {
              "customVariables", new Dictionary<string, dynamic>()
              {
                  {
                    "gender", "m"
                  }
              }
            },
            {
              "variationTargettingVariable", new Dictionary<string, dynamic>()
              {
                  {
                      "random", 1
                  }
              }
            }
        };
    }
}
