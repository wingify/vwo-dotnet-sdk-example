using System.IO;
using System.Reflection;

namespace VWOSdk.DemoApp.Controllers
{
    public class SettingsProvider
    {
        public static Settings GetSettings(long accountId, string sdkKey)
        {
            if (accountId == DemoApp.Defaults.AccountId)
                return GetSettingsFile("DemoSettingsFile");
            return VWO.GetSettings(accountId, sdkKey);
        }

        private static Settings GetSettingsFile(string filename)
        {
            string json = GetJsonText(filename);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<Settings>(json);
        }

        private static string GetJsonText(string filename)
        {
            string path = null;

            foreach (var resource in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (resource.Contains("." + filename + "."))
                {
                    path = resource;
                    break;
                }
            }

            try
            {
                var _assembly = Assembly.GetExecutingAssembly();
                using (Stream resourceStream = _assembly.GetManifestResourceStream(path))
                {
                    if (resourceStream == null)
                        return null;

                    using (StreamReader reader = new StreamReader(resourceStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch { }

            return null;
        }
    }
}
