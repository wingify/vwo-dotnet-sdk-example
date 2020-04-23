using System.Collections.Generic;

namespace VWOSdk.DemoApp
{
    internal class Defaults
    {
        public readonly static long AccountId = 1;
        public readonly static string SdkKey = "";
        public readonly static string CampaignKey = "";          ////Assign actual value;
        public readonly static string GoalIdentifier = "";          ////Assign actual value;
        public readonly static Dictionary<string, dynamic> RevenueVariables = new Dictionary<string, dynamic>
        {
            {"revenue_value", 10}
        };
        public readonly static Dictionary<string, dynamic> CustomVariables = new Dictionary<string, dynamic>{};
        public readonly static Dictionary<string, dynamic> VariationTargettingVariable = new Dictionary<string, dynamic>{};
    }
}
