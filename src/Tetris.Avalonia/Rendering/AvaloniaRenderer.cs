using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Tetris.Core.Game;

namespace Tetris.Avalonia.Rendering;

public class AvaloniaRenderer : IRenderer
{
    private readonly Canvas _nextPieceCanvas;
    private readonly Canvas _canvas;
    private const int CellSize = 30;
    private readonly SolidColorBrush _blockBrush = new(Color.Parse("#00FFFF"));
    private readonly SolidColorBrush _pieceBrush = new(Color.Parse("#FFFF00"));
    private readonly SolidColorBrush _gridBrush = new(Color.Parse("#333333")); // Grid color


    public AvaloniaRenderer(Canvas canvas, Canvas nextPieceCanvas)
    {
        _canvas = canvas;
        _nextPieceCanvas = nextPieceCanvas;
    }

    public void Draw(Board board, Tetromino currentPiece, Tetromino nextPiece, int score, int level)
    {
        _canvas.Children.Clear();
        _nextPieceCanvas.Children.Clear(); 

        DrawGrid(board);
        DrawBoard(board, currentPiece);
        DrawPiecePreview(nextPiece, 15, 15);
    }
    private void DrawGrid(Board board)
    {
        // Vertical lines
        for (int x = 0; x <= board.Width; x++)
        {
            _canvas.Children.Add(new Line
            {
                StartPoint = new Point(x * CellSize, 0),
                EndPoint = new Point(x * CellSize, board.Height * CellSize),
                Stroke = _gridBrush,
                StrokeThickness = 1
            });
        }

        // Horizontal lines
        for (int y = 0; y <= board.Height; y++)
        {
            _canvas.Children.Add(new Line
            {
                StartPoint = new Point(0, y * CellSize),
                EndPoint = new Point(board.Width * CellSize, y * CellSize),
                Stroke = _gridBrush,
                StrokeThickness = 1
            });
        }
    }

    private void DrawBoard(Board board, Tetromino piece)
    {
        for (int y = 0; y < board.Height; y++)
        {
            for (int x = 0; x < board.Width; x++)
            {
                bool isBlock = board.Grid[y, x] != 0;
                bool isPiece = IsPieceAt(piece, x, y);

                if (isBlock || isPiece)
                    DrawCell(x, y, isBlock ? _blockBrush : _pieceBrush);
            }
        }
    }

    private void DrawCell(int gridX, int gridY, SolidColorBrush brush)
    {
        var rect = new Rectangle
        {
            Width = CellSize - 1,
            Height = CellSize - 1,
            Fill = brush
        };
        Canvas.SetLeft(rect, gridX * CellSize);
        Canvas.SetTop(rect, gridY * CellSize);
        _canvas.Children.Add(rect);
    }

    private void DrawPiecePreview(Tetromino piece, int startX, int startY)
    {
        if (piece == null) return;

        for (int y = 0; y < piece.Shape.GetLength(0); y++)
        {
            for (int x = 0; x < piece.Shape.GetLength(1); x++)
            {
                if (piece.Shape[y, x] != 0) 
                {
                    var rect = new Rectangle
                    {
                        Width = CellSize - 1,
                        Height = CellSize - 1,
                        Fill = _pieceBrush
                    };

                    Canvas.SetLeft(rect, startX + x * CellSize);
                    Canvas.SetTop(rect, startY + y * CellSize);
                    _nextPieceCanvas.Children.Add(rect);
                }
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