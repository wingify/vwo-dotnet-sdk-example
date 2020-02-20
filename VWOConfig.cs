using System.Collections.Generic;

namespace VWOSdk.DemoApp
{
    public class VWOConfig
    {
        internal static class ABCampaignSettings {
            public static long AccountId = Defaults.AccountId;          ////Assign actual value;
            public static string SdkKey = Defaults.SdkKey;          ////Assign actual value;
            public static string CampaignKey = Defaults.CampaignKey;          ////Assign actual value;
            public static string GoalIdentifier = Defaults.GoalIdentifier;          ////Assign actual value;
            public static Dictionary<string, dynamic> CustomVariables = Defaults.CustomVariables;
            public static Dictionary<string, dynamic> RevenueVariables = Defaults.RevenueVariables;
        }

        internal static class PushData {
            public static string TagKey = "";
            public static dynamic TagValue = "";
        }

        internal static class FeatureRolloutData {
            public static string CampaignKey = "";
            public static Dictionary<string, dynamic> CustomVariables = new Dictionary<string, dynamic>();
        }

        internal static class FeatureTestData {
            public static string CampaignKey = "";
            public static string GoalIdentifier = "";
            public static Dictionary<string, dynamic> RevenueAndCustomVariables = new Dictionary<string, dynamic>();
            public static string StringVariableKey = "";
            public static string IntegerVariableKey = "";
            public static string DoubleVariableKey = "";
            public static string BooleanVariableKey = "";
        }
    }
}
