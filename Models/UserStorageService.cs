using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VWOSdk.DemoApp
{
    public class UserStorageService : IUserStorageService
    {
        private static string path = @"userStorageMaps.json";
        private ConcurrentDictionary<string, ConcurrentDictionary<string, string>> _userStorageMap = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();

        public UserStorageService()
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
                    _userStorageMap = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>(data);
                }
                else
                    _userStorageMap = new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
            }
            catch { }
        }

        public UserStorageMap Get(string userId, string CampaignKey)
        {
            string variationName = null;
            if (_userStorageMap.TryGetValue(CampaignKey, out ConcurrentDictionary<string, string> userMap))
                userMap.TryGetValue(userId, out variationName);

            if (string.IsNullOrEmpty(variationName) == false)
                return new UserStorageMap(userId, CampaignKey, variationName);

            return null;
        }

        public void Set(UserStorageMap userStorageMap)
        {
            if (_userStorageMap.TryGetValue(userStorageMap.CampaignKey, out ConcurrentDictionary<string, string> userMap) == false)
            {
                userMap = new ConcurrentDictionary<string, string>();
                _userStorageMap[userStorageMap.CampaignKey] = userMap;
            }
            userMap[userStorageMap.UserId] = userStorageMap.VariationName;
            SaveAsync();
        }

        public async Task SaveAsync()
        {
            System.IO.File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(_userStorageMap, Newtonsoft.Json.Formatting.Indented));
        }
    }
}