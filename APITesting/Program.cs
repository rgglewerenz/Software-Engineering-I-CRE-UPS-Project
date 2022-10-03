using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace APITesting
{
    internal class Program
    {  
        static async Task Main(string[] args)
        {
            await (new App(Build())).Run();
        }
        static IConfigurationRoot Build()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json");
            return builder.Build();
        }
    }
}
