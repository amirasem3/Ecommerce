using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Serilog.Context;

namespace Ecommerce.Core.Log;

public class LoggerHelper
{
    public enum LogLevel
    {
        Information,
        Warning,
        Error
    }
    

    public static void LogWithDetails(string message = "", object retrievedData = null, object[] args = null,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        LogLevel logLevel = LogLevel.Information)
    {
        
        
        var jsonSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        var serializedArgs = args != null ? JsonConvert.SerializeObject(args) : "null";
        var serializedRetrievedData = retrievedData != null ? JsonConvert.SerializeObject(retrievedData) : "null";

        var className = System.IO.Path.GetFileNameWithoutExtension(sourceFilePath);

        using (LogContext.PushProperty("ClassName", className))
        using (LogContext.PushProperty("FunctionName", memberName))
        using (LogContext.PushProperty("Arguments", serializedArgs))
        using (LogContext.PushProperty("RetrieveData", serializedRetrievedData))
        {

            switch (logLevel)
            {
                case LogLevel.Information:
                    Serilog.Log.Information(message);
                    break;
                case LogLevel.Error:
                    Serilog.Log.Error(message);
                    break;
                case LogLevel.Warning:
                    Serilog.Log.Warning(message);
                    break;
            }
             
        }
    }
}