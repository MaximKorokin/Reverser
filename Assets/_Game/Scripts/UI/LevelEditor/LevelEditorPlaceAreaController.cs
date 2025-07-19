using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelEditorPlaceAreaController : LevelEditorSelectableAreaController
{
    [SerializeField]
    private PointerEventsHandler _pointerEventsHandler;
    [SerializeField]
    private LevelEditorContextButtons _levelEditorContextButtons;
    [SerializeField]
    private RectTransform _selectableLevelObjectsPlaceParent;

    private SnapPlacer _snapPlacer;
    private SelectableGameObjectWrapper _selectedPlaceGameObjectWrapper;
    private GameObject _placePrefab;

    protected override void Start()
    {
        base.Start();

        _snapPlacer = new(1, _selectableLevelObjectsPlaceParent);

        _pointerEventsHandler.PointerClickUp += OnPointerClickUp;
        _levelEditorContextButtons.RemoveButtonClicked += OnRemoveContextButtonClicked;

        LevelObjectsSelectableGroup.SelectedChanged += OnPlaceLevelObjectsSelectedChanged;
    }

    protected SelectableGameObjectWrapper CreateSelectableGameObjectWrapper(GameObject toWrap, RectTransform parent, Vector2 worldPosition)
    {
        var wrapper = CreateSelectableGameObjectWrapper(toWrap, parent);
        _snapPlacer.Place(wrapper.transform as RectTransform, worldPosition);
        return wrapper;
    }

    private void OnPointerClickUp(PointerEventData eventData)
    {
        if (_placePrefab != null)
        {
            CreateSelectableGameObjectWrapper(_placePrefab, _selectableLevelObjectsPlaceParent, eventData.pointerCurrentRaycast.worldPosition);
        }
    }

    private void OnRemoveContextButtonClicked()
    {
        LevelObjectsSelectableGroup.RemoveSelectable(_selectedPlaceGameObjectWrapper);
        Destroy(_selectedPlaceGameObjectWrapper.gameObject);
    }

    private void OnPlaceLevelObjectsSelectedChanged(SelectableGameObjectWrapper selectable)
    {
        _selectedPlaceGameObjectWrapper = selectable;
        var selectedGameObject = selectable == null ? null : selectable.WrappedGameObject;
        if (selectedGameObject != null)
        {
            _levelEditorContextButtons.Show();
        }
    }

    public void SetPlacePrefab(GameObject placePrefab)
    {
        _placePrefab = placePrefab;
    }

    public IEnumerable<(GameObject, Vector2)> GetPositionedPrefabs()
    {
        return LevelObjectsSelectableGroup.SelectableElements.Select(x => (x.WrappedGameObject, (Vector2)x.transform.position));
    }

    public void SetPositionedPrefabs(IEnumerable<(GameObject, Vector2)> positionedPrefabs)
    {
        LevelObjectsSelectableGroup.SelectableElements
            .ToArray()
            .ForEach(x => Logger.Log(111))
            .ForEach(LevelObjectsSelectableGroup.RemoveSelectable)
            .ForEach(x => Logger.Log(222))
            .ForEach(x => Destroy(x.gameObject))
            .Count(); // Triggers linq

        foreach (var (prefab, position) in positionedPrefabs)
        {
            CreateSelectableGameObjectWrapper(prefab, _selectableLevelObjectsPlaceParent, position);
        }
    }
}
