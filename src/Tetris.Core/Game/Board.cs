namespace Tetris.Core.Game;

public class Board
{
    public int Height { get; } = 20;
    public int Width { get; } = 10;
    public int[,] Grid = new int[20, 10];
    public bool IsInside(int x, int y)
    {
        return x >= 0 && y >= 0 && x < Width && y < Height;
    }
    public bool IsValidPosition(int[,] Shape, int pieceX, int pieceY)
    {
        for (int y = 0; y < Shape.GetLength(0); y++)
        {
            for (int x = 0; x < Shape.GetLength(1); x++)
            {
                // Skip empty spaces in the tetromino's bounding box
                if (Shape[y, x] == 0) continue;

                int boardX = pieceX + x;
                int boardY = pieceY + y;

                // Check walls/floor/ceiling
                if (!IsInside(boardX, boardY)) return false;

                // Check collision with already locked blocks
                if (Grid[boardY, boardX] != 0) return false;
            }
        }
        return true;
    }
    public void LockPiece(Tetromino piece)
    {
        for (int y = 0; y < piece.Shape.GetLength(0); y++)
        {
            for (int x = 0; x < piece.Shape.GetLength(1); x++)
            {
                //Skip empty spaces in the tetromino
                if (piece.Shape[y, x] == 0) continue;

                int boardX = piece.X + x;
                int boardY = piece.Y + y;

                Grid[boardY, boardX] = 1;
            }
        }
    }
    public int ClearFullLines()
    {
        int linesCleared = 0;
        int writeRow = Height - 1; // Where the next non-full row will be placed

        // Read from bottom to top
        for (int readRow = Height - 1; readRow >= 0; readRow--)
        {
            // Check if current row is full
            bool isFull = true;
            for (int x = 0; x < Width; x++)
            {
                if (Grid[readRow, x] == 0)
                {
                    isFull = false;
                    break;
                }
            }

            if (!isFull)
            {
                // Copy this row down to the writeRow position
                for (int x = 0; x < Width; x++)
                {
                    Grid[writeRow, x] = Grid[readRow, x];
                }
                writeRow--; // Move write pointer up
            }
            else
            {
                linesCleared++; // Row was full, so we skip copying it (effectively clearing it)
            }
        }

        // Clear all remaining rows above the last written row
        for (int y = writeRow; y >= 0; y--)
        {
            for (int x = 0; x < Width; x++)
            {
                Grid[y, x] = 0;
            }
        }

        return linesCleared;
    }
}