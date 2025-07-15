using UnityEngine;
using UnityEngine.UI;

public class SizeToggle : MonoBehaviourBase
{
    [SerializeField]
    private Orientation _orientation;
    [SerializeField]
    private Sprite _showSprite;
    [SerializeField]
    private Sprite _hideSprite;
    [SerializeField]
    private RectTransform _target;

    private Image _image;
    private Vector2 _initailSize;
    private bool _isHidden;

    protected override void Awake()
    {
        base.Awake();

        _image = GetRequiredComponent<Image>();
        _initailSize = _target.sizeDelta;
    }

    public void Show()
    {
        _isHidden = false;
        _image.sprite = _hideSprite;
        _target.sizeDelta = new(_initailSize.x, _initailSize.y);

    }

    public void Hide()
    {
        _isHidden = true;
        _image.sprite = _showSprite;
        if (_orientation == Orientation.Vertical) _target.sizeDelta = new(0, _initailSize.y);
        else if (_orientation == Orientation.Horizontal) _target.sizeDelta = new(_initailSize.x, 0);
    }

    public void Toggle()
    {
        if (_isHidden) Show();
        else Hide();
    }

    private enum Orientation
    {
        Vertical,
        Horizontal,
    }
}
