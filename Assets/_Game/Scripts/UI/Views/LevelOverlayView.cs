using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class LevelOverlayView : ViewBase
{
    [SerializeField]
    private Image _overlay;

    [Inject]
    private void Construct(LevelOverlayService service)
    {
        ConstructBase(service);

        service.OverlayRequested += OnOverlayRequested;
    }

    protected override void Disable()
    {
        _overlay.gameObject.SetActive(false);
    }

    private void OnOverlayRequested(LevelOverlayService.OverlayType overlayType)
    {
        _overlay.gameObject.SetActive(true);
        _overlay.color = overlayType switch
        {
            LevelOverlayService.OverlayType.LevelStart => new(1, 1, 1, .1f),
            LevelOverlayService.OverlayType.LevelComplete => new(0, 1, 0, .1f),
            LevelOverlayService.OverlayType.LevelFail => new(1, 0, 0, .1f),
            _ => new()
        };
    }
}
