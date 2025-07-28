using System;

public class LevelSharedContext
{
    public LevelData LevelData { get; set; }

    public TimeCounter LevelTimeCounter { get; } = new(0);

    public event Action LevelCompleted;

    public void InvokeLevelCompleted()
    {
        LevelCompleted?.Invoke();
    }
}
