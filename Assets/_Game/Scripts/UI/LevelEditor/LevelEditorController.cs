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
    [SerializeField]
    private LevelEditorInfoAreaController _levelEditorInfoAreaController;

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

        _levelEditorDataExchange.LevelDataImportRequested += SetLevelData;

        _levelEditorPoolAreaController.PopulateLevelObjects(_prefabsManager.Prefabs);
        _levelEditorPoolAreaController.SelectedPrefabChanged += OnSelectedPoolPrefabChanged;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (_levelSharedContext.LevelData != null) SetLevelData(_levelSharedContext.LevelData);
    }

    private void OnSelectedPoolPrefabChanged(GameObject prefab)
    {
        _levelEditorPlaceAreaController.SetPlacePrefab(prefab);
    }

    private void SetLevelData(LevelData levelData)
    {
        _levelEditorPlaceAreaController.Reset();
        _levelEditorPlaceAreaController.SetPositionedPrefabs(levelData.LevelObjects.Select(x => (_prefabsManager.ToLevelPrefab(x.Name), x.Position)));
        _levelEditorInfoAreaController.SetLevelData(levelData);
    }

    private LevelData ComposeLevelData()
    {
        var levelData = _levelEditorInfoAreaController.GetLevelData();
        levelData.LevelObjects = _prefabsManager.ToLevelObjects(_levelEditorPlaceAreaController.GetPositionedPrefabs());
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
