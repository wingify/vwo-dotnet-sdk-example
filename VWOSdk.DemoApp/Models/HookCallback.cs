using Newtonsoft.Json;
using System.Collections.Generic;
namespace VWOSdk.DemoApp
{
    internal class HookCallback : IntegrationEventListener
    {     
        public void onEvent(Dictionary<string, dynamic> properties)
        {
            string payLoad = JsonConvert.SerializeObject(properties);
            CustomLogger logger = new CustomLogger();
            logger.WriteLog(LogLevel.DEBUG, "onEvent call from SDK: " + payLoad);

        }
    }
}
