using UnityEngine;

public static class ColorUtils
{
    private static Color[] _universalIndexedColors = new Color[]
    {
        Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta
    };

    public static Color GetUniversalIndexedColor(int index)
    {
        if (index < 0 || index > _universalIndexedColors.Length - 1)
        {
            Logger.Warn($"{nameof(index)} is outside of universal colors bounds");
            index %= _universalIndexedColors.Length;
        }

        return _universalIndexedColors[index];
    }
}
