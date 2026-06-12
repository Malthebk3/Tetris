namespace Tetris.Core.Game;

public interface IGameLoop
{
    void Start();
    void Stop();
    event Action<double> OnUpdate;
}