using System.Security.Cryptography;
using System.Text;

namespace VWOSdk.DemoApp.Models
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {

        }

        public IndexViewModel(Settings settingsFile, string userId, string campaignTestKey, string goalIdenfier, string activateResponse, string getVariationResponse, bool trackResponse)
        {
            this.UserId = userId;
            this.CurrentSettingsFile = settingsFile;
            this.CampaignTestKey = campaignTestKey;
            this.GoalIdentifier = goalIdenfier;
            this.Activate = activateResponse;
            this.GetVariation = getVariationResponse;
            this.Track = trackResponse;
            this.Changes = GetSha256Hash(SHA256.Create(), this.CampaignTestKey + this.GetVariation);
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

        public string CampaignTestKey { get; set; }

        public string GoalIdentifier { get; set; }
        
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
    }
}