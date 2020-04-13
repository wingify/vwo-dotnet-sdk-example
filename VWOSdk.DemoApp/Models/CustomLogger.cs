using System;

namespace VWOSdk.DemoApp
{
    public class CustomLogger : ILogWriter
    {
        public void WriteLog(LogLevel logLevel, string message)
        {
            message = message ?? string.Empty;
            message = $"[CustomLog] - [WVO-SDK] - [{logLevel}] {DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ss.fffZ")} - {message}";

            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
