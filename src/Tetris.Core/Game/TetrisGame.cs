namespace Tetris.Core.Game;

public class TetrisGame
{
    public int Score { get; private set; }
    public int LinesCleared { get; private set;}
    public int Level { get; private set; } = 1;
    public bool IsGameOver { get; private set; }

    public Tetromino NextPiece { get; private set; }
    public Tetromino CurrentPiece;

    public Board Board = new();
    public Random _random = new();

    private double _gravityAccumulator = 0;
    private double GetDropInterval()
    {
        // Official modern tetris formula: Seconds Per Row = (0.8 - [(Level-1) × 0.007])^(Level-1)
        const double BaseSpeed = 0.8;
        const double DecayPerLevel = 0.007;
        const double MinInterval = 0.031; // Max speed (reached at Level 15)

        // Clamp level to prevent negative/undefined math
        int clampedLevel = Math.Max(1, Level);
        
        // Calculate the formula components
        double inner = BaseSpeed - ((clampedLevel - 1) * DecayPerLevel);
        double exponent = clampedLevel - 1;
        
        // Apply formula: inner^exponent
        double result = Math.Pow(inner, exponent);
        
        // Cap minimum to keep it playable in console + prevent accumulator issues
        return Math.Max(MinInterval, result);
    }

    public TetrisGame()
    {
        CurrentPiece = GenerateRandomPiece();

        CurrentPiece.X = Board.Width / 2;
        CurrentPiece.Y = 0;

        NextPiece = GenerateRandomPiece();
    }
    public void Update(IInputHandler input, double deltaTime)
    {
        if (IsGameOver) return; //Stops processing if game ended

        if (input.HardDrop)
        {
            while (Board.IsValidPosition(CurrentPiece.Shape, CurrentPiece.X, CurrentPiece.Y + 1))
            {
                CurrentPiece.Y++;
            }
            Board.LockPiece(CurrentPiece);
            int clearedLines = Board.ClearFullLines();
            if (clearedLines > 0) UpdateProgression(clearedLines);
            SpawnNewPiece();

            _gravityAccumulator = 0; //Reset gravity to prevent instant drop after hard drop
            return; //Early return since we already handled the drop and spawn logic
        }

        //Left and right movement
        if (input.Left && Board.IsValidPosition(CurrentPiece.Shape, CurrentPiece.X - 1, CurrentPiece.Y))
            CurrentPiece.X--;
        if (input.Right && Board.IsValidPosition(CurrentPiece.Shape, CurrentPiece.X + 1, CurrentPiece.Y))
            CurrentPiece.X++;
        
        //Rotation
        if (input.Rotate)
        {
            var rotated = TetrominoShapes.RotateClockwise(CurrentPiece.Shape);

            if (Board.IsValidPosition(rotated, CurrentPiece.X, CurrentPiece.Y))
            {
                CurrentPiece.Shape = rotated;
            }
        }
        
        //Gravity and soft drop
        _gravityAccumulator += deltaTime;
        double effectiveInterval = input.SoftDrop ? 0.05 : GetDropInterval();
        if (_gravityAccumulator >= effectiveInterval)
        {
            if (Board.IsValidPosition(CurrentPiece.Shape, CurrentPiece.X, CurrentPiece.Y + 1))
            {
                CurrentPiece.Y++;
            }
            else
            {
                Board.LockPiece(CurrentPiece);
                int clearedLines = Board.ClearFullLines();
                if (clearedLines > 0) UpdateProgression(clearedLines);
                SpawnNewPiece();
            }
            _gravityAccumulator -= effectiveInterval; //Reset accumulator but keep overflow for consistent timing
        }
    }

    public void UpdateProgression(int lines)
    {
        Score += lines switch
        {
            1 => 100 * Level,
            2 => 300 * Level,
            3 => 500 * Level,
            4 => 800 * Level,
            _ => 0
        };

        LinesCleared += lines;

        Level = (LinesCleared / 10) + 1; //Increase level every 10 lines
    }
    
    public void SpawnNewPiece()
    {
        CurrentPiece = NextPiece;
        NextPiece = GenerateRandomPiece();

        CurrentPiece.X = Board.Width / 2;
        CurrentPiece.Y = 0;

        if (!Board.IsValidPosition(CurrentPiece.Shape, CurrentPiece.X, CurrentPiece.Y))
        {
            IsGameOver = true;
        }
    }
    private Tetromino GenerateRandomPiece()
    {
        int randomShape = _random.Next(0, 6);
        TetrominoType type = (TetrominoType)randomShape;

        return new Tetromino
        {
            X = 0,
            Y = 0,
            Shape = TetrominoShapes.GetShape(type)
        };
    }
}