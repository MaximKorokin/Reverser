using UnityEngine;
using VContainer;

public class ExitGameView : ViewBase
{
    [SerializeField]
    private GameObject _exitButton;
    [SerializeField]
    private GameObject _exitPanel;

    private Timer _timer;

    [Inject]
    private void Construct(ExitGameService service, Timer timer)
    {
        ConstructBase(service);

        _timer = timer;

        service.ExitRequested += OnExitRequested;
    }

    protected override void DisableView()
    {
        _exitButton.SetActiveNextFrame(false, _timer);
        _exitPanel.SetActiveNextFrame(false, _timer);
    }

    protected override void EnableView()
    {
        _exitButton.SetActiveNextFrame(true, _timer);
    }

    private void OnExitRequested(bool value)
    {
        _exitPanel.SetActiveNextFrame(value, _timer);
    }
}
