namespace HealthCare.Logging
{
    using Serilog.Core;
    using Serilog.Events;
    using System;
    using HealthCare.Models;
    using Microsoft.Extensions.DependencyInjection;

    public class EFCoreSink : ILogEventSink
    {
        private readonly IServiceProvider _serviceProvider;

        public EFCoreSink(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<HealthcaredbContext>();
                var logEntry = new LogEntry
                {
                    Message = logEvent.RenderMessage(),
                    MessageTemplate = logEvent.MessageTemplate.Text,
                    Level = logEvent.Level.ToString(),
                    TimeStamp = logEvent.Timestamp.UtcDateTime,
                    Exception = logEvent.Exception?.ToString(),
                    Properties = logEvent.Properties != null ? string.Join(", ", logEvent.Properties.Select(p => $"{p.Key}: {p.Value}")) : null,
                    LogEvent = logEvent.ToString()
                };
                context.LogEntries.Add(logEntry);
                context.SaveChanges();
            }
        }
    }

}
