using UnityEngine;

[CreateAssetMenu(fileName = "LevelEditorPlaceAreaSettings", menuName = "Scriptable Objects/LevelEditorPlaceAreaSettings")]
public class LevelEditorPlaceAreaSettings : ScriptableObject
{
    [field: SerializeField]
    public Sprite SelectableSubImageBindSprite {  get; private set; }
    [field: SerializeField]
    public Sprite SelectableSubImageToggleOnSprite {  get; private set; }
    [field: SerializeField]
    public Sprite SelectableSubImageToggleOffSprite {  get; private set; }
}