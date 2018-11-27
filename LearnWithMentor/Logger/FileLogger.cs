using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace LearnWithMentor.Logger
{
    public class FileLogger : ILogger
    {
        private string filePath;
        private object _lock = new object();
        public FileLogger(string path)
        {
            filePath = path;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == LogLevel.Trace;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                lock (_lock)
                {
                    using (var stream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write))
                    {
                        using (var writer = new StreamWriter(stream))
                        {
                            string logText = formatter(state, exception) + Environment.NewLine;
                            writer.Write(logText);
                        }
                    }
                }
            }
        }
    }
}
