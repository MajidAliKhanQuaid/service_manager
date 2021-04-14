using ServiceManager.Common.Helpers;
using ServiceManager.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.WindowsService.BLL.Services
{
    public class SystemServiceManager : ISystemServiceManager
    {
        private readonly ServiceManagerContext _context;
        private const string START_SERVICE_COMMAND = "start";
        private const string STOP_SERVICE_COMMAND = "stop";

        public SystemServiceManager(ServiceManagerContext context)
        {
            _context = context;
        }

        public async Task<int> AddServiceAsync(SystemService service)
        {
            _context.SystemService.Add(service);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteService(Guid serviceId)
        {
            var service = _context.SystemService.Find(serviceId);
            if (service != null)
            {
                _context.SystemService.Remove(service);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        private void ToggleService(string serviceName, bool turnOn)
        {
            string command = turnOn ? "start" : "stop";
            ExecuteServiceComamnd(command, serviceName);
        }


        public SystemService GetService(Guid serviceId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// System Services are from operating system
        /// </summary>
        /// <returns></returns>
        public List<ServiceController> GetServiceControllers()
        {
            return ServiceController.GetServices().ToList();
        }

        public ServiceStatus GetServiceStatus(Guid serviceId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> StartServiceAsync(Guid serviceId)
        {
            var service = await _context.SystemService.FindAsync(serviceId);
            string logMessage = ExecuteServiceComamnd(START_SERVICE_COMMAND, service.Name);
            return logMessage;
        }

        public async Task<string> StopServiceAsync(Guid serviceId)
        {
            var service = _context.SystemService.Find(serviceId);
            string logMessage = ExecuteServiceComamnd(STOP_SERVICE_COMMAND, service.Name);
            return logMessage;
        }

        private string ExecuteServiceComamnd(string command, string serviceName)
        {
            StringBuilder builder = new StringBuilder();
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            //System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/C net [start or stop] [service name]");  
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("cmd.exe", $"/C net {command} {serviceName}");
            psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //process.StartInfo.RedirectStandardOutput = true;
            //process.OutputDataReceived += (sender, args) =>
            //{
            //    builder.Append(args);
            //};
            process.StartInfo = psi;
            process.Start();
            //process.BeginOutputReadLine();
            //process.WaitForExit();
            //process.CancelOutputRead();

            return builder.ToString();
        }

    }
}
