using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    [LevelDataInfo("Level name", "name")]
    public string LevelName;
    [LevelDataInfo("Half duration", "15")]
    public float LevelHalfDuration;
    public List<LevelObject> LevelObjects;
}

[Serializable]
public class LevelObject
{
    public string Name;
    public Vector2 Position;
}
