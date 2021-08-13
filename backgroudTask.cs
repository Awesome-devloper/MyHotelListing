using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyHotelListing
{

    public class backgroudTask : BackgroundService
    {
        private readonly ILogger<backgroudTask> _logger;
        private readonly IServiceProvider _serviceProvider;

        //private readonly IScopedService scopedService;

        //  public backgroudTask(ILogger<backgroudTask> logger,IScopedService scopedService)
        public backgroudTask(IServiceProvider serviceProvider,ILogger<backgroudTask> logger)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            // this.scopedService = scopedService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using (var scop= _serviceProvider.CreateScope())
                {
                    Console.WriteLine("background servecie{dateTime}", DateTime.Now);
                    var scopedService = scop.ServiceProvider.GetRequiredService<IScopedService>();
                    scopedService.write();
                    _logger.LogInformation("background servecie {dateTime}", DateTime.Now);
                    await Task.Delay(TimeSpan.FromMilliseconds(1000), stoppingToken);
                }
     
            }


           // return Task.CompletedTask;
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("background servecie {dateTime}",DateTime.Now);
            return base.StopAsync(cancellationToken);
        }
    }
}
