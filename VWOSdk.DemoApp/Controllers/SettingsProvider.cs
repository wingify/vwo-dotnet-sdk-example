using System.IO;
using System.Reflection;
using System.Threading.Tasks;


namespace VWOSdk.DemoApp.Controllers
{
    public class SettingsProvider
    {

       
        private static string _SettingsFilePath = Defaults.SettingsFilePath;     
        public static Settings GetSettingsFile(long accountId, string sdkKey)
        {

            if (System.IO.File.Exists(_SettingsFilePath) == false)
            {
                System.IO.File.Create(_SettingsFilePath).Close();
                Settings SettingsFile = VWO.GetSettingsFile(accountId, sdkKey);
                _ = SaveAsync(SettingsFile);

                return SettingsFile;
            }
            else
            {
                return GetSettingsFile();
            }
        }

        public static async Task<Settings> GetAndUpdateSettingsFile(long accountId, string sdkKey)
        {
           if (File.Exists(_SettingsFilePath) == false)
            {
                File.Create(_SettingsFilePath).Close();
            }
            Settings SettingsFile = VWO.GetAndUpdateSettingsFile(accountId, sdkKey);
            if (SettingsFile != null)
            {
                await File.WriteAllTextAsync(_SettingsFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(SettingsFile, Newtonsoft.Json.Formatting.Indented));
            }
            return GetSettingsFile();
        }
        public static async Task SaveAsync(Settings SettingsFile)
        {
            await File.WriteAllTextAsync(_SettingsFilePath, Newtonsoft.Json.JsonConvert.SerializeObject(SettingsFile, Newtonsoft.Json.Formatting.Indented));
        }
        private static Settings GetSettingsFile()
        {
            string json = File.ReadAllText(_SettingsFilePath);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(json);
        }
    }    
}
