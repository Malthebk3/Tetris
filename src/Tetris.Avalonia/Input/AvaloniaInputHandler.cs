using Avalonia.Input;
using Tetris.Core.Game;

namespace Tetris.Avalonia.Input;

public class AvaloniaInputHandler : IInputHandler
{
    // Held states (true while key is physically pressed)
    public bool Left { get; private set; }
    public bool Right { get; private set; }
    public bool Rotate { get; private set; }
    public bool SoftDrop { get; private set; }
    public bool HardDrop { get; private set; }

    // Called by Avalonia Window events
    public void OnKeyDown(Key key)
    {
        switch (key)
        {
            case Key.Left:  Left = true; break;
            case Key.Right: Right = true; break;
            case Key.Up:    Rotate = true; break;
            case Key.Down:  SoftDrop = true; break;
            case Key.Space: HardDrop = true; break;
        }
    }

    public void OnKeyUp(Key key)
    {
        switch (key)
        {
            case Key.Left:  Left = false; break;
            case Key.Right: Right = false; break;
            case Key.Up:    Rotate = false; break;
            case Key.Down:  SoftDrop = false; break;
            case Key.Space: HardDrop = false; break;
        }
    }
    public void Update()
    {
        Left = Right = Rotate = SoftDrop = HardDrop = false;
    }
}