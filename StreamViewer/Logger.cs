using Microsoft.Extensions.Logging;

namespace StreamViewer;

public static partial class Logger
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "=================================== Startup ====================================")]
    public static partial void LogStartup(this ILogger logger);

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "=================================== Shutdown ===================================")]
    public static partial void LogShutdown(this ILogger logger);
}
