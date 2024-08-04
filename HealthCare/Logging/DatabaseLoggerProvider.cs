using Microsoft.Extensions.Logging;
using System;

namespace HealthCare.Logging
{
    public class DatabaseLoggerProvider : ILoggerProvider
    {
        private readonly Func<LogLevel, bool> _filter;
        private readonly IServiceProvider _serviceProvider;

        public DatabaseLoggerProvider(Func<LogLevel, bool> filter, IServiceProvider serviceProvider)
        {
            _filter = filter ?? throw new ArgumentNullException(nameof(filter));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DatabaseLogger(_filter, _serviceProvider);
        }

        public void Dispose() { }
    }
}
