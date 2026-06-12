using Tetris.Core.Game;

namespace Tetris.CLI.Rendering;

public class ConsoleRenderer : IRenderer
{
    private const int BoardOffsetX = 2; //Left margin for the board
    private const int BoardOffsetY = 2; //Top margin for the board
    private const int PreviewOffsetX = 25; //Side panel position
    private const int PreviewOffsetY = 3;
    public void Draw (Board board, Tetromino currentPiece, Tetromino nextPiece, int score, int level)
    {
        // Console.Clear();
        Console.SetCursorPosition(0, 0);
        
        DrawBoard(board, currentPiece);

        DrawSidePanel(nextPiece, score, level);
    }
    public void DrawBoard(Board board, Tetromino piece)
    {
        Console.SetCursorPosition(0, 0);
        
        for (int y = 0; y < board.Height; y++)
        {
            Console.SetCursorPosition(BoardOffsetX, BoardOffsetY + y);

            Console.Write("|"); //Left border

            for (int x = 0; x < board.Width; x++)
            {
                
                bool isBlock = board.Grid[y, x] != 0;
                bool isPiece = IsPieceAt(piece, x, y);

                Console.Write(isPiece || isBlock ? "██" : "  ");
            }

            Console.WriteLine("|"); //Right border
        }
        //Bottom border
        Console.SetCursorPosition(BoardOffsetX, BoardOffsetY + board.Height);
        Console.Write(new string('─', board.Width * 2 + 2));
    }
    private void DrawSidePanel (Tetromino nextPiece, int score, int level)
    {
        Console.SetCursorPosition(PreviewOffsetX, PreviewOffsetY);

        Console.Write("Next:");

        DrawPiecePreview(nextPiece, PreviewOffsetX, PreviewOffsetY + 2);

        int statsY = PreviewOffsetY + 6;
        Console.SetCursorPosition(PreviewOffsetX, statsY);
        Console.Write("Score: " + score);
        Console.SetCursorPosition(PreviewOffsetX, statsY + 1);
        Console.Write("Level: " + level);
    }
    private void DrawPiecePreview(Tetromino piece, int startX, int startY)
    {
        const int previewWidth = 4;
        const int previewHeight = 4;

        for (int y = 0; y < previewHeight; y++)
        {
            Console.SetCursorPosition(startX, startY + y);
            Console.Write(new string(' ', previewWidth * 2)); //Clear previous preview
        }

        for (int y = 0; y < piece.Shape.GetLength(0); y++)
        {
            for (int x = 0; x < piece.Shape.GetLength(1); x++)
            {
                Console.SetCursorPosition(startX + x * 2, startY + y);
                
                Console.Write(piece.Shape[y, x] != 0 ? "██" : "  ");
            }
        }
    }
    private bool IsPieceAt(Tetromino piece, int boardX, int boardY)
    {
        int localX = boardX - piece.X;
        int localY = boardY - piece.Y;

        if (localY < 0 || localY >= piece.Shape.GetLength(0)) return false;
        if (localX < 0 || localX >= piece.Shape.GetLength(1)) return false;

        return piece.Shape[localY, localX] != 0;
    }
}