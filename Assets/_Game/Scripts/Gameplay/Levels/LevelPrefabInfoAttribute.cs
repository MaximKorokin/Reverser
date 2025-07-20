using System;

public class LevelPrefabInfoAttribute : Attribute
{
    public readonly string Name;

    public LevelPrefabInfoAttribute(string name)
    {
        Name = name;
    }
}
