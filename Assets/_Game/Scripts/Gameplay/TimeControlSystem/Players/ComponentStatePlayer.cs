public abstract class ComponentStatePlayer : ComponentStateProcessor
{
    public ComponentStatePlayer(TimeControlSettings settings) : base(settings) { }

    public abstract void PlayState(ComponentState state);
    public abstract void PlayInterpolatedState(ComponentState state1, ComponentState state2, float time);
}
