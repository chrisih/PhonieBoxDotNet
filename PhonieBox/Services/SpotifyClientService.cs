using System;
using System.Collections.Generic;
using System.Text;
using SpotifyAPI.Web;

namespace PhonieBox.Services
{
  internal class SpotifyClientService
  {
    private readonly SpotifyClient _client;

    public SpotifyClientService()
    {
      var config = SpotifyClientConfig.CreateDefault().WithAuthenticator(new ClientCredentialsAuthenticator("771f28d4d5e9425cbaf9f5ff3a7fb7f7", "f13920b536274ff7a5052ffe6a578e2c"));
      _client = new SpotifyClient(config);
    }
  }
}
