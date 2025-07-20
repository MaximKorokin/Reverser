using System;

public class LevelDataInfoAttribute : Attribute
{
    public readonly string DisplayName;
    public readonly string DefaultValue;

    public LevelDataInfoAttribute(string displayName, string defaultValue)
    {
        DisplayName = displayName;
        DefaultValue = defaultValue;
    }
}
