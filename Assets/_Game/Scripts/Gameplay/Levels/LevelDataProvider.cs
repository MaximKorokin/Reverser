using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelDataProvider", menuName = "Scriptable Objects/LevelDataProvider")]
public class LevelDataProvider : ScriptableObject
{
    [SerializeField]
    private string[] _levels;

    public IEnumerable<LevelData> Levels => _levels.Select((_, i) => GetLevelData(i));

    public LevelData GetLevelData(int index)
    {
        return JsonUtility.FromJson<LevelData>(_levels[index]);
    }
}
