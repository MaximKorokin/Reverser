using UnityEngine;
using VContainer;

public class PlayPauseView : ViewBase
{
    [SerializeField]
    private GameObject _pauseButton;
    [SerializeField]
    private GameObject _pauseMenu;

    [Inject]
    private void Construct(PlayPauseService playPauseController)
    {
        ConstructBase(playPauseController);

        gameObject.SetActive(true);

        playPauseController.PlayPaused += OnPlayPaused;
        playPauseController.PlayResumed += OnPlayResumed;

        OnDestroying += () =>
        {
            playPauseController.PlayPaused -= OnPlayPaused;
            playPauseController.PlayResumed -= OnPlayResumed;
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
        _pauseButton.SetActive(true);
    }

    protected override void DisableView()
    {
        _pauseButton.SetActive(false);
        _pauseMenu.SetActive(false);
    }

    private void OnPlayPaused()
    {
        _pauseMenu.SetActive(true);
    }

    private void OnPlayResumed()
    {
        _pauseMenu.SetActive(false);
    }
}
