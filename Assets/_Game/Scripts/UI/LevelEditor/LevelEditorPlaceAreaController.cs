using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public class LevelEditorPlaceAreaController : LevelEditorSelectableAreaController
{
    [SerializeField]
    private PointerEventsHandler _pointerEventsHandler;
    [SerializeField]
    private LevelEditorContextButtons _levelEditorContextButtons;
    [SerializeField]
    private RectTransform _prompt;

    private SnapPlacer SnapPlacer => GetLazy(() => new SnapPlacer(1, RectTransform));

    private readonly Dictionary<SelectableGameObjectWrapper, LevelObject> _levelObjects = new();

    private SelectableGameObjectWrapper _selectedPlaceGameObjectWrapper;
    private GameObject _placePrefab;
    private Vector2 _currentOffset;
    private SelectableBehaviourMode _selectableBehaviourMode;

    private LevelPrefabsManager _prefabsManager;

    [Inject]
    private void Construct(LevelPrefabsManager prefabsManager)
    {
        _prefabsManager = prefabsManager;
    }

    protected override void Awake()
    {
        base.Awake();

        _pointerEventsHandler.PointerDown += OnPointerDown;
        _pointerEventsHandler.PointerDrag += OnPointerDrag;
        _pointerEventsHandler.PointerUp += OnPointerClickUp;

        _levelEditorContextButtons.RemoveButtonClicked += OnRemoveContextButtonClicked;
        _levelEditorContextButtons.MoveButtonClicked += OnMoveContextButtonClicked;
        _levelEditorContextButtons.BindButtonClicked += OnBindContextButtonClicked;
        _levelEditorContextButtons.UnbindButtonClicked += OnUnbindContextButtonClicked;

        _levelEditorContextButtons.OnHidden += OnContextButtonsHidden;

        LevelObjectsSelectableGroup.SelectedChanged += OnPlaceLevelObjectsSelectedChanged;

        _selectableBehaviourMode = SelectableBehaviourMode.Select;
    }

    protected SelectableGameObjectWrapper CreateSelectableGameObjectWrapper(GameObject toWrap, string id, Vector2 worldPosition)
    {
        var wrapper = base.CreateSelectableGameObjectWrapper(toWrap);
        _levelObjects.Add(wrapper, new() { Name = _prefabsManager.ToLevelPrefabName(toWrap), Id = id });
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
        switch (_selectableBehaviourMode)
        {
            case SelectableBehaviourMode.Select:
                if (_placePrefab != null)
                {
                    CreateSelectableGameObjectWrapper(_placePrefab, Guid.NewGuid().ToString(), eventData.pointerCurrentRaycast.worldPosition);
                }
                break;
            case SelectableBehaviourMode.Move:
                PlaceSnapped(_selectedPlaceGameObjectWrapper.RectTransform, eventData.pointerCurrentRaycast.worldPosition);
                _selectedPlaceGameObjectWrapper.SetSelection(false);
                _selectableBehaviourMode = SelectableBehaviourMode.Select;
                break;
        }
    }

    private void OnRemoveContextButtonClicked()
    {
        LevelObjectsSelectableGroup.RemoveSelectable(_selectedPlaceGameObjectWrapper);
        Destroy(_selectedPlaceGameObjectWrapper.gameObject);
    }

    private void OnMoveContextButtonClicked()
    {
        _selectableBehaviourMode = SelectableBehaviourMode.Move;
    }

    private void OnBindContextButtonClicked()
    {
        _selectableBehaviourMode = SelectableBehaviourMode.Bind;
    }

    private void OnUnbindContextButtonClicked()
    {
        _selectableBehaviourMode = SelectableBehaviourMode.Bind;
    }

    private void OnContextButtonsHidden()
    {
        if (_selectableBehaviourMode == SelectableBehaviourMode.Select)
        {
            _selectedPlaceGameObjectWrapper.SetSelection(false);
        }
    }

    private void OnPlaceLevelObjectsSelectedChanged(SelectableGameObjectWrapper selectable)
    {
        switch (_selectableBehaviourMode)
        {
            case SelectableBehaviourMode.Select:
                _selectedPlaceGameObjectWrapper = selectable;
                var selectedGameObject = selectable == null ? null : selectable.WrappedGameObject;
                if (selectedGameObject != null)
                {
                    _levelEditorContextButtons.Show(selectedGameObject);
                }
                break;
            case SelectableBehaviourMode.Move:
                _selectableBehaviourMode = SelectableBehaviourMode.Select;
                if (selectable == null || _selectedPlaceGameObjectWrapper == null) return;

                var selectablePosition = selectable.RectTransform.position;
                PlaceSnapped(selectable.RectTransform, _selectedPlaceGameObjectWrapper.RectTransform.position);
                PlaceSnapped(_selectedPlaceGameObjectWrapper.RectTransform, selectablePosition);
                _selectedPlaceGameObjectWrapper.SetSelection(false);
                selectable.SetSelection(false);
                break;
            case SelectableBehaviourMode.Bind:
                _selectableBehaviourMode = SelectableBehaviourMode.Select;
                if (selectable == null || _selectedPlaceGameObjectWrapper == null) return;
                if (_selectedPlaceGameObjectWrapper.WrappedGameObject.GetComponent<IStateBindable>() == null || selectable.WrappedGameObject.GetComponent<IStateful>() == null)
                {
                    Logger.Warn($"Cannot bind {_selectedPlaceGameObjectWrapper.WrappedGameObject} to {selectable.WrappedGameObject}");
                    return;
                }

                var levelObjectToBind = _levelObjects[selectable];
                var bindableLevelObject = _levelObjects[_selectedPlaceGameObjectWrapper];
                bindableLevelObject.Bindings ??= new();
                bindableLevelObject.Bindings.Add(levelObjectToBind.Id);
                break;
        }
    }

    public void SetPlacePrefab(GameObject placePrefab)
    {
        _placePrefab = placePrefab;
    }

    public IEnumerable<LevelObject> GetLevelObjects()
    {
        var worldOffset = _currentOffset / Camera.main.GetUnitScreenSize(Canvas.scaleFactor);
        return LevelObjectsSelectableGroup.SelectableElements.Select(wrapper =>
        {
            var levelObject = _levelObjects[wrapper];
            levelObject.Position = (Vector2)wrapper.transform.position - worldOffset;
            return levelObject;
        });
    }

    public void SetPositionedPrefabs(IEnumerable<(GameObject, string, Vector2)> positionedPrefabs)
    {
        foreach (var (prefab, id, position) in positionedPrefabs)
        {
            CreateSelectableGameObjectWrapper(prefab, id, position);
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

    private enum SelectableBehaviourMode
    {
        None = 0,
        Select = 10,
        Move = 20,
        Bind = 30,
    }
}
