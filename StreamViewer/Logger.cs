using System;

using Microsoft.Extensions.Logging;

namespace StreamViewer;

public static partial class Logger
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "=================================== Startup ====================================")]
    public static partial void LogStartup(this ILogger logger);

    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Waveform control size changed: {width}x{height}.")]
    public static partial void SizeChanged(this ILogger logger, double width, double height);

    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "Failed to create StreamInlet!")]
    public static partial void FailedToCreateStreamInlet(this ILogger logger, Exception ex);

    [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "=================================== Shutdown ===================================")]
    public static partial void LogShutdown(this ILogger logger);
}
