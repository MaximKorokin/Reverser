using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public class LevelEditorPlaceAreaController : LevelEditorSelectableAreaController
{
    private const int BindSubImageIndex = 0;
    private const int ToggleSubImageIndex = 2;

    [SerializeField]
    private LevelEditorPlaceAreaSettings _settings;
    [SerializeField]
    private PointerEventsHandler _pointerEventsHandler;
    [SerializeField]
    private LevelEditorContextButtons _levelEditorContextButtons;
    [SerializeField]
    private RectTransform _prompt;

    private SnapPlacer SnapPlacer => GetLazy(() => new SnapPlacer(1, RectTransform));

    private readonly Dictionary<SelectableGameObjectWrapper, (LevelObject LevelObject, SubImagesController SubImagesController)> _levelObjects = new();

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
        _levelEditorContextButtons.ToggleButtonClicked += OnToggleContextButtonClicked;

        _levelEditorContextButtons.OnHidden += OnContextButtonsHidden;

        LevelObjectsSelectableGroup.SelectedChanged += OnPlaceLevelObjectsSelectedChanged;

        _selectableBehaviourMode = SelectableBehaviourMode.Select;
    }

    protected SelectableGameObjectWrapper CreateSelectableGameObjectWrapper(LevelObject levelObject)
    {
        var wrapper = base.CreateSelectableGameObjectWrapper(_prefabsManager.ToLevelPrefab(levelObject.Name));
        _levelObjects.Add(wrapper, (levelObject, wrapper.GetRequiredComponent<SubImagesController>()));
        PlaceSnapped(wrapper.RectTransform, levelObject.Position);
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
                    var levelObject = new LevelObject() { Name = _prefabsManager.ToLevelPrefabName(_placePrefab), Id = Guid.NewGuid().ToString(), Position = eventData.pointerCurrentRaycast.worldPosition };
                    CreateSelectableGameObjectWrapper(levelObject);
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
        var id = _levelObjects[_selectedPlaceGameObjectWrapper].LevelObject.Id;
        if (_selectedPlaceGameObjectWrapper != null && 
            (_levelObjects[_selectedPlaceGameObjectWrapper].LevelObject.Bindings?.Count > 0 || _levelObjects.Any(x => x.Value.LevelObject.Bindings.Contains(id))))
        {
            UnbindSelectable(_selectedPlaceGameObjectWrapper);
            _selectableBehaviourMode = SelectableBehaviourMode.Select;
        }
        else
        {
            _selectableBehaviourMode = SelectableBehaviourMode.Bind;
        }
    }

    private void OnToggleContextButtonClicked()
    {
        if (_selectedPlaceGameObjectWrapper == null) return;
        var (levelObject, subImagesController) = _levelObjects[_selectedPlaceGameObjectWrapper];
        levelObject.ToggleBind = !levelObject.ToggleBind;

        subImagesController.Set(ToggleSubImageIndex, levelObject.ToggleBind ? _settings.SelectableSubImageToggleOnSprite : _settings.SelectableSubImageToggleOffSprite, Color.gray);
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
                BindSelectables(selectable, _selectedPlaceGameObjectWrapper);
                _selectedPlaceGameObjectWrapper.SetSelection(false);
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
            var levelObject = _levelObjects[wrapper].LevelObject;
            levelObject.Position = (Vector2)wrapper.transform.position - worldOffset;
            return levelObject;
        });
    }

    public void SetLevelData(LevelData levelData)
    {
        //levelData.LevelObjects.Select(x => (_prefabsManager.ToLevelPrefab(x.Name), x.Id, x.Position))
        foreach (var levelObject in levelData.LevelObjects)
        {
            CreateSelectableGameObjectWrapper(levelObject);
        }

        int bindsCount = 0;
        foreach (var (wrapper, (levelObject, subImagesController)) in _levelObjects)
        {
            if (levelObject.Bindings?.Count > 0)
            {
                subImagesController.Set(BindSubImageIndex, _settings.SelectableSubImageBindSprite, ColorUtils.GetUniversalIndexedColor(bindsCount));
                var binded = _levelObjects.First(x => x.Value.LevelObject.Id == levelObject.Bindings[0]).Value.SubImagesController;
                binded.Set(BindSubImageIndex, _settings.SelectableSubImageBindSprite, ColorUtils.GetUniversalIndexedColor(bindsCount));
                bindsCount++;
            }

            if (wrapper.WrappedGameObject.GetComponent<IStateBindable>() != null)
            {
                subImagesController.Set(ToggleSubImageIndex, levelObject.ToggleBind ? _settings.SelectableSubImageToggleOnSprite : _settings.SelectableSubImageToggleOffSprite, Color.gray);
            }
        }
    }

    public void ResetState()
    {
        _levelObjects.Clear();
        _currentOffset = Vector2.zero;
        LevelObjectsSelectableGroup.SelectableElements
            .ToArray()
            .ForEach(selectable =>
            {
                LevelObjectsSelectableGroup.RemoveSelectable(selectable);
                Destroy(selectable.gameObject);
            });
    }

    private void BindSelectables(SelectableGameObjectWrapper selectable1, SelectableGameObjectWrapper selectable2)
    {
        var toChoseFrom = new SelectableGameObjectWrapper[] { selectable1, selectable2 };
        var stateful = toChoseFrom.FirstOrDefault(x => x.WrappedGameObject.GetComponent<IStateful>() != null);
        var bindable = toChoseFrom.FirstOrDefault(x => x.WrappedGameObject.GetComponent<IStateBindable>() != null);

        if (stateful == null || bindable == null)
        {
            Logger.Warn($"Cannot bind {selectable2.WrappedGameObject} and {selectable1.WrappedGameObject}");
            return;
        }

        var statefulLevelObject = _levelObjects[stateful].LevelObject;
        var bindableLevelObject = _levelObjects[bindable].LevelObject;
        if (bindableLevelObject.Bindings != null && bindableLevelObject.Bindings.Contains(statefulLevelObject.Id)) bindableLevelObject.Bindings.Remove(statefulLevelObject.Id);
        else bindableLevelObject.Bindings = new() { statefulLevelObject.Id };

        var activeBindingsAmount = _levelObjects.Count(x => x.Value.LevelObject.Bindings?.Count > 0);

        _levelObjects[selectable1].SubImagesController.Set(BindSubImageIndex, _settings.SelectableSubImageBindSprite, ColorUtils.GetUniversalIndexedColor(activeBindingsAmount - 1));
        _levelObjects[selectable2].SubImagesController.Set(BindSubImageIndex, _settings.SelectableSubImageBindSprite, ColorUtils.GetUniversalIndexedColor(activeBindingsAmount - 1));
    }

    private void UnbindSelectable(SelectableGameObjectWrapper selectable)
    {
        SelectableGameObjectWrapper secondSelectable = null;

        if (selectable.WrappedGameObject.TryGetComponent<IStateBindable>(out var bindable)) 
        {
            var secondId = _levelObjects[selectable].LevelObject.Bindings[0];
            secondSelectable = _levelObjects.First(x => x.Value.LevelObject.Id == secondId).Key;
            _levelObjects[selectable].LevelObject.Bindings.Clear();
        }
        else if (selectable.WrappedGameObject.TryGetComponent<IStateful>(out var stateful))
        {
            var firstId = _levelObjects[selectable].LevelObject.Id;
            secondSelectable = _levelObjects.First(x => x.Value.LevelObject.Bindings.Contains(firstId)).Key;
            _levelObjects[secondSelectable].LevelObject.Bindings.Clear();
        }

        if (secondSelectable == null)
        {
            Logger.Warn($"Cannot unbind {selectable.WrappedGameObject}");
            return;
        }

        _levelObjects[selectable].SubImagesController.Set(BindSubImageIndex, null, default);
        _levelObjects[secondSelectable].SubImagesController.Set(BindSubImageIndex, null, default);
    }

    private enum SelectableBehaviourMode
    {
        None = 0,
        Select = 10,
        Move = 20,
        Bind = 30,
    }
}
