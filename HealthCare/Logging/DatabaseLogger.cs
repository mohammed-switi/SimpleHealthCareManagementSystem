using HealthCare.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HealthCare.Logging
{
    public class DatabaseLogger : ILogger
    {
        private readonly Func<LogLevel, bool> _filter;
        private readonly IServiceProvider _serviceProvider;

        public DatabaseLogger(Func<LogLevel, bool> filter, IServiceProvider serviceProvider)
        {
            _filter = filter;
            _serviceProvider = serviceProvider;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => _filter(logLevel);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var message = formatter(state, exception);
            SaveLog(logLevel, message, exception).Wait();
        }

        private async Task SaveLog(LogLevel logLevel, string message, Exception exception)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<HealthcaredbContext>();
                var logEntry = new LogEntry
                {
                    Level = logLevel.ToString(),
                    Message = message,
                    Exception = exception?.ToString(),
                    TimeStamp = DateTime.UtcNow
                };
                context.LogEntries.Add(logEntry);
                await context.SaveChangesAsync();
            }
        }
    }
}
