using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VWOSdk.DemoApp
{
    public class UserProfileService : IUserProfileService
    {
        private static string path = @"userProfileMaps.json";
        private ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _userProfileMap = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();

        public UserProfileService()
        {
            Load();
        }

        private void Load()
        {
            if (System.IO.File.Exists(path) == false)
                System.IO.File.Create(path);

            try
            {
                string json = System.IO.File.ReadAllText(path);
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ConcurrentDictionary<string, ConcurrentDictionary<string, string>>>(json);
                if (data != null)
                {
                    _userProfileMap = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>(data);
                }
                else
                    _userProfileMap = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
            }
            catch { }
        }

        public UserProfileMap Lookup(string userId, string campaignTestKey)
        {
            string variationName = null;
            if (_userProfileMap.TryGetValue(campaignTestKey, out ConcurrentDictionary<string, string> userMap))
                userMap.TryGetValue(userId, out variationName);

            if (string.IsNullOrEmpty(variationName) == false)
                return new UserProfileMap(userId, campaignTestKey, variationName);

            return null;
        }

        public void Save(UserProfileMap userProfileMap)
        {
            if (_userProfileMap.TryGetValue(userProfileMap.CampaignTestKey, out ConcurrentDictionary<string, string> userMap) == false)
            {
                userMap = new ConcurrentDictionary<string, string>();
                _userProfileMap[userProfileMap.CampaignTestKey] = userMap;
            }
            userMap[userProfileMap.UserId] = userProfileMap.VariationName;
            SaveAsync();
        }

        public async Task SaveAsync()
        {
            System.IO.File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(_userProfileMap, Newtonsoft.Json.Formatting.Indented));
        }
    }
}