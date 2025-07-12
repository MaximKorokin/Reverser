using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelConstructor
{
    private readonly Transform _levelParent;
    private readonly Func<Component, Vector2, Component> _componentFactory;
    private readonly LevelPrefabs _levelPrefabs;

    public LevelConstructor(
        Transform levelParent,
        Func<Component, Vector2, Component> characterFactory,
        LevelPrefabs levelPrefabs)
    {
        _levelParent = levelParent;
        _componentFactory = characterFactory;
        _levelPrefabs = levelPrefabs;
    }

    public void Clear()
    {
        _levelParent.Cast<Transform>().ForEach(x => UnityEngine.Object.Destroy(x.gameObject));
    }

    public void Costruct(LevelData levelData)
    {
        var levelTransforms = new List<Transform>
        {
            _componentFactory(_levelPrefabs.Character1, levelData.Character1Position).transform,
            _componentFactory(_levelPrefabs.Character2, levelData.Character2Position).transform,
            _componentFactory(_levelPrefabs.LevelEndZone, levelData.LevelEndDoorPosition).transform,
        };
        levelTransforms.AddRange(levelData.PickableBoxesPositions.Select(position => _componentFactory(_levelPrefabs.PickableBox, position).transform));
        levelTransforms.AddRange(levelData.LevelGroundPositions.Select(position => _componentFactory(_levelPrefabs.LevelGround, position).transform));

        levelTransforms.ForEach(x => x.SetParent(_levelParent, true));
    }
}
