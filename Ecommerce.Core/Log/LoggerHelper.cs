using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Serilog.Context;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
        
        
        var jsonOptions = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        var serializedArgs = args != null ? JsonSerializer.Serialize(args, jsonOptions) : "null";
        var serializedRetrievedData = retrievedData != null ? JsonSerializer.Serialize(retrievedData, jsonOptions) : "null";

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