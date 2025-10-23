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
using System.Diagnostics;

namespace Kavita.Logger;


/// <summary>
/// The core Serilog logging API, used for writing log events.
/// </summary>
/// <example><code>
/// var log = new LoggerConfiguration()
///     .WriteTo.Console()
///     .CreateLogger();
///
/// var thing = "World";
/// log.Information("Hello, {Thing}!", thing);
/// </code></example>
/// <remarks>
/// The methods on <see cref="ILogger"/> (and its static sibling <see cref="Log"/>) are guaranteed
/// never to throw exceptions. Methods on all other types may.
/// </remarks>
public interface ILogger
{
    /// <summary>
    /// Write an event to the log.
    /// </summary>
    /// <param name="logEvent">The event to write.</param>
    void Write(LogEvent logEvent);

    /// <summary>
    /// Write a log event with the specified level.
    /// </summary>
    /// <param name="level">The level of the event.</param>
    /// <param name="format">Message template describing the event.</param>
    void Write(LogEventLevel level, string format, params object[] args);

    /// <summary>
    /// Write a log event with the specified level and associated exception.
    /// </summary>
    /// <param name="level">The level of the event.</param>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    void Write(LogEventLevel level, Exception exception, string format, params object[] args);

    /// <summary>
    /// Determine if events at the specified level will be passed through
    /// to the log sinks.
    /// </summary>
    /// <param name="level">Level to check.</param>
    /// <returns><see langword="true"/> if the level is enabled; otherwise, <see langword="false"/>.</returns>
    bool IsEnabled(LogEventLevel level);

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Verbose"/> level.
    /// </summary>
    /// <param name="format">Message template describing the event.</param>
    /// <example><code>
    /// Log.Verbose("Staring into space, wondering if we're alone.");
    /// </code></example>
    void Verbose(string format, params object[] args);

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Verbose"/> level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    /// <example><code>
    /// Log.Verbose(ex, "Staring into space, wondering where this comet came from.");
    /// </code></example>
    void Verbose(Exception exception, string format, params object[] args);

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Debug"/> level.
    /// </summary>
    /// <param name="format">Message template describing the event.</param>
    /// <example><code>
    /// Log.Debug("Starting up at {StartedAt}.", DateTime.Now);
    /// </code></example>
    void Debug(string format, params object[] args);
    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Debug"/> level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    /// <example><code>
    /// Log.Debug(ex, "Swallowing a mundane exception.");
    /// </code></example>
    void Debug(Exception exception, string format, params object[] args);

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Information"/> level.
    /// </summary>
    /// <param name="format">Message template describing the event.</param>
    /// <example><code>
    /// Log.Information("Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
    /// </code></example>
    void Information(string format, params object[] args);

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Information"/> level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    /// <example><code>
    /// Log.Information(ex, "Processed {RecordCount} records in {TimeMS}.", records.Length, sw.ElapsedMilliseconds);
    /// </code></example>
    void Information(Exception exception, string format, params object[] args);

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Warning"/> level.
    /// </summary>
    /// <param name="format">Message template describing the event.</param>
    /// <example><code>
    /// Log.Warning("Skipped {SkipCount} records.", skippedRecords.Length);
    /// </code></example>
    void Warning(string format, params object[] args);

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Warning"/> level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    /// <example><code>
    /// Log.Warning(ex, "Skipped {SkipCount} records.", skippedRecords.Length);
    /// </code></example>

    void Warning(Exception exception, string format, params object[] args);

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Error"/> level.
    /// </summary>
    /// <param name="format">Message template describing the event.</param>
    /// <example><code>
    /// Log.Error("Failed {ErrorCount} records.", brokenRecords.Length);
    /// </code></example>

    void Error(string format, params object[] args);

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Error"/> level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    /// <example><code>
    /// Log.Error(ex, "Failed {ErrorCount} records.", brokenRecords.Length);
    /// </code></example>

    void Error(Exception exception, string format, params object[] args);

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Fatal"/> level.
    /// </summary>
    /// <param name="format">Message template describing the event.</param>
    /// <example><code>
    /// Log.Fatal("Process terminating.");
    /// </code></example>

    void Fatal(string format, params object[] args);

    /// <summary>
    /// Write a log event with the <see cref="LogEventLevel.Fatal"/> level and associated exception.
    /// </summary>
    /// <param name="exception">Exception related to the event.</param>
    /// <param name="format">Message template describing the event.</param>
    /// <example><code>
    /// Log.Fatal(ex, "Process terminating.");
    /// </code></example>

    void Fatal(Exception exception, string format, params object[] args);
}