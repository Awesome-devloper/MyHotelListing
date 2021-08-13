using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyHotelListing
{
    public class MyScopedService : IScopedService
    {
        private readonly ILogger<MyScopedService> _logger;
        public MyScopedService(ILogger<MyScopedService> logger)
        {
            _logger = logger;
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }

        public void write()
        {
            _logger.LogInformation("test service {Id}", Id);
        }
    }
}
