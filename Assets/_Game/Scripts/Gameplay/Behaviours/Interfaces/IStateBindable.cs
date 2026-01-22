public interface IStateBindable
{
    public void Bind(IStateful stateful)
    {
        stateful.StateChanged += OnStateChanged;
        OnStateChanged(stateful.CurrentState);
    }

    public void OnStateChanged(bool state);

    public void SetBindInterpretation(bool inversed);
}
