using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

// todo: split ui and game logic
public class LevelSelectionController : MonoBehaviourBase
{
    [SerializeField]
    private Button _buttonPrefab;

    private LevelDataProvider _levelDataProvider;

    public event Action<LevelData> LevelSelected;

    [Inject]
    private void Construct(LevelDataProvider levelDataProvider)
    {
        _levelDataProvider = levelDataProvider;
    }

    public void GenerateButtons()
    {
        transform.Cast<Transform>().ForEach(x => Destroy(x.gameObject));
        _levelDataProvider.Levels.ForEach((_, i) => GenerateLevelButton(i));
    }

    private void GenerateLevelButton(int index)
    {
        var button = Instantiate(_buttonPrefab, transform);
        button.onClick.AddListener(() => LevelSelected?.Invoke(_levelDataProvider.GetLevelData(index)));
    }
}
