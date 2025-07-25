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

    public void Place(RectTransform transform, Vector2 worldPosition, Vector2 worldOffset)
    {
        transform.SetParent(_parent, false);
        var snappedPosition = GetNearestSnappedPosition(worldPosition);
        var positionDelta = worldPosition - snappedPosition;
        var offset = new Vector2(worldOffset.x % _snappingStep, worldOffset.y % _snappingStep);
        offset -= new Vector2(
            Mathf.Abs(offset.x - positionDelta.x) < _snappingStep / 2 ? 0 : _snappingStep * Mathf.Sign(offset.x),
            Mathf.Abs(offset.y - positionDelta.y) < _snappingStep / 2 ? 0 : _snappingStep * Mathf.Sign(offset.y));
        transform.position = snappedPosition + offset;
        var sideSize = Camera.main.GetUnitScreenSize() * _snappingStep;
        transform.sizeDelta = new Vector2(sideSize, sideSize);
    }

    private Vector2 GetNearestSnappedPosition(Vector2 position)
    {
        var snapConverter = 1 / _snappingStep;
        var snappedX = Mathf.Round(position.x / _snappingStep) / snapConverter;
        var snappedY = Mathf.Round(position.y / _snappingStep) / snapConverter;
        return new Vector2(snappedX, snappedY);
    }
}
