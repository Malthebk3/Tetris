namespace Tetris.Core.Game;

public enum TetrominoType
{
    I,
    O,
    T,
    S,
    Z,
    J,
    L
}
public static class TetrominoShapes
{
    public static int[,] GetShape(TetrominoType type)
    {
        return type switch
        {
            TetrominoType.I => new int[,] { { 1, 1, 1, 1 } },
            TetrominoType.O => new int[,] { { 1, 1 }, { 1, 1 } },
            TetrominoType.T => new int[,]
            {
                { 0, 1, 0 },
                { 1, 1, 1 }
            },
            TetrominoType.S => new int[,]
            {
                { 0, 1, 1 },
                { 1, 1, 0 }
            },
            TetrominoType.Z => new int[,]
            {
                { 1, 1, 0 },
                { 0, 1, 1 }
            },
            TetrominoType.J => new int[,]
            {
                { 0, 1 },
                { 0, 1 },
                { 1, 1 }
            },
            TetrominoType.L => new int[,]
            {
                { 1, 0 },
                { 1, 0 },
                { 1, 1 }
            },
            _ => new int[,] { { 1, 1 }, { 1, 1 } }
        };
    }
    //Returns a new array rotated 90° clockwise.
    public static int[,] RotateClockwise(int[,] Shape)
    {
        int rows = Shape.GetLength(0);
        int cols = Shape.GetLength(1);

        int[,] rotated = new int[cols, rows];

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                rotated[x, rows - 1 - y] = Shape[y, x];   
            }
        }

        return rotated;
    }
}