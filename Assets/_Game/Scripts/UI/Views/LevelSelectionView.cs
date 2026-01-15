using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class LevelSelectionView : ViewBase
{
    [SerializeField]
    private Button _buttonPrefab;

    private LevelDataProvider _levelDataProvider;
    private LevelSelectionService _service;

    [Inject]
    private void Construct(LevelSelectionService service, LevelDataProvider levelDataProvider)
    {
        ConstructBase(service);

        _levelDataProvider = levelDataProvider;
        _service = service;
    }

    protected override void Enable()
    {
        _levelDataProvider.Levels.ForEach((_, i) => GenerateLevelButton(i));
    }

    protected override void Disable()
    {
        transform.Cast<Transform>().ForEach(x => Destroy(x.gameObject));
    }

    private void GenerateLevelButton(int index)
    {
        var button = Instantiate(_buttonPrefab, transform);
        button.onClick.AddListener(() => _service.SelectLevel(_levelDataProvider.GetLevelData(index)));
    }
}
