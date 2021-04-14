using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceManager.Common.Helpers;
using ServiceManager.Common.Models;
using System;

namespace ServiceManager.WindowsService
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //db context
                    services.AddDbContext<ServiceManagerContext>(options => options.UseSqlServer(Config.GetConnectionString(nameof(ServiceManagerContext))));

                    ////services
                    //services.AddScoped<IBaseService, BaseService>();
                    //services.AddScoped<ImportService>();
                    //services.AddScoped<ConfigService>();

                    //services.AddScoped<inContactAPI>();

                    //services.AddScoped<ServiceProcessor>();
                    //services.AddScoped<EmailHelper>();
                    //services.AddScoped<IEmailSender, EmailSender>();

                    //services.AddScoped<StatusUpdateHelper>();
                    //services.AddScoped<EventLogger>();

                    ////email helpers
                    //var emailConfig = Config.GetEmailConfig("EmailConfiguration");
                    //services.AddSingleton(emailConfig);

                    services.AddHostedService<Worker>();
                }).UseWindowsService();
    }
}
