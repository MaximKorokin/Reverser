using UnityEngine;
using VContainer;

public class PlayPauseView : ViewBase
{
    [SerializeField]
    private GameObject _pauseButton;
    [SerializeField]
    private GameObject _pauseMenu;

    private Timer _timer;

    [Inject]
    private void Construct(GamePauseService service, Timer timer)
    {
        ConstructBase(service);

        _timer = timer;

        service.GamePaused += OnGamePaused;
        service.GameResumed += OnGameResumed;

        OnDestroying += () =>
        {
            service.GamePaused -= OnGamePaused;
            service.GameResumed -= OnGameResumed;
        };
    }

    protected override void Awake()
    {
        base.Awake();

        _pauseButton.SetActive(false);
        _pauseMenu.SetActive(false);
    }

    protected override void Enable()
    {
        _pauseButton.SetActiveNextFrame(true, _timer);
    }

    protected override void Disable()
    {
        _pauseButton.SetActiveNextFrame(false, _timer);
        _pauseMenu.SetActiveNextFrame(false, _timer);
    }

    private void OnGamePaused()
    {
        _pauseButton.SetActiveNextFrame(false, _timer);
        _pauseMenu.SetActiveNextFrame(true, _timer);
    }

    private void OnGameResumed()
    {
        _pauseButton.SetActiveNextFrame(true, _timer);
        _pauseMenu.SetActiveNextFrame(false, _timer);
    }
}
