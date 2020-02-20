using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace VWOSdk.DemoApp.Models
{
    public class ViewModel
    {
        public ViewModel()
        {

        }

        public ViewModel(Settings settingsFile, string userId, string CampaignKey, string goalIdenfier, string activateResponse, string getVariationResponse, bool trackResponse, Dictionary<string, dynamic> customVariables)
        {
            this.UserId = userId;
            this.CurrentSettingsFile = settingsFile;
            this.CampaignKey = CampaignKey;
            this.GoalIdentifier = goalIdenfier;
            this.Activate = activateResponse;
            this.GetVariation = getVariationResponse;
            this.CustomVariables = customVariables;
            this.Track = trackResponse;
            this.Changes = GetSha256Hash(SHA256.Create(), this.CampaignKey + this.GetVariation);
        }

        // For Push Api
        public ViewModel(Settings settingsFile, string userId, bool activateResponse, string tagKey, dynamic tagValue)
        {
            this.UserId = userId;
            this.CurrentSettingsFile = settingsFile;
            this.PartOfCampaign = activateResponse;
            this.TagKey = tagKey;
            this.TagValue = tagValue;
        }

        // For Feature Rollout
        public ViewModel(Settings settingsFile, string userId, string CampaignKey, string campaignType, bool activateResponse, Dictionary<string, dynamic> customVariables)
        {
            this.UserId = userId;
            this.CurrentSettingsFile = settingsFile;
            this.PartOfCampaign = activateResponse;
            this.CampaignKey = CampaignKey;
            this.CampaignType = campaignType;
            this.CustomVariables = customVariables;
        }

        // For Feature Test
        public ViewModel(Settings settingsFile, string userId, string CampaignKey, string goalIdenfier, string campaignType, bool activateResponse, Dictionary<string, dynamic> customVariables, dynamic stringVariable, dynamic integerVariable, dynamic booleanVariable, dynamic doubleVariable)
        {
            this.UserId = userId;
            this.CurrentSettingsFile = settingsFile;
            this.PartOfCampaign = activateResponse;
            this.CampaignKey = CampaignKey;
            this.CampaignType = campaignType;
            this.CustomVariables = customVariables;
            this.StringVariable = stringVariable;
            this.IntegerVariable = integerVariable;
            this.BooleanVariable = booleanVariable;
            this.DoubleVariable = doubleVariable;
        }
        
        public Settings CurrentSettingsFile { get; set; }
        
        public string Settings { get
            {
                if(CurrentSettingsFile != null)
                    return "\n" + Newtonsoft.Json.JsonConvert.SerializeObject(CurrentSettingsFile, Newtonsoft.Json.Formatting.Indented);
                return "\n Settings File not Valid!!!";
            }
        }

        public string Changes { get; set; } = "57e0a2";

        public string CampaignKey { get; set; }

        public string CampaignType { get; set; }

        public string GoalIdentifier { get; set; }

        public dynamic StringVariable { get; set; }

        public dynamic IntegerVariable { get; set; }

        public dynamic BooleanVariable { get; set; }

        public dynamic DoubleVariable { get; set; }

        public string TagKey { get; set; }

        public dynamic TagValue { get; set; }

        public bool PartOfCampaign { get; set; }
        
        public string GetVariation {
            get; set;
        }

        public string Activate
        {
            get; set;
        }

        public bool Track
        {
            get; set;
        }
        
        public string UserId { get; set; }

        public Dictionary<string, dynamic> CustomVariables { get; set; }
        
        static string GetSha256Hash(SHA256 shaHash, string input)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = shaHash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString().Substring(0, 6);
        }

        public string StringifyCustomVariables()
        {
            if (this.CustomVariables.Count == 0) {
                return "";
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("{\n  ");
            foreach (var pair in this.CustomVariables)
            {
                builder.Append("\"" + pair.Key + "\"").Append(": ").Append("\"" + pair.Value + "\"").Append(",\n  ");
            }
            builder.Length -= 4;
            builder.Append("\n}");
            string result = builder.ToString();
            return result;
        }
    }
}