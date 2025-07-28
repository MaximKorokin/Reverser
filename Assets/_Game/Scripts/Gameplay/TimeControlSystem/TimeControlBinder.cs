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
        foreach(var component in GetComponents<Component>())
        {
            var recorder = componentStateProcessorFactory.GetComponentStateRecorder(component);
            var player = componentStateProcessorFactory.GetComponentStatePlayer(component);
            if (recorder != null && player != null)
            {
                _timeControllers.Add(new(recorder, player));
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        _timeControllers.ForEach(x => x.Tick(_timeFlowModeInterpretation.InterpretTimeFlowMode(_timeControlMediator.TimeFlowMode), Time.deltaTime));
    }
}
