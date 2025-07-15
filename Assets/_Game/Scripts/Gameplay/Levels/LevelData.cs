using System;
using UnityEngine;

[Serializable]
public class LevelData
{
    [field: SerializeField]
    public float LevelHalfDuration {  get; set; }
    [field: SerializeField]
    public Vector2[] LevelGroundPositions {  get; set; }
    [field: SerializeField]
    public Vector2 LevelEndDoorPosition {  get; set; }
    [field: SerializeField]
    public Vector2 Character1Position {  get; set; }
    [field: SerializeField]
    public Vector2 Character2Position {  get; set; }
    [field: SerializeField]
    public Vector2[] PickableBoxesPositions {  get; set; }
}
