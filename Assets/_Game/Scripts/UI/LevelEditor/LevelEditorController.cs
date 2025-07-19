using System;
using System.Linq;
using UnityEngine;
using VContainer;

public class LevelEditorController : MonoBehaviourBase
{
    [SerializeField]
    private LevelEditorDataExchange _levelEditorDataExchange;
    [SerializeField]
    private LevelEditorPlaceAreaController _levelEditorPlaceAreaController;
    [SerializeField]
    private LevelEditorPoolAreaController _levelEditorPoolAreaController;

    private LevelSharedContext _levelSharedContext;
    private LevelPrefabsManager _prefabsManager;

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

        _levelEditorDataExchange.LevelDataImportRequested += OnLevelDataImportRequested;

        _levelEditorPoolAreaController.PopulateLevelObjects(_prefabsManager.Prefabs);
        _levelEditorPoolAreaController.SelectedPrefabChanged += OnSelectedPoolPrefabChanged;
    }

    private void OnSelectedPoolPrefabChanged(GameObject prefab)
    {
        _levelEditorPlaceAreaController.SetPlacePrefab(prefab);
    }

    private void OnLevelDataImportRequested(LevelData levelData)
    {
        _levelEditorPlaceAreaController.SetPositionedPrefabs(levelData.LevelObjects.Select(x => (_prefabsManager.ToLevelPrefab(x.Name), x.Position)));
    }

    private LevelData ComposeLevelData()
    {
        var levelData = _prefabsManager.ToLevelData(_levelEditorPlaceAreaController.GetPositionedPrefabs());
        levelData.LevelHalfDuration = 10;
        return levelData;
    }

    public void RequestCloseLevelEditor()
    {
        CloseEditorRequested?.Invoke();
    }

    public void LoadLevel()
    {
        _levelSharedContext.LevelData = ComposeLevelData();
        LoadLevelRequested?.Invoke();
    }

    public void OpenLevelDataExchange()
    {
        _levelEditorDataExchange.SetData(ComposeLevelData());
        _levelEditorDataExchange.Show();
    }
}
