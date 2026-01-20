using System;

public interface IStateful
{
    public bool CurrentState { get; }

    public event Action<bool> StateChanged;
}
