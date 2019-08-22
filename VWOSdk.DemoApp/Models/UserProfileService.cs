using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VWOSdk.DemoApp
{
    public class UserProfileService : IUserProfileService
    {
        private static string path = @"userProfileMaps.json";
        private static object _obj = new object();
        private Dictionary<string, UserProfileMap> _userProfileMap = new Dictionary<string, UserProfileMap>();

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
                string json = null;
                lock (_obj)
                {
                    json = System.IO.File.ReadAllText(path);
                }
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, UserProfileMap>>(json);
                if (data != null)
                {
                    _userProfileMap = new Dictionary<string, UserProfileMap>(data);
                }
                else
                    _userProfileMap = new Dictionary<string, UserProfileMap>();
            }
            catch { }
        }

        public UserProfileMap Lookup(string userId)
        {
            return _userProfileMap[userId];
        }

        public void Save(UserProfileMap userProfileMap)
        {
            _userProfileMap[userProfileMap.UserId] = userProfileMap;
            SaveAsync();
        }

        private async Task SaveAsync()
        {
            lock (_obj)
            {
                System.IO.File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(_userProfileMap, Newtonsoft.Json.Formatting.Indented));
            }
        }
    }
}