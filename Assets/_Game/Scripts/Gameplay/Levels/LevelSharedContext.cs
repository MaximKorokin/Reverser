using System;

public class LevelSharedContext
{
    public LevelData LevelData { get; set; }

    public event Action LevelCompleted;

    public void InvokeLevelCompleted()
    {
        LevelCompleted?.Invoke();
    }
}
