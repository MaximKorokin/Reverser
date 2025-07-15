using System;

public class LevelPrefabInfoAttribute : Attribute
{
    public readonly string Name;
    public readonly bool IsRequired;

    public LevelPrefabInfoAttribute(string name, bool isRequired)
    {
        Name = name;
        IsRequired = isRequired;
    }
}
