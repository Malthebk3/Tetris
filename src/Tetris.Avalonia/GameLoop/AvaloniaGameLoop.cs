using System;
using System.Diagnostics;
using Avalonia.Threading;
using Tetris.Core.Game;

namespace Tetris.Avalonia.GameLoop;

public class AvaloniaGameLoop : IGameLoop
{
    private readonly DispatcherTimer _timer;
    private readonly Stopwatch _sw;
    private double _lastElapsed;

    public event Action<double>? OnUpdate;

    public AvaloniaGameLoop()
    {
        _timer = new DispatcherTimer();
        _sw = new Stopwatch();
        
        // 10ms = ~100 FPS
        _timer.Interval = TimeSpan.FromMilliseconds(10); 
        _timer.Tick += OnTimerTick;
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        double now = _sw.Elapsed.TotalSeconds;
        
        // Cap delta to 0.1s to prevent huge jumps if the app freezes
        double delta = Math.Min(now - _lastElapsed, 0.1); 
        _lastElapsed = now;

        // Fire the event to tell the ViewModel to update
        OnUpdate?.Invoke(delta);
    }

    public void Start()
    {
        _lastElapsed = _sw.Elapsed.TotalSeconds;
        _sw.Start();
        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
        _sw.Stop();
    }
}