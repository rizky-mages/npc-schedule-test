using System;

public interface ITimeProvider
{
    event Action<GameTime> OnTimeChanged;
    GameTime CurrentTime { get; }
}