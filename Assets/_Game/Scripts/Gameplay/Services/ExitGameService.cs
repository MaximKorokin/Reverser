using System;
using UnityEngine;

public class ExitGameService : UIInputServiceBase
{
    private bool _isRequested;

    public event Action<bool> ExitRequested;

    public ExitGameService(UIInputHandler uiInputHandler) : base(uiInputHandler)
    {
    }

    protected override void OnSubmitInputRecieved()
    {
        if (_isRequested) Application.Quit();
    }

    protected override void OnCancelInputRecieved()
    {
        _isRequested = !_isRequested;
        ExitRequested?.Invoke(_isRequested);
    }
}