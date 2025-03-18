using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Compact;

namespace ChallengeRickAndMortyApi.Infrastructure.Logging
{
    public class SerilogConfig
    {
        public static void ConfigureLogging(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)  
                .CreateLogger();
        }
    }
}
