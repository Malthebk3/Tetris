using Avalonia.Controls;
using Avalonia.Input;
using Tetris.Avalonia.Rendering;
using Tetris.Avalonia.ViewModels;

namespace Tetris.Avalonia.Views;
public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        
        // DataContext is set by MainWindow after this constructor
        this.DataContextChanged += (s, e) =>
        {
            if (DataContext is MainViewModel vm)
            {
                vm.AttachRenderer(new AvaloniaRenderer(GameCanvas, NextPieceCanvas));
            }
        };
    }
}