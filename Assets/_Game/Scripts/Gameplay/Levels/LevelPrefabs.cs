using UnityEngine;

[CreateAssetMenu(fileName = "PrefabsList", menuName = "Scriptable Objects/PrefabsList")]
public class LevelPrefabs : ScriptableObject
{
    [field: SerializeField]
    public Transform LevelGround { get; private set; }
    [field: SerializeField]
    public Character Character1 { get; private set; }
    [field: SerializeField]
    public Character Character2 { get; private set; }
    [field: SerializeField]
    public Pickable PickableBox { get; private set; }
    [field: SerializeField]
    public LevelEndZone LevelEndZone { get; private set; }
}
