using UnityEngine;
using UnityEngine.UI;

public class SelectableGameObjectWrapper : SelectableElement
{
    [SerializeField]
    private Image _wrappedObjectImage;
    [SerializeField]
    private Image _backgroundImage;

    public GameObject WrappedGameObject { get; private set; }

    public void SetGameObject(GameObject obj)
    {
        _backgroundImage.enabled = false;

        WrappedGameObject = obj;
        _wrappedObjectImage.sprite = WrappedGameObject.GetRequiredComponentOrInChildren<SpriteRenderer>().sprite;
    }

    public override void SetSelection(bool selection, bool silent = false)
    {
        base.SetSelection(selection, silent);
        _backgroundImage.enabled = selection;
    }
}
