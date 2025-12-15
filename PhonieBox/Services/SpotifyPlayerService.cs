using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;
using PhonieBox.Services.Abstraction;

namespace PhonieBox.Services
{
  /// <summary> Wrapper for spotifyd </summary>
  public class SpotifyPlayerService : ISpotifyPlayerService, IDisposable
  {
    private Process _playerProcess;
    private bool disposedValue;
    private readonly ILogger<ISpotifyPlayerService> _logger;

    public SpotifyPlayerService(ILogger<ISpotifyPlayerService> logger)
    {
      _logger = logger;
    }

    public bool IsPlayerRunning()
    {
      return _playerProcess != null && !_playerProcess.HasExited;
    }

    private string Command
    {
      get
      {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
          return "-e ./lib/x86_64/spotifyd";
        else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && (RuntimeInformation.ProcessArchitecture == Architecture.Arm || RuntimeInformation.ProcessArchitecture == Architecture.Arm64))
          return "lib/armv7/spotifyd";
        throw new NotSupportedException("Unsupported OS platform or architecture for spotifyd");
      }
    }

    public void StartPlayer()
    {
      _playerProcess?.Dispose();

      var startInfo = new ProcessStartInfo
      {
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        CreateNoWindow = true
      };

      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        startInfo.FileName = "wsl.exe";
      else
        startInfo.FileName = Command;

      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        startInfo.Arguments = Command;

      _playerProcess = new Process
      {
        StartInfo = startInfo,
        EnableRaisingEvents = true
      };
      _playerProcess.ErrorDataReceived += (sender, args) => _logger.LogError(args.Data);
      _playerProcess.OutputDataReceived += (sender, args) => _logger.LogInformation(args.Data);

      _playerProcess.Start();
      _playerProcess.BeginErrorReadLine();
      _playerProcess.BeginOutputReadLine();
    }

    public string Config
    {
      get
      {
        return File.ReadAllText("/etc/raspotify/conf");
      }
      set
      {
        File.WriteAllText("/etc/raspotify/conf", value);
      }
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          _playerProcess.Dispose();
        }

        // TODO: free unmanaged resources (unmanaged objects) and override finalizer
        // TODO: set large fields to null
        disposedValue = true;
      }
    }

    public void Dispose()
    {
      // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
      Dispose(disposing: true);
      GC.SuppressFinalize(this);
    }
  }
}
