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
    private void Construct(PlayPauseService service, Timer timer)
    {
        ConstructBase(service);

        _timer = timer;

        service.PlayPaused += OnPlayPaused;
        service.PlayResumed += OnPlayResumed;

        OnDestroying += () =>
        {
            service.PlayPaused -= OnPlayPaused;
            service.PlayResumed -= OnPlayResumed;
        };
    }

    protected override void Awake()
    {
        base.Awake();

        _pauseButton.SetActive(false);
        _pauseMenu.SetActive(false);
    }

    protected override void EnableView()
    {
        _pauseButton.SetActiveNextFrame(true, _timer);
    }

    protected override void DisableView()
    {
        _pauseButton.SetActiveNextFrame(false, _timer);
        _pauseMenu.SetActiveNextFrame(false, _timer);
    }

    private void OnPlayPaused()
    {
        _pauseButton.SetActiveNextFrame(false, _timer);
        _pauseMenu.SetActiveNextFrame(true, _timer);
    }

    private void OnPlayResumed()
    {
        _pauseButton.SetActiveNextFrame(true, _timer);
        _pauseMenu.SetActiveNextFrame(false, _timer);
    }
}
