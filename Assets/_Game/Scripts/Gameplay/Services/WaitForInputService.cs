using System;

public class WaitForInputService : UIInputServiceBase
{
    public Action InputReceived;

    public WaitForInputService(UIInputHandler inputhandler) : base(inputhandler) { }

    protected override void OnCancelInputRecieved()
    {
        
    }

    protected override void OnSubmitInputRecieved()
    {
        InputReceived?.Invoke();
    }
}