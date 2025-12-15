namespace PhonieBox.Services.Abstraction
{
  public interface ISpotifyPlayerService
  {
    bool IsPlayerRunning();
    void StartPlayer();
    string Config { get; set; }
  }
}
