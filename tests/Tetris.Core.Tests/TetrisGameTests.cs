using Xunit;
using Tetris.Core.Game;

namespace Tetris.Core.Tests;

public class TetrisGameTests
{
    // Helper method to keep tests clean
    private (TetrisGame game, FakeInputHandler input) CreateGameSetup()
    {
        var game = new TetrisGame();
        var input = new FakeInputHandler();
        return (game, input);
    }

    [Fact]
    public void HardDrop_ShouldLockPieceAtBottom()
    {
        // ARRANGE: Set up the game and fake input
        var (game, input) = CreateGameSetup();
        int pieceWidth = game.CurrentPiece.Shape.GetLength(1);
        int pieceHeight = game.CurrentPiece.Shape.GetLength(0);
        
        // ACT: Simulate pressing Spacebar and update the game
        input.HardDrop = true;
        game.Update(input, 0.01); // 0.010 is roughly 100 FPS delta time

        // ASSERT: Check if the piece was actually locked into the board
        // The piece should now be at the bottom of the board.
        // We check if the bottom row has blocks in it.
        int bottomRow = game.Board.Height - 1;
        bool isLocked = false;
        
        for (int x = 0; x < game.Board.Width; x++)
        {
            if (game.Board.Grid[bottomRow, x] != 0)
            {
                isLocked = true;
                break;
            }
        }

        Assert.True(isLocked, "The piece should be locked at the bottom of the board after a hard drop.");
    }

    [Fact]
    public void GameOver_ShouldBeTrue_WhenSpawningOnTopOfBlocks()
    {
        // ARRANGE: Create a game and manually fill the top row of the board
        var (game, input) = CreateGameSetup();
        
        // Fill the top row (row index 0) with blocks
        for (int x = 0; x < game.Board.Width; x++)
        {
            game.Board.Grid[0, x] = 1; // 1 represents a locked block
        }

        // ACT: Force the game to spawn a new piece
        game.SpawnNewPiece();

        // ASSERT: The game should realize the new piece has no valid space
        Assert.True(game.IsGameOver, "Game should be over if a piece spawns on top of existing blocks.");
    }

    [Fact]
    public void MoveLeft_ShouldDecreaseX_WhenValid()
    {
        // ARRANGE
        var (game, input) = CreateGameSetup();
        int originalX = game.CurrentPiece.X;

        // ACT
        input.Left = true;
        game.Update(input, 0.016);

        // ASSERT
        Assert.Equal(originalX - 1, game.CurrentPiece.X);
    }

    [Fact]
    public void ClearFullLines_ShouldRemoveCompletedRows()
    {
        // ARRANGE: Create a board and fill the bottom row completely
        var board = new Board();
        int lastRow = board.Height - 1;
        for (int x = 0; x < board.Width; x++)
        {
            board.Grid[lastRow, x] = 1; // Fill with blocks
        }

        // ACT: Tell the board to clear lines
        int linesCleared = board.ClearFullLines();

        // ASSERT: 1 line should be cleared, and the bottom row should now be empty
        Assert.Equal(1, linesCleared);
        
        for (int x = 0; x < board.Width; x++)
        {
            Assert.Equal(0, board.Grid[lastRow, x]);
        }
    }
}