using UnityEngine;
using VContainer;

public class TestScript : MonoBehaviour
{
    private TimeControlMediator _timeControlMediator;
    private InputSystemActions _inputActions;

    [Inject]
    private void Construct(TimeControlMediator timeControlMediator, InputSystemActions inputActions)
    {
        _timeControlMediator = timeControlMediator;
        _inputActions = inputActions;
    }

    private void Update()
    {
        //Logger.Log(_inputActions.UI.enabled);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Application.targetFrameRate = 60;
            _timeControlMediator.SetTimeFlowMode(TimeFlowMode.Backward);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            _timeControlMediator.SetTimeFlowMode(TimeFlowMode.Paused);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //Application.targetFrameRate = 10;
            _timeControlMediator.SetTimeFlowMode(TimeFlowMode.Forward);
        }
    }
}
