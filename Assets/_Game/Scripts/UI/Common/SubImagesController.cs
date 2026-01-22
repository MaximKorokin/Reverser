using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SubImagesController : UIBehaviourBase
{
    [SerializeField]
    private RectTransform _subImagesParent;

    /// <summary>
    /// If <paramref name="sprite"/> is null, modifier image will be disabled
    /// </summary>
    /// <param name="siblingIndex"></param>
    /// <param name="sprite"></param>
    /// <param name="color"></param>
    public void Set(int siblingIndex, Sprite sprite, Color color)
    {
        if (_subImagesParent == null)
        {
            Logger.Error($"{_subImagesParent} is not set");
            return;
        }

        if (siblingIndex < 0 || siblingIndex > _subImagesParent.childCount)
        {
            Logger.Error($"Parameter {siblingIndex} is out of allowed range");
            return;
        }

        var childImage = _subImagesParent.GetChild(siblingIndex).GetComponent<Image>();
        childImage.enabled = sprite != null;
        childImage.sprite = sprite;
        childImage.color = color;
    }

    public void Clear()
    {
        foreach (var childImage in transform.Cast<Image>().Where(x => x != null))
        {
            childImage.enabled = false;
            childImage.sprite = null;
        }
    }
}
