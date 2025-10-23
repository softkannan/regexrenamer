// Copyright 2013-2016 Serilog Contributors
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
using System.Diagnostics;

namespace Kavita.Logger;


/// <summary>
/// The core Serilog logging pipeline. A <see cref="Logger"/> must
/// be disposed to flush any events buffered within it. Most application
/// code should depend on <see cref="ILogger"/>, not this class.
/// </summary>
public sealed class Logger : ILogger, IDisposable
{
    // It's important that checking minimum level is a very
    // quick (CPU-cacheable) read in the simple case, hence
    // we keep a separate field from the switch, which may
    // not be specified. If it is, we'll set _minimumLevel
    // to its lower limit and fall through to the secondary check.
    readonly LogEventLevel _minimumLevel;

    public Logger(LogEventLevel minimumLevel)
    {
        _minimumLevel = minimumLevel;
    }

    /// <summary>
    /// Write an event to the log.
    /// </summary>
    /// <param name="logEvent">The event to write.</param>
    public void Write(LogEvent logEvent)
    {
        if (!IsEnabled(logEvent.Level)) return;

        Dispatch(logEvent);
    }

    /// <summary>
    /// Write a log event with the specified level.
    /// </summary>
    /// <param name="level">The level of the event.</param>
    /// <param name="format">Message template describing the event.</param>
    public void Write(LogEventLevel level, string format, params object[] args)
    {
        Write(level, null, format, args);
    }

    /// <summary>
    /// Write a log event with the specified level and associated exception.
    /// </summary>
    /// <param name="level">The level of the event.</param>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    
    public void Write(LogEventLevel level, Exception exception, string format, params object[] args)
    {
        if (!IsEnabled(level)) return;

        var logTimestamp = DateTimeOffset.Now;

        var logEvent = new LogEvent(logTimestamp, level, exception,format,args);
        Dispatch(logEvent);
    }

    void Dispatch(LogEvent logEvent)
    {
        
    }

    /// <summary>
    /// Determine if events at the specified level, and higher, will be passed through
    /// to the log sinks.
    /// </summary>
    /// <param name="level">Level to check.</param>
    /// <returns><see langword="true"/> if the level is enabled; otherwise, <see langword="false"/>.</returns>
    public bool IsEnabled(LogEventLevel level)
    {
        if (level < _minimumLevel)
            return false;

        return true;
    }

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Verbose"/> level.
    /// </summary>
    /// <param name="format">Message template describing the event.</param>
    /// <example>
    /// Log.Verbose("Staring into space, wondering if we're alone.");
    /// </example>

    public void Verbose(string format, params object[] args)
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
    
    public void Verbose(Exception exception, string format, params object[] args)
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
    
    public void Debug(string format, params object[] args)
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
    
    public void Debug(Exception exception, string format, params object[] args)
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
    
    public void Information(string format, params object[] args)
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
    
    public void Information(Exception exception, string format, params object[] args)
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
    
    public void Warning(string format, params object[] args)
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
    
    public void Warning(Exception exception, string format, params object[] args)
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
    
    public void Error(string format, params object[] args)
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
    
    public void Error(Exception exception, string format, params object[] args)
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
    
    public void Fatal(string format, params object[] args)
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
    
    public void Fatal(Exception exception, string format, params object[] args)
    {
        Write(LogEventLevel.Fatal, exception, format);
    }

    /// <summary>
    /// Close and flush the logging pipeline.
    /// </summary>
    public void Dispose()
    {
    }

    /// <summary>
    /// An <see cref="ILogger"/> instance that efficiently ignores all method calls.
    /// </summary>
    public static ILogger None { get; } = new SilentLogger();
}