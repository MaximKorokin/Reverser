using UnityEngine;

public class LevelEditorSelectableAreaController : UIBehaviourBase
{
    [SerializeField]
    private SelectableGameObjectWrapper _selectableLevelObjectWrapperPrefab;
    protected readonly SelectableElementsGroup<SelectableGameObjectWrapper> LevelObjectsSelectableGroup = new();

    protected override void Awake()
    {
        base.Awake();

        Show();
    }

    protected SelectableGameObjectWrapper CreateSelectableGameObjectWrapper(GameObject toWrap, RectTransform parent)
    {
        var wrapper = Instantiate(_selectableLevelObjectWrapperPrefab);
        wrapper.transform.SetParent(parent, false);
        LevelObjectsSelectableGroup.AddSelectable(wrapper);
        wrapper.SetGameObject(toWrap);
        return wrapper;
    }
}
