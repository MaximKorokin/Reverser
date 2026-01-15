using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class LevelPlaybackView : ViewBase
{
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private TMP_Text _timeText;

    private LevelPlaybackService _levelPlaybackService;

    [Inject]
    private void Construct(LevelPlaybackService levelPlaybackService)
    {
        ConstructBase(levelPlaybackService);

        this.KeepSynchronized(_slider.gameObject);
        this.KeepSynchronized(_timeText.gameObject);

        levelPlaybackService.PlaybackValueChanged += OnPlaybackValueChanged;
        _levelPlaybackService = levelPlaybackService;
    }

    protected override void Disable()
    {
    }

    protected override void Enable()
    {
    }

    private void OnPlaybackValueChanged(float value)
    {
        _slider.value = value;
        _timeText.text = Mathf.Round(_levelPlaybackService.RemainingTime).ToString();
    }
}
