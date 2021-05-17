using Newtonsoft.Json;
using System.Collections.Generic;

namespace VWOSdk.DemoApp
{
    internal class FlushCallback : IFlushInterface
    {
        public void onFlush(string error, object events)
        {
            CustomLogger logger = new CustomLogger();
            logger.WriteLog(LogLevel.DEBUG, "OnFlush call from SDK: " + events.ToString());
                      
        }
       
    }
}
