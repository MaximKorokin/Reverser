using System;
using UnityEngine.EventSystems;

public class PointerEventsHandler : MonoBehaviourBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public event Action<PointerEventData> PointerDrag;
    public event Action<PointerEventData> PointerDown;
    /// <summary>
    /// Pointer up with no drag
    /// </summary>
    public event Action<PointerEventData> PointerUp;
    public event Action<PointerEventData> PointerDragUp;

    public void OnDrag(PointerEventData eventData)
    {
        PointerDrag?.Invoke(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDown?.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            PointerDragUp?.Invoke(eventData);
        }
        else
        {
            PointerUp?.Invoke(eventData);
        }
    }
}
