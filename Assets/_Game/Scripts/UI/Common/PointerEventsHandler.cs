using System;
using UnityEngine.EventSystems;

public class PointerEventsHandler : MonoBehaviourBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool _isDragging;

    public event Action<PointerEventData> PointerDrag;
    public event Action<PointerEventData> PointerDown;
    public event Action<PointerEventData> PointerClickUp;
    public event Action<PointerEventData> PointerDragUp;

    public void OnDrag(PointerEventData eventData)
    {
        _isDragging = true;
        PointerDrag?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = false;
        PointerDown?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isDragging)
        {
            PointerDragUp?.Invoke(eventData);
        }
        else
        {
            PointerClickUp?.Invoke(eventData);
        }
    }
}
