namespace Tetris.Core.Game;

public interface IRenderer
{
    void Draw(Board board, Tetromino currentPiece, Tetromino nextPiece, int score, int level);
}