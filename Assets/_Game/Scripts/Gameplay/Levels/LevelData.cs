using System;
using UnityEngine;

[Serializable]
public class LevelData
{
    [field: SerializeField]
    public float LevelHalfDuration {  get; private set; }
    [field: SerializeField]
    public Vector2[] LevelGroundPositions {  get; private set; }
    [field: SerializeField]
    public Vector2 LevelEndDoorPosition {  get; private set; }
    [field: SerializeField]
    public Vector2 Character1Position {  get; private set; }
    [field: SerializeField]
    public Vector2 Character2Position {  get; private set; }
    [field: SerializeField]
    public Vector2[] PickableBoxesPositions {  get; private set; }
}
