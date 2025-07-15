using UnityEngine;

public class SnapPlacer
{
    private readonly float _snappingStep;
    private readonly RectTransform _parent;

    public SnapPlacer(float snappingStep, RectTransform parent)
    {
        _snappingStep = snappingStep;
        _parent = parent;
    }

    public void Place(RectTransform transform, Vector2 worldPosition)
    {
        transform.SetParent(_parent, false);
        var snappedPosition = GetNearestSnappedPosition(worldPosition);
        transform.position = snappedPosition;
        var screenSize = (Camera.main.WorldToScreenPoint(Vector2.right * _snappingStep) - Camera.main.WorldToScreenPoint(Vector2.zero)).x;
        transform.sizeDelta = new Vector2(screenSize, screenSize);
    }

    private Vector2 GetNearestSnappedPosition(Vector2 position)
    {
        var snapConverter = 1 / _snappingStep;
        var snappedX = Mathf.Round(position.x / _snappingStep) / snapConverter;
        var snappedY = Mathf.Round(position.y / _snappingStep) / snapConverter;
        return new Vector2(snappedX, snappedY);
    }
}
