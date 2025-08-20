using Microsoft.Extensions.Configuration;
using System.IO;

namespace core.Util
{
    public class Settings
    {
        public static IConfigurationRoot Configuration { get; set; }
        public string Appsettings(string par)
        {

            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            return Configuration["AppSettings:" + par].ToString();
        }

    }
}
