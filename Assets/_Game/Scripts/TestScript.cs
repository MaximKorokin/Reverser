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

    }
}
