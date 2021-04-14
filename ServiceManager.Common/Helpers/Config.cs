using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ServiceManager.Common.Helpers
{
    public static class Config
    {
        private static IConfiguration configuration;
        static Config()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            configuration = builder.Build();
        }

        public static string Get(string name)
        {
            string appSettings = configuration.GetSection(name).Value;
            return appSettings;
        }

        public static string GetKey(string name)
        {
            string appSettings = configuration.GetSection("Keys").GetSection(name).Value;
            return appSettings;
        }

        public static string GetConnectionString(string name)
        {
            //string appSettings = configuration["ConnectionStrings:" + name];            
            string appSettings = configuration.GetConnectionString(name);
            return appSettings;
        }
    }
}
