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
    private RectTransform _prompt;

    private SnapPlacer SnapPlacer => GetLazy(() => new SnapPlacer(1, RectTransform));

    private SelectableGameObjectWrapper _selectedPlaceGameObjectWrapper;
    private GameObject _placePrefab;
    private Vector2 _currentOffset;

    protected override void Awake()
    {
        base.Awake();

        _pointerEventsHandler.PointerDown += OnPointerDown;
        _pointerEventsHandler.PointerDrag += OnPointerDrag;
        _pointerEventsHandler.PointerUp += OnPointerClickUp;
        _levelEditorContextButtons.RemoveButtonClicked += OnRemoveContextButtonClicked;
        _levelEditorContextButtons.OnHidden += OnContextButtonsHidden;

        LevelObjectsSelectableGroup.SelectedChanged += OnPlaceLevelObjectsSelectedChanged;
    }

    protected SelectableGameObjectWrapper CreateSelectableGameObjectWrapper(GameObject toWrap, Vector2 worldPosition)
    {
        var wrapper = CreateSelectableGameObjectWrapper(toWrap);
        PlaceSnapped(wrapper.RectTransform, worldPosition);
        return wrapper;
    }

    private void PlaceSnapped(RectTransform rectTransform, Vector2 worldPosition)
    {
        var worldOffset = _currentOffset / Camera.main.GetUnitScreenSize(Canvas.scaleFactor);
        SnapPlacer.Place(rectTransform, worldPosition, worldOffset, Canvas.scaleFactor);
    }

    private void OnPointerDown(PointerEventData eventData)
    {
        if (_placePrefab != null)
        {
            _prompt.gameObject.SetActive(true);
            PlaceSnapped(_prompt, eventData.pointerCurrentRaycast.worldPosition);
        }
    }

    private void OnPointerDrag(PointerEventData eventData)
    {
        _prompt.gameObject.SetActive(false);
        _currentOffset += eventData.delta;
        LevelObjectsSelectableGroup.SelectableElements.ForEach(x => x.RectTransform.anchoredPosition += eventData.delta);
    }

    private void OnPointerClickUp(PointerEventData eventData)
    {
        _prompt.gameObject.SetActive(false);
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

    private void OnContextButtonsHidden()
    {
        _selectedPlaceGameObjectWrapper.SetSelection(false);
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
        var worldOffset = _currentOffset / Camera.main.GetUnitScreenSize(Canvas.scaleFactor);
        return LevelObjectsSelectableGroup.SelectableElements.Select(
            x => (x.WrappedGameObject, (Vector2)x.transform.position - worldOffset));
    }

    public void SetPositionedPrefabs(IEnumerable<(GameObject, Vector2)> positionedPrefabs)
    {
        foreach (var (prefab, position) in positionedPrefabs)
        {
            CreateSelectableGameObjectWrapper(prefab, position);
        }
    }

    public void ResetState()
    {
        _currentOffset = Vector2.zero;
        LevelObjectsSelectableGroup.SelectableElements
            .ToArray()
            .ForEach(selectable =>
            {
                LevelObjectsSelectableGroup.RemoveSelectable(selectable);
                Destroy(selectable.gameObject);
            });
    }
}
