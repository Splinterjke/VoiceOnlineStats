using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VoiceOnlineStats.Services
{
    public class LiteDBService : IDBService
    {
        private readonly IConfiguration Configuration;
        public LiteDBService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string DBSource { get => Configuration["AppSettings:DBSource"];}
    }
}
