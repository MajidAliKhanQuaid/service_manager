using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceManager.Common.Helpers;

namespace ServiceManager.DummyService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                new EventLogger().AddLogEntry("DummyService", "INFO", "Service is Running", nameof(ExecuteAsync));
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
