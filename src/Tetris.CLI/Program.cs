using System.Diagnostics;
using Tetris.CLI.Rendering;
using Tetris.CLI.Input;
using Tetris.Core.Game;
using Tetris.CLI.GameLoop;

namespace Tetris.CLI;

public class Program
{
    public static void Main(string[] args)
    {
        Console.CursorVisible = false;
        Console.Clear();

        // 1. Initialize Core Game (Pure Logic - knows nothing about CLI)
        var game = new TetrisGame();

        // 2. Initialize CLI-specific implementations
        var input = new InputHandler();
        var renderer = new ConsoleRenderer();
        var loop = new CliGameLoop();

        // 3. Wire everything together using the IGameLoop event
        loop.OnUpdate += (deltaTime) => 
        {
            // The classic Input -> Logic -> Render pipeline
            input.Update();
            game.Update(input, deltaTime);
            renderer.Draw(game.Board, game.CurrentPiece, game.NextPiece, game.Score, game.Level);

            // Check for Game Over to stop the loop
            if (game.IsGameOver)
            {
                Console.WriteLine("\nGame Over! Press any key to exit...");
                loop.Stop();
            }
        };

        // 4. Start the game loop
        loop.Start();
        
        // Keep console open after game over
        if (game.IsGameOver) Console.ReadKey();
    }
}