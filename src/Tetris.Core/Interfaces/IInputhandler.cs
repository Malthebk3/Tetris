namespace Tetris.Core.Game;

public interface IInputHandler
{
    bool Left { get; }
    bool Right { get; }
    bool Rotate { get; }
    bool SoftDrop { get; }
    bool HardDrop { get; }
    void Update();
}