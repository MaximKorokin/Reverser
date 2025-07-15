using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using VContainer;

public class LevelEditorController : MonoBehaviourBase
{
    [SerializeField]
    private RectTransform _selectableLevelObjectsParent;
    [SerializeField]
    private SelectableGameObjectWrapper _selectableLevelObjectWrapper;

    private LevelSharedContext _levelSharedContext;
    private Transform _levelParent;

    private readonly SelectableElementsGroup<SelectableGameObjectWrapper> _levelObjectsSelectableGroup = new();

    public event Action LoadLevelRequested;
    public event Action CloseEditorRequested;

    [Inject]
    private void Construct(
        LevelSharedContext levelSharedContext,
        Transform levelParent,
        LevelPrefabs levelPrefabs)
    {
        gameObject.SetActive(false);

        _levelSharedContext = levelSharedContext;
        _levelParent = levelParent;

        _levelObjectsSelectableGroup.SelectedChanged += OnLevelObjectsSelectedChanged;

        PopulateLevelObjects(levelPrefabs);
    }

    private void OnLevelObjectsSelectedChanged(SelectableGameObjectWrapper selectable)
    {
        Logger.Log(selectable.WrappedGameObject);
    }

    private void PopulateLevelObjects(LevelPrefabs levelPrefabs)
    {
        foreach (var behaviour in levelPrefabs
            .GetType()
            .GetAllProperties(flags: BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
            .Select(x => x.GetValue(levelPrefabs))
            .Where(x => x is Component or GameObject))
        {
            var wrapper = Instantiate(_selectableLevelObjectWrapper);
            wrapper.transform.SetParent(_selectableLevelObjectsParent, false);
            _levelObjectsSelectableGroup.AddSelectable(wrapper);

            if (behaviour is GameObject obj) wrapper.SetGameObject(obj);
            else if (behaviour is Component component) wrapper.SetGameObject(component.gameObject);
        }
    }

    public void RequestCloseLevelEditor()
    {
        CloseEditorRequested?.Invoke();
    }

    public void LoadLevel()
    {
        var levelData = new LevelData();
        _levelSharedContext.LevelData = levelData;
        LoadLevelRequested?.Invoke();
    }
}
