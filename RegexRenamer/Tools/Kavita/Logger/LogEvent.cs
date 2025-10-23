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
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
// ReSharper disable IntroduceOptionalParameters.Global

namespace Kavita.Logger;


/// <summary>
/// A log event.
/// </summary>
public class LogEvent
{
    /// <summary>
    /// Construct a new <seealso cref="LogEvent"/>.
    /// </summary>
    /// <param name="timestamp">The time at which the event occurred.</param>
    /// <param name="level">The level of the event.</param>
    /// <param name="exception">An exception associated with the event, or null.</param>
    public LogEvent(DateTimeOffset timestamp, LogEventLevel level, Exception exception, string format, object[] args)
    {
        Timestamp = timestamp;
        Level = level;
        Exception = exception;
        Format = format;
        Args = args;
    }

    /// <summary>
    /// The time at which the event occurred.
    /// </summary>
    public DateTimeOffset Timestamp { get; }

    /// <summary>
    /// The level of the event.
    /// </summary>
    public LogEventLevel Level { get; }

    /// <summary>
    /// Arguments
    /// </summary>
    public object[] Args { get; }

    /// <summary>
    /// string format string
    /// </summary>
    public string Format { get; }

    /// <summary>
    /// Render the message template to the specified output, given the properties associated
    /// with the event.
    /// </summary>
    /// <param name="output">The output.</param>
    /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
    public void RenderMessage(TextWriter output, IFormatProvider formatProvider = null)
    {
        //MessageTemplate.Render(Properties, output, formatProvider);
    }

    /// <summary>
    /// Render the message template given the properties associated
    /// with the event, and return the result.
    /// </summary>
    /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
    public string RenderMessage(IFormatProvider formatProvider = null)
    {
        return string.Empty; //  MessageTemplate.Render(Properties, formatProvider);
    }

    /// <summary>
    /// An exception associated with the event, or null.
    /// </summary>
    public Exception Exception { get; }

    internal LogEvent Copy()
    {
        return new LogEvent(
            Timestamp,
            Level,
            Exception,
            Format,
            Args
            );
    }
}