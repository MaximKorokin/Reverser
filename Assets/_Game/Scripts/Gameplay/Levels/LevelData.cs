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
    public string Id;
    public string Name;
    public List<string> Bindings;

    [SerializeField]
    private Vector2 _position;
    public Vector2 Position
    {
        get => _position;
        // avoiding excessive float error (like 3.000000238418579f)
        set => _position = new Vector2((float)Math.Round(value.x, 2), (float)Math.Round(value.y, 2));
    }
}
