using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ServiceManager.Common.Helpers;
using ServiceManager.Common.Models;
using ServiceManager.WindowsService.BLL.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceManager.WindowsService
{
    public class Worker : BackgroundService
    {
        private readonly EventLogger _eventLogger;
        public Worker()
        {
            _eventLogger = new EventLogger();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessCommandRequests();
                await UpdateServiceStatus();

                _eventLogger.AddLogEntry(string.Empty, "INFO", "Thread Sleeping for 1 Minutes", nameof(ExecuteAsync));
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
            await Task.FromResult(0);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }

        private async Task UpdateServiceStatus()
        {
            try
            {
                _eventLogger.AddLogEntry(string.Empty, "INFO", "Starting to Update Service Statuses", nameof(UpdateServiceStatus));

                using (ServiceManagerContext context = new ServiceManagerContext())
                {
                    int updatesCount = 0;
                    var sServiceManager = new SystemServiceManager(context);
                    var serviceControllers = sServiceManager.GetServiceControllers();
                    var services = await context.SystemService.Where(x => x.Machine.Identifier == System.Environment.MachineName).ToListAsync();
                    foreach (var service in services)
                    {
                        var serviceController = serviceControllers.Where(x => x.ServiceName.ToUpper() == service.Name.ToUpper()).FirstOrDefault();
                        if (serviceController != null)
                        {
                            int controllerStatusId = (int)serviceController.Status;
                            if ((int)service.ServiceStatus != controllerStatusId)
                            {
                                service.ServiceStatus = (ServiceStatus)controllerStatusId;
                                service.LastStatusUpdatedUtc = DateTime.UtcNow;
                                updatesCount += 1;
                            }
                        }
                        else
                        {
                            // if a service is not found
                            if (service.ServiceStatus != ServiceStatus.NotFound)
                            {
                                service.ServiceStatus = ServiceStatus.NotFound;
                                service.LastStatusUpdatedUtc = DateTime.UtcNow;
                                updatesCount += 1;
                            }
                        }
                    }

                    if (updatesCount > 0)
                    {
                        await context.SaveChangesAsync();
                        _eventLogger.AddLogEntry(string.Empty, "INFO", $"`{updatesCount}` services were updated", nameof(UpdateServiceStatus));
                    }
                }
            }
            catch (Exception e)
            {
                _eventLogger.AddLogEntry(string.Empty, "ERROR", e.Message, nameof(UpdateServiceStatus));
            }

        }

        private async Task ProcessCommandRequests()
        {
            try
            {
                _eventLogger.AddLogEntry(string.Empty, "INFO", "Starting to Process Command Requests", nameof(UpdateServiceStatus));

                string machineName = System.Environment.MachineName;

                using (ServiceManagerContext context = new ServiceManagerContext())
                {
                    var requests = await context.CommandRequest.Where(x => x.LastProcessedUtc == null && x.MachineIdentifier == machineName).ToListAsync();
                    foreach (var request in requests)
                    {
                        _eventLogger.AddLogEntry(string.Empty, "INFO", $"Processing request {request.Id}", nameof(ExecuteAsync));

                        try
                        {
                            var sServiceManager = new SystemServiceManager(context);
                            string logMessage = string.Empty;

                            if (request.Command == "start")
                                logMessage = await sServiceManager.StartServiceAsync(request.ServiceId);
                            else
                                logMessage = await sServiceManager.StopServiceAsync(request.ServiceId);

                            _eventLogger.AddLogEntry(string.Empty, "INFO", $"Request `{request.Id}`, response is `{logMessage}`", nameof(ExecuteAsync));

                            var response = new CommandResponse() { Command = request.Command, ConsoleMessage = logMessage, CreatedOnUtc = DateTime.UtcNow, ServiceRequestCommandId = request.Id, MachineIdentifier = machineName };
                            context.CommandResponse.Add(response);
                            request.LastProcessedUtc = DateTime.UtcNow;
                        }
                        catch (Exception eInner)
                        {
                            _eventLogger.AddLogEntry(string.Empty, "ERROR", eInner.Message, nameof(ProcessCommandRequests));
                        }

                        await context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception e)
            {
                _eventLogger.AddLogEntry(string.Empty, "ERROR", e.Message, nameof(ProcessCommandRequests));
            }
        }

    }
}
