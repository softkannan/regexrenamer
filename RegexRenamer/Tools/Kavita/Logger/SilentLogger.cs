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

namespace Kavita.Logger
{

    sealed class SilentLogger : ILogger
    {
        public void Write(LogEvent logEvent)
        {
        }

        public void Write(LogEventLevel level, string format, params object[] args)
        {
        }

        public void Write(LogEventLevel level, Exception exception, string format, params object[] args)
        {
        }

        public bool IsEnabled(LogEventLevel level) => false;

        public void Verbose(string format, params object[] args)
        {
        }

        public void Verbose(Exception exception, string format, params object[] args)
        {
        }

        public void Debug(string format, params object[] args)
        {
        }

        public void Debug(Exception exception, string format, params object[] args)
        {

        }

        public void Information(string format, params object[] args)
        {
        }

        public void Information(Exception exception, string format, params object[] args)
        {
        }

        public void Warning(string format, params object[] args)
        {
        }

        public void Warning(Exception exception, string format, params object[] args)
        {
        }

        public void Error(string format, params object[] args)
        {
        }

        public void Error(Exception exception, string format, params object[] args)
        {
        }

        public void Fatal(string format, params object[] args)
        {
        }

        public void Fatal(Exception exception, string format, params object[] args)
        {
        }
    }
}