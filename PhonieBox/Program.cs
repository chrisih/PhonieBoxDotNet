using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhonieBox.Services;
using PhonieBox.Services.Abstraction;

namespace PhonieBox
{
  internal class Program
  {
    static void Main(string[] args)
    {
      var host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, services) =>
        {
          // Register services here
           services.AddSingleton<ISpotifyPlayerService, SpotifyPlayerService>();
        })
        .Build();

      // Connect Spotify
      // Connect RFID Reader
      // Play Audio

      var player = host.Services.GetRequiredService<ISpotifyPlayerService>();
      player.StartPlayer();

      host.Run();
    }
  }
}
