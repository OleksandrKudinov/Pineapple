using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Pineapple.Service
{
    public class Program
    {
        public static void Main(String[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
