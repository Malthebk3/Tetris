using System;
using System.Diagnostics;
using System.Threading;
using Tetris.Core.Game; // For the IGameLoop interface

namespace Tetris.CLI.GameLoop;

public class CliGameLoop : IGameLoop
{
    private bool _isRunning;
    
    // This event allows Program.cs to subscribe to the tick
    public event Action<double>? OnUpdate; 

    public void Start()
    {
        _isRunning = true;
        var stopwatch = Stopwatch.StartNew();
        double lastTime = stopwatch.Elapsed.TotalSeconds;

        // The actual game loop lives here now!
        while (_isRunning)
        {
            double currentTime = stopwatch.Elapsed.TotalSeconds;
            double deltaTime = Math.Min(currentTime - lastTime, 0.1); // Cap delta
            lastTime = currentTime;

            // Trigger the event, passing deltaTime to whoever is listening
            OnUpdate?.Invoke(deltaTime);

            Thread.Sleep(10); // Cap at ~100 FPS
        }
    }

    public void Stop()
    {
        _isRunning = false;
    }
}