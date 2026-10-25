using System;

public static class TimeEvents
{
    public static Action<GameTime> OnTimeChanged;
    public static Action<float> OnTimeMultiplierChanged;
}