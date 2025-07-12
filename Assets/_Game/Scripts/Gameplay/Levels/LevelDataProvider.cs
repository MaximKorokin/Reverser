using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataProvider", menuName = "Scriptable Objects/LevelDataProvider")]
public class LevelDataProvider : ScriptableObject
{
    [SerializeField]
    private LevelData[] _levels;

    public IEnumerable<LevelData> Levels => _levels;

    public LevelData GetLevelData(int index)
    {
        return _levels[index];
    }
}
