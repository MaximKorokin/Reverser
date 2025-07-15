using System;
using System.Collections.Generic;

public class SelectableElementsGroup<T> where T : SelectableElement
{
    private readonly HashSet<T> _selectableElements = new();

    public event Action<T> SelectedChanged;

    public void AddSelectable(T selectableElement)
    {
        _selectableElements.Add(selectableElement);
        selectableElement.SelectionChanged += (e, s) => OnSelectableElementSelectionChanged((T)e, s);
    }

    private void OnSelectableElementSelectionChanged(T selectableElement, bool selected)
    {
        if (selected)
        {
            _selectableElements.ForEach(x => x.SetSelection(false));
            SelectedChanged?.Invoke(selectableElement);
        }
    }
}
