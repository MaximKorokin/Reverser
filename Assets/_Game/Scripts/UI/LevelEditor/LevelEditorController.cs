using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

public class LevelEditorController : MonoBehaviourBase
{
    [SerializeField]
    private PointerEventsHandler _pointerEventsHandler;
    [SerializeField]
    private LevelEditorContextButtons _levelEditorContextButtons;
    [SerializeField]
    private RectTransform _selectableLevelObjectsPoolParent;
    [SerializeField]
    private SelectableGameObjectWrapper _selectableLevelObjectWrapperPool;
    [SerializeField]
    private RectTransform _selectableLevelObjectsPlaceParent;
    [SerializeField]
    private SelectableGameObjectWrapper _selectableLevelObjectWrapperPlace;

    private LevelSharedContext _levelSharedContext;
    private LevelPrefabsManager _prefabsManager;
    private SnapPlacer _snapPlacer;
    private GameObject _selectedPoolGameObject;
    private SelectableGameObjectWrapper _selectedPlaceGameObjectWrapper;

    private readonly SelectableElementsGroup<SelectableGameObjectWrapper> _levelObjectsSelectablePoolGroup = new();
    private readonly SelectableElementsGroup<SelectableGameObjectWrapper> _levelObjectsSelectablePlaceGroup = new();

    public event Action LoadLevelRequested;
    public event Action CloseEditorRequested;

    [Inject]
    private void Construct(
        LevelSharedContext levelSharedContext,
        LevelPrefabsManager prefabsManager)
    {
        gameObject.SetActive(false);

        _levelSharedContext = levelSharedContext;
        _prefabsManager = prefabsManager;
        _snapPlacer = new(1, _selectableLevelObjectsPlaceParent);

        _levelObjectsSelectablePoolGroup.SelectedChanged += OnPoolLevelObjectsSelectedChanged;
        _levelObjectsSelectablePlaceGroup.SelectedChanged += OnPlaceLevelObjectsSelectedChanged;
        _pointerEventsHandler.PointerClickUp += OnPointerClickUp;

        _levelEditorContextButtons.RemoveButtonClicked += OnRemoveContextButtonClicked;

        PopulateLevelObjects(_prefabsManager.Prefabs);
    }

    private void OnPoolLevelObjectsSelectedChanged(SelectableGameObjectWrapper selectable)
    {
        _selectedPoolGameObject = selectable == null ? null : selectable.WrappedGameObject;
    }

    private void OnPlaceLevelObjectsSelectedChanged(SelectableGameObjectWrapper selectable)
    {
        _selectedPlaceGameObjectWrapper = selectable;
        var selectedGameObject = selectable == null ? null : selectable.WrappedGameObject;
        if (selectedGameObject != null) _levelEditorContextButtons.Show(selectedGameObject);
    }

    private void OnPointerClickUp(PointerEventData eventData)
    {
        if (_selectedPoolGameObject != null)
        {
            var levelObject = CreateSelectableGameObjectWrapper(_selectableLevelObjectWrapperPlace, _selectedPoolGameObject, _selectableLevelObjectsPlaceParent, _levelObjectsSelectablePlaceGroup);
            _snapPlacer.Place(levelObject.transform as RectTransform, eventData.pointerCurrentRaycast.worldPosition);
        }
    }

    private void OnRemoveContextButtonClicked()
    {
        _levelObjectsSelectablePlaceGroup.RemoveSelectable(_selectedPlaceGameObjectWrapper);
        Destroy(_selectedPlaceGameObjectWrapper.gameObject);
    }

    private void PopulateLevelObjects(IEnumerable<GameObject> levelPrefabs)
    {
        foreach (var toWrap in levelPrefabs)
        {
            CreateSelectableGameObjectWrapper(_selectableLevelObjectWrapperPool, toWrap, _selectableLevelObjectsPoolParent, _levelObjectsSelectablePoolGroup);
        }
    }

    private static SelectableGameObjectWrapper CreateSelectableGameObjectWrapper(
        SelectableGameObjectWrapper wrapperPrefab,
        GameObject toWrap,
        RectTransform parent,
        SelectableElementsGroup<SelectableGameObjectWrapper> wrapperGroup)
    {
        var wrapper = Instantiate(wrapperPrefab);
        wrapper.transform.SetParent(parent, false);
        wrapperGroup.AddSelectable(wrapper);
        wrapper.SetGameObject(toWrap);
        return wrapper;
    }

    public void RequestCloseLevelEditor()
    {
        CloseEditorRequested?.Invoke();
    }

    public void LoadLevel()
    {
        var levelData = _prefabsManager.ToLevelData(
            _levelObjectsSelectablePlaceGroup.SelectableElements.Select(x => (x.WrappedGameObject, (Vector2)x.transform.position)));
        levelData.LevelHalfDuration = 10;
        Logger.Log(JsonUtility.ToJson(levelData));

        _levelSharedContext.LevelData = levelData;
        LoadLevelRequested?.Invoke();
    }
}
