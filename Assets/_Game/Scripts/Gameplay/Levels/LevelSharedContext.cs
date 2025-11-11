using System;

public class LevelSharedContext
{
    public LevelData LevelData { get; set; }

    public TimeCounter LevelTimeCounter { get; } = new(0);

    public event Action LevelCompleted;
    public event Action LevelFailed;

    public void InvokeLevelCompleted()
    {
        LevelCompleted?.Invoke();
    }

    public void InvokeLevelFailed()
    {
        LevelFailed?.Invoke();
    }
}
