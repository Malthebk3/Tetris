using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using System;
using Tetris.Core.Game;
using Tetris.Avalonia.Input;
using Tetris.Avalonia.Rendering;
using Tetris.Avalonia.ViewModels;
using Tetris.Avalonia.GameLoop;

namespace Tetris.Avalonia.Views;

public partial class MainWindow : Window
{
    private MainViewModel _vm;

    public MainWindow()
    {
        InitializeComponent();
        
        var loop = new AvaloniaGameLoop();

        _vm = new MainViewModel(loop);
        DataContext = _vm;
        
        // Wire keyboard at Window level (receives all key events)
        KeyDown += (s, e) => _vm.OnKeyDown(e.Key);
        KeyUp += (s, e) => _vm.OnKeyUp(e.Key);

        loop.Start();
    }
}