using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class TimeControlBinder : MonoBehaviourBase
{
    [SerializeField]
    private TimeFlowModeInterpretation _timeFlowModeInterpretation;

    private readonly List<TimeController> _timeControllers = new();
    private TimeControlMediator _timeControlMediator;

    [Inject]
    private void Construct(TimeControlMediator timeControlMediator, ComponentStateProcessorFactory componentStateProcessorFactory)
    {
        _timeControlMediator = timeControlMediator;

        CreateTimeControllers(componentStateProcessorFactory);
    }

    private void CreateTimeControllers(ComponentStateProcessorFactory componentStateProcessorFactory)
    {
        _timeControllers.Add(new(
            componentStateProcessorFactory.GetComponentStateRecorder(transform),
            componentStateProcessorFactory.GetComponentStatePlayer(transform)));

        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            _timeControllers.Add(new(
                componentStateProcessorFactory.GetComponentStateRecorder(spriteRenderer),
                componentStateProcessorFactory.GetComponentStatePlayer(spriteRenderer)));
        }
    }

    protected override void Update()
    {
        base.Update();
        _timeControllers.ForEach(x => x.Tick(_timeFlowModeInterpretation.InterpretTimeFlowMode(_timeControlMediator.TimeFlowMode)));
    }
}
