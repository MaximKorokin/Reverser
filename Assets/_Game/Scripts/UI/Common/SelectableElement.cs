using System;
using UnityEngine.EventSystems;

public class SelectableElement : MonoBehaviourBase, IPointerDownHandler
{
    private bool _isSelected;

    public event Action<SelectableElement, bool> SelectionChanged;

    public void OnPointerDown(PointerEventData eventData)
    {
        SetSelection(!_isSelected, false);
    }

    public virtual void SetSelection(bool selection, bool silent)
    {
        var previousValue = _isSelected;
        _isSelected = selection;
        if (!silent && previousValue != _isSelected)
        {
            SelectionChanged?.Invoke(this, _isSelected);
        }
    }
}
