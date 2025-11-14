using System;

public class LevelOverlayService : ServiceBase
{
    public Action<OverlayType> OverlayRequested;

    public void SetOverlay(OverlayType overlayType)
    {
        OverlayRequested?.Invoke(overlayType);
    }

    public enum OverlayType
    {
        LevelStart = 10,
        LevelFail = 20,
        LevelComplete = 30,
    }
}
