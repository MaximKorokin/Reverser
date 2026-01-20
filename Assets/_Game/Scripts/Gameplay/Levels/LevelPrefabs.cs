using UnityEngine;

[CreateAssetMenu(fileName = "PrefabsList", menuName = "Scriptable Objects/PrefabsList")]
public class LevelPrefabs : ScriptableObject
{
    [field: SerializeField]
    [LevelPrefabInfo("Character")]
    public Character Character1 { get; private set; }
    [field: SerializeField]
    [LevelPrefabInfo("CharacterReversed")]
    public Character Character2 { get; private set; }
    [field: SerializeField]
    [LevelPrefabInfo("Ground")]
    public Transform LevelGround { get; private set; }
    [field: SerializeField]
    [LevelPrefabInfo("PickableBox")]
    public Pickable PickableBox { get; private set; }
    [field: SerializeField]
    [LevelPrefabInfo("PressableGroundButton")]
    public Pressable PressableGroundButton { get; private set; }
    [field: SerializeField]
    [LevelPrefabInfo("DisappearableWall")]
    public Disappearable DisappearableWall { get; private set; }
    [field: SerializeField]
    [LevelPrefabInfo("LevelEndZone")]
    public LevelEndZone LevelEndZone { get; private set; }
}
