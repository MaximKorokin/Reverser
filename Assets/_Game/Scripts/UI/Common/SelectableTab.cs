using UnityEngine;
using UnityEngine.UI;

public class SelectableTab : SelectableElement
{
    [SerializeField]
    private Color _selectedColor = Color.white;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private GameObject[] _tabElements;

    private Color _initialColor;

    protected override void Awake()
    {
        base.Awake();

        _initialColor = _image.color;
    }

    public override void SetSelection(bool selection, bool silent)
    {
        base.SetSelection(selection, silent);
        _image.color = selection ? _selectedColor : _initialColor;
        _tabElements.ForEach(x => x.SetActive(selection));
    }
}
