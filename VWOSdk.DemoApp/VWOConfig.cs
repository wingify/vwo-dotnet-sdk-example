using System.Collections.Generic;

namespace VWOSdk.DemoApp
{
    public class VWOConfig
    {
        internal static class SDK {
            public static long AccountId = Defaults.AccountId;         ////Assign actual value;
            public static string SdkKey = Defaults.SdkKey;
        }
        internal static class ABCampaignSettings {       ////Assign actual value;
            public static string CampaignKey = Defaults.CampaignKey;          ////Assign actual value;
            public static string GoalIdentifier = Defaults.GoalIdentifier;          ////Assign actual value;
            public static Dictionary<string, dynamic> Options = Defaults.Options;      ////Assign actual value;
        }

        internal static class PushData {
            public static string TagKey = "";
            public static dynamic TagValue = "";
        }

        internal static class FeatureRolloutData {
            public static string CampaignKey = "";
            public static Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>();
        }

        internal static class FeatureTestData {
            public static string CampaignKey = "";
            public static string GoalIdentifier = "";
            public static Dictionary<string, dynamic> Options = new Dictionary<string, dynamic>();
            public static string StringVariableKey = "";
            public static string IntegerVariableKey = "";
            public static string DoubleVariableKey = "";
            public static string BooleanVariableKey = "";
        }
    }
}
