using core.Util;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Net.Http;
using System.Threading.Tasks;


namespace core
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            ConfigureLog4net();

            CreateWebHostBuilder(args).Build().Run();

        }

        private static void ConfigureLog4net()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

           
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            
            var fileAppender = logRepository.GetAppenders().FirstOrDefault(a => a.Name == "FileAppenderName") as log4net.Appender.RollingFileAppender;

            if (fileAppender != null)
            {
                
                string logFileName = GetLogFileName();

               
                fileAppender.File = logFileName;
                fileAppender.ActivateOptions();
            }
        }

        private static string GetLogFileName()
        {
            core.Util.Settings settings = new core.Util.Settings();
            string logDirectory = settings.Appsettings("Path"); 
            string currentDate = DateTime.Now.ToString("yyyyMMdd");
            string logFileName = $"{logDirectory}log_{currentDate}.log";

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            return logFileName;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
