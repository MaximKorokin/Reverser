using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorPoolAreaController : LevelEditorSelectableAreaController
{
    [SerializeField]
    private RectTransform _selectableLevelObjectsPoolParent;

    public event Action<GameObject> SelectedPrefabChanged;
    private GameObject _selectedPoolGameObject;

    protected override void Start()
    {
        base.Awake();

        LevelObjectsSelectableGroup.SelectedChanged += OnPoolLevelObjectsSelectedChanged;
    }

    private void OnPoolLevelObjectsSelectedChanged(SelectableGameObjectWrapper selectable)
    {
        var newSelected = selectable == null ? null : selectable.WrappedGameObject;
        if (_selectedPoolGameObject != newSelected)
        {
            _selectedPoolGameObject = newSelected;
            SelectedPrefabChanged?.Invoke(_selectedPoolGameObject);
        }
    }

    public void PopulateLevelObjects(IEnumerable<GameObject> levelPrefabs)
    {
        foreach (var prefab in levelPrefabs)
        {
            CreateSelectableGameObjectWrapper(prefab, _selectableLevelObjectsPoolParent);
        }
    }
}
