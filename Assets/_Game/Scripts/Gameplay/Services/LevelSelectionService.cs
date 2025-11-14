using System;

public class LevelSelectionService : ServiceBase
{
    public event Action<LevelData> LevelSelected;

    public void SelectLevel(LevelData levelData)
    {
        LevelSelected?.Invoke(levelData);
    }
}