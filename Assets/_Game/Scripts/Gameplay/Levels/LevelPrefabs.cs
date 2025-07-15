using UnityEngine;

[CreateAssetMenu(fileName = "PrefabsList", menuName = "Scriptable Objects/PrefabsList")]
public class LevelPrefabs : ScriptableObject
{
    [field: SerializeField]
    [LevelPrefabInfo("Character", true)]
    public Character Character1 { get; private set; }
    [field: SerializeField]
    [LevelPrefabInfo("CharacterReversed", true)]
    public Character Character2 { get; private set; }
    [field: SerializeField]
    [LevelPrefabInfo("Ground", false)]
    public Transform LevelGround { get; private set; }
    [field: SerializeField]
    [LevelPrefabInfo("PickableBox", false)]
    public Pickable PickableBox { get; private set; }
    [field: SerializeField]
    [LevelPrefabInfo("LevelEndZone", true)]
    public LevelEndZone LevelEndZone { get; private set; }
}
