using System;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using Tetris.Core.Game;
using Tetris.Avalonia.Input;
using Tetris.Avalonia.Rendering;

namespace Tetris.Avalonia.ViewModels;

public partial class MainViewModel : ViewModelBase, IDisposable
{
    private TetrisGame _game { get; set; } = new();
    public AvaloniaInputHandler Input { get; set; } = new();
    private AvaloniaRenderer? _renderer; // Assigned from View

    private readonly IGameLoop _loop;

    [ObservableProperty] private int _score;
    [ObservableProperty] private int _level;
    [ObservableProperty] private int _lines;
    [ObservableProperty] private bool _isGameOver;
    [ObservableProperty] private Tetromino? _nextPiece;

    public MainViewModel(IGameLoop loop)
    {
        _loop = loop;
        _loop.OnUpdate += OnGameTick;
    }

    public void AttachRenderer(AvaloniaRenderer renderer) => _renderer = renderer;

    private void OnGameTick(double delta)
    {
        _game.Update(Input, delta);
        Input.Update(); 
        SyncToUI();      
        _renderer?.Draw(_game.Board, _game.CurrentPiece, _game.NextPiece, _game.Score, _game.Level);

        if (_game.IsGameOver)
        {
            _loop.Stop(); // Stop the loop
            IsGameOver = true;
        }
    }
    private void SyncToUI()
    {
        Score = _game.Score;
        Level = _game.Level;
        Lines = _game.LinesCleared;
        IsGameOver = _game.IsGameOver;
        NextPiece = _game.NextPiece;
    }

    // Called from MainWindow
    public void OnKeyDown(Key key) => Input.OnKeyDown(key);
    public void OnKeyUp(Key key) => Input.OnKeyUp(key);
    public void Dispose()
    {
        _loop.Stop();
        _loop.OnUpdate -= OnGameTick;
    }
}