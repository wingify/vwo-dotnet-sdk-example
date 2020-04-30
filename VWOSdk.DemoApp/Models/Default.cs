using System.Collections.Generic;

namespace VWOSdk.DemoApp
{
    internal class Defaults
    {
        public readonly static long AccountId = 1;
        public readonly static string SdkKey = "";
        public readonly static string CampaignKey = "";          ////Assign actual value;
        public readonly static string GoalIdentifier = "custom";          ////Assign actual value;
        public readonly static Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>()
        {
            {
                "revenue_value", 10
            },
            {
              "customVariables", new Dictionary<string, dynamic>()
              {
                  {
                    "gender", "f"
                  }
              }
            },
            {
              "variationTargettingVariable", new Dictionary<string, dynamic>()
              {
                  {
                      "abcd", 1
                  }
              }
            }
        };
    }
}
