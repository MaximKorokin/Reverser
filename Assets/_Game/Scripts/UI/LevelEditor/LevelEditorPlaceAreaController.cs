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

    private SnapPlacer SnapPlacer => GetLazy(() => new SnapPlacer(1, RectTransform));

    private SelectableGameObjectWrapper _selectedPlaceGameObjectWrapper;
    private GameObject _placePrefab;

    protected override void Awake()
    {
        base.Awake();

        _pointerEventsHandler.PointerClickUp += OnPointerClickUp;
        _levelEditorContextButtons.RemoveButtonClicked += OnRemoveContextButtonClicked;

        LevelObjectsSelectableGroup.SelectedChanged += OnPlaceLevelObjectsSelectedChanged;
    }

    protected SelectableGameObjectWrapper CreateSelectableGameObjectWrapper(GameObject toWrap, Vector2 worldPosition)
    {
        var wrapper = CreateSelectableGameObjectWrapper(toWrap);
        SnapPlacer.Place(wrapper.transform as RectTransform, worldPosition);
        return wrapper;
    }

    private void OnPointerClickUp(PointerEventData eventData)
    {
        if (_placePrefab != null)
        {
            CreateSelectableGameObjectWrapper(_placePrefab, eventData.pointerCurrentRaycast.worldPosition);
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
            .ForEach(selectable =>
            {
                LevelObjectsSelectableGroup.RemoveSelectable(selectable);
                Destroy(selectable.gameObject);
            });

        foreach (var (prefab, position) in positionedPrefabs)
        {
            CreateSelectableGameObjectWrapper(prefab, position);
        }
    }
}
