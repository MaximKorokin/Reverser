using System;
using UnityEngine.EventSystems;

public class SelectableElement : MonoBehaviourBase, IPointerDownHandler
{
    private bool _isSelected;

    public event Action<SelectableElement, bool> SelectionChanged;

    public void OnPointerDown(PointerEventData eventData)
    {
        SetSelection(!_isSelected);
    }

    public virtual void SetSelection(bool selection, bool silent = false)
    {
        var previousValue = _isSelected;
        _isSelected = selection;
        if (!silent && previousValue != _isSelected)
        {
            SelectionChanged?.Invoke(this, _isSelected);
        }
    }
}
