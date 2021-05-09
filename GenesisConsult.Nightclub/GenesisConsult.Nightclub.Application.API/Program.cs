using GenesisConsult.Nightclub.Application.API.IoC;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace GenesisConsult.Nightclub.Application.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureServices(services => ContainerConfiguration.Initialize(services));
    }
}
