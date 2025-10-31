// Copyright 2013-2015 Serilog Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Threading;

namespace Kavita.Logger;


/// <summary>
/// An optional static entry point for logging that can be easily referenced
/// by different parts of an application. To configure the <see cref="Log"/>
/// set the Logger static property to a logger instance.
/// </summary>
/// <example>
/// Log.Logger = new LoggerConfiguration()
///     .WithConsoleSink()
///     .CreateLogger();
///
/// var thing = "World";
/// Log.Logger.Information("Hello, {Thing}!", thing);
/// </example>
/// <remarks>
/// The methods on <see cref="Log"/> (and its dynamic sibling <see cref="ILogger"/>) are guaranteed
/// never to throw exceptions. Methods on all other types may.
/// </remarks>
public static class Log
{
    static ILogger _logger = Kavita.Logger.Logger.None;

    /// <summary>
    /// The globally-shared logger.
    /// </summary>
    /// <exception cref="ArgumentNullException">When <paramref name="value"/> is <code>null</code></exception>
    public static ILogger Logger
    {
        get => _logger;
    }

    /// <summary>
    /// Resets <see cref="Logger"/> to the default and disposes the original if possible
    /// </summary>
    public static void CloseAndFlush()
    {
        var logger = Interlocked.Exchange(ref _logger, Kavita.Logger.Logger.None);

        (logger as IDisposable)?.Dispose();
    }

    /// <summary>
    /// Write an event to the log.
    /// </summary>
    /// <param name="logEvent">The event to write.</param>
    public static void Write(LogEvent logEvent)
    {
        Logger.Write(logEvent);
    }

    /// <summary>
    /// Write a log event with the specified level.
    /// </summary>
    /// <param name="level">The level of the event.</param>
    /// <param name="format">Message template describing the event.</param>
    public static void Write(LogEventLevel level, string format)
    {
        Logger.Write(level, format);
    }

    /// <summary>
    /// Write a log event with the specified level and associated exception.
    /// </summary>
    /// <param name="level">The level of the event.</param>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    
    public static void Write(LogEventLevel level, Exception exception, string format)
    {
        Logger.Write(level, exception, format);
    }

    /// <summary>
    /// Determine if events at the specified level will be passed through
    /// to the log sinks.
    /// </summary>
    /// <param name="level">Level to check.</param>
    /// <returns><see langword="true"/> if the level is enabled; otherwise, <see langword="false"/>.</returns>
    public static bool IsEnabled(LogEventLevel level) => Logger.IsEnabled(level);

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Verbose"/> level.
    /// </summary>
    /// <param name="format">Message template describing the event.</param>
    /// <example>
    /// Log.Verbose("Staring into space, wondering if we're alone.");
    /// </example>
    
    public static void Verbose(string format)
    {
        Write(LogEventLevel.Verbose, format);
    }

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Verbose"/> level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    /// <example>
    /// Log.Verbose(ex, "Staring into space, wondering where this comet came from.");
    /// </example>
    
    public static void Verbose(Exception exception, string format)
    {
        Write(LogEventLevel.Verbose, exception, format);
    }
    
    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Debug"/> level.
    /// </summary>
    /// <param name="format">Message template describing the event.</param>
    /// <example>
    /// Log.Debug("Starting up at {StartedAt}.", DateTime.Now);
    /// </example>
    
    public static void Debug(string format)
    {
        Write(LogEventLevel.Debug, format);
    }

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Debug"/> level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    /// <example>
    /// Log.Debug(ex, "Swallowing a mundane exception.");
    /// </example>
    
    public static void Debug(Exception exception, string format)
    {
        Write(LogEventLevel.Debug, exception, format);
    }

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Information"/> level.
    /// </summary>
    /// <param name="format">Message template describing the event.</param>
    /// <example>
    /// Log.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
    /// </example>
    
    public static void Information(string format)
    {
        Write(LogEventLevel.Information, format);
    }

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Information"/> level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    /// <example>
    /// Log.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
    /// </example>
    
    public static void Information(Exception exception, string format)
    {
        Write(LogEventLevel.Information, exception, format);
    }

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Warning"/> level.
    /// </summary>
    /// <param name="format">Message template describing the event.</param>
    /// <example>
    /// Log.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
    /// </example>
    
    public static void Warning(string format)
    {
        Write(LogEventLevel.Warning, format);
    }

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Warning"/> level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    /// <example>
    /// Log.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
    /// </example>
    
    public static void Warning(Exception exception, string format)
    {
        Write(LogEventLevel.Warning, exception, format);
    }

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Error"/> level.
    /// </summary>
    /// <param name="format">Message template describing the event.</param>
    /// <example>
    /// Log.Error("Failed {ErrorCount} records.", brokenRecords.Length);
    /// </example>
    
    public static void Error(string format)
    {
        Write(LogEventLevel.Error, format);
    }

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Error"/> level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    /// <example>
    /// Log.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
    /// </example>
    
    public static void Error(Exception exception, string format)
    {
        Write(LogEventLevel.Error, exception, format);
    }

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Fatal"/> level.
    /// </summary>
    /// <param name="format">Message template describing the event.</param>
    /// <example>
    /// Log.Fatal("Process terminating.");
    /// </example>
    
    public static void Fatal(string format)
    {
        Write(LogEventLevel.Fatal, format);
    }

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Fatal"/> level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    /// <example>
    /// Log.Fatal(ex, "Process terminating.");
    /// </example>
    
    public static void Fatal(Exception exception, string format)
    {
        Write(LogEventLevel.Fatal, exception, format);
    }
}