using ServiceManager.Common.Models;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ServiceManager.WindowsService.BLL.Services
{
    public interface ISystemServiceManager
    {
        Task<int> AddServiceAsync(SystemService service);
        Task<int> DeleteService(Guid serviceId);
        Task<string> StartServiceAsync(Guid serviceId);
        Task<string> StopServiceAsync(Guid serviceId);
        List<ServiceController> GetServiceControllers();
        SystemService GetService(Guid serviceId);
        ServiceStatus GetServiceStatus(Guid serviceId);
    }
}
