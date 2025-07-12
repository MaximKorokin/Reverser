using UnityEngine;
using VContainer;

public class TestScript : MonoBehaviour
{
    private TimeControlMediator _timeControlMediator;

    [Inject]
    private void Construct(TimeControlMediator timeControlMediator)
    {
        _timeControlMediator = timeControlMediator;
    }

    private void Update()
    {
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
