# Cross-Platform Tetris: A Study in Clean Architecture

A C# and .NET implementation of Tetris designed to demonstrate clean software architecture. This project features a platform-agnostic core game engine that seamlessly powers both a terminal-based (CLI) version and a desktop GUI version (Avalonia) without duplicating business logic.

## 🏗️ Architecture & Design Principles

The primary goal of this project is to enforce **Separation of Concerns (SoC)** and the **Dependency Inversion Principle (DIP)**. The core game logic is completely isolated from UI frameworks and input mechanisms.

### Project Structure
```text
Tetris/
├── src/
│   ├── Tetris.Core/          # 🔵 Pure game logic & Abstractions
│   │   ├── Game/             # TetrisGame, Board, Tetromino
│   │   └── Interfaces/       # IInputHandler, IRenderer, IGameLoop
│   │
│   ├── Tetris.CLI/           # 🖥️ Console Implementation
│   │   ├── Input/            # ConsoleInputHandler
│   │   ├── Rendering/        # ConsoleRenderer
│   │   └── GameLoop/         # CliGameLoop
│   │
│   └── Tetris.Avalonia/      # 🎨 Desktop GUI Implementation
│       ├── Input/            # AvaloniaInputHandler
│       ├── Rendering/        # AvaloniaRenderer
│       ├── ViewModels/       # MainViewModel (MVVM)
│       └── Views/            # XAML UI
│
└── tests/
    └── Tetris.Core.Tests/    # ✅ Unit Tests (xUnit)
```

### 1. Dependency Inversion & Interfaces
To prevent the core logic from being tightly coupled to specific platforms, `Tetris.Core` defines strict contracts (interfaces) rather than concrete implementations. 

*   **`IInputHandler`**: Abstracts user input. The core game does not know if input comes from a keyboard, a gamepad, or an automated test script.
*   **`IRenderer`**: Abstracts the drawing process. The core game dictates *what* state to draw, but not *how* it is rendered (ASCII characters vs. XAML Canvas shapes).
*   **`IGameLoop`**: Abstracts the timing mechanism. The CLI utilizes a blocking `while` loop with `Thread.Sleep`, while Avalonia utilizes an asynchronous `DispatcherTimer`. 

**Example of Decoupling:**
```csharp
// TetrisGame.cs (Core) - Has zero dependencies on UI or Console libraries
public void Update(IInputHandler input, double deltaTime) 
{
    if (input.HardDrop) { /* Execute drop logic */ }
}
```

### 2. The Composition Root
Because the core logic relies entirely on abstractions, concrete implementations must be injected at the application's entry point. This is handled in the **Composition Root** (`Program.cs` for CLI, `MainWindow.axaml.cs` for Avalonia). This pattern ensures `Tetris.Core` remains a pure, dependency-free class library.

### 3. MVVM Pattern (Avalonia UI)
The GUI implementation strictly follows the **Model-View-ViewModel (MVVM)** pattern:
*   **Model:** The `TetrisGame` and `Board` classes in `Tetris.Core`.
*   **ViewModel:** `MainViewModel` acts as the intermediary. It subscribes to the `IGameLoop`, updates the model, and exposes observable properties (e.g., `Score`, `IsGameOver`) for XAML data binding.
*   **View:** The `.axaml` files handle purely visual layout and styling, remaining completely unaware of the underlying game logic.

## 🧪 Testing Strategy

A major advantage of decoupling the core logic via interfaces is the ability to perform isolated unit testing without requiring a UI or physical hardware.

### Mocking Dependencies
The test project (`Tetris.Core.Tests`) utilizes a `FakeInputHandler` that implements `IInputHandler`. This allows the tests to programmatically simulate keyboard inputs.

```csharp
// Example: Testing Hard Drop logic without a real keyboard
[Fact]
public void HardDrop_ShouldLockPieceAtBottom()
{
    var game = new TetrisGame();
    var input = new FakeInputHandler { HardDrop = true }; // Simulate input

    game.Update(input, 0.016); // Act

    Assert.True(IsPieceLockedAtBottom(game)); // Assert
}
```

### Test Patterns
All tests follow the industry-standard **Arrange, Act, Assert (AAA)** pattern to ensure readability and maintainability. The test suite covers core mechanics such as piece movement, collision detection, line clearing, and game-over states.

## ⏱️ Technical Implementation Details

### Frame-Rate Independence (Delta Time)
To ensure the game runs at the same speed on a 60Hz monitor and a 144Hz monitor, the game loop calculates the time elapsed between frames (`deltaTime`). 

The `TetrisGame` utilizes an **accumulator pattern** for gravity. Instead of moving pieces a fixed distance per frame, it accumulates `deltaTime` until it exceeds the calculated drop interval, ensuring physics are tied to real-world time rather than frame rate.

### Input Handling & OS Key Repeat
GUI frameworks fire continuous `KeyDown` events when a key is held (OS key repeat). To prevent this from breaking the game loop (e.g., triggering multiple hard drops in a single frame), the input handlers separate state into **"Key Pressed"** (one-shot actions like Rotate) and **"Key Held"** (continuous actions like Soft Drop).

## 🚀 Building and Running

### Prerequisites
*   [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Running the CLI Version
```bash
dotnet run --project src/Tetris.CLI
```

### Running the Avalonia (GUI) Version
```bash
dotnet run --project src/Tetris.Avalonia
```

### Running Unit Tests
```bash
dotnet test
```

## 🎮 Controls

| Action | Key |
| :--- | :--- |
| **Move Left/Right** | `←` / `→` |
| **Rotate** | `↑` |
| **Soft Drop** | `↓` (Held) |
| **Hard Drop** | `Spacebar` |