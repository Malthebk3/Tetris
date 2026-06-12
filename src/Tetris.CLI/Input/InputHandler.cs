using Tetris.Core.Game;

namespace Tetris.CLI.Input;

public class InputHandler : IInputHandler
{
    public bool Left { get; private set; }
    public bool Right { get; private set; }
    public bool Rotate { get; private set; }
    public bool SoftDrop { get; private set; }
    public bool HardDrop { get; private set; }

    public void Update()
    {
        Left = Right = Rotate = SoftDrop = HardDrop = false;

        while (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;

            if (key == ConsoleKey.LeftArrow)    Left = true;
            if (key == ConsoleKey.RightArrow)   Right = true;
            if (key == ConsoleKey.UpArrow)      Rotate = true;
            if (key == ConsoleKey.DownArrow)    SoftDrop = true;
            if (key == ConsoleKey.Spacebar)     HardDrop = true;
        }
    }
}