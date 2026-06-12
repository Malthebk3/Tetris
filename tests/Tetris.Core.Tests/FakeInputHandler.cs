using Tetris.Core.Game;

namespace Tetris.Core.Tests;

/// <summary>
/// A fake input handler used to simulate keyboard presses during unit tests.
/// </summary>
public class FakeInputHandler : IInputHandler
{
    public bool Left { get; set; }
    public bool Right { get; set; }
    public bool Rotate { get; set; }
    public bool SoftDrop { get; set; }
    public bool HardDrop { get; set; }

    public void Update()
    {
        // In a real test, we don't need to clear flags automatically.
        // We just set them manually before calling game.Update().
    }
}