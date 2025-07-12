using UnityEngine;

[CreateAssetMenu(fileName = "TimeControlSettings", menuName = "Scriptable Objects/TimeControlSettings")]
public class TimeControlSettings : ScriptableObject
{
    [field: SerializeField]
    public float MaxSecondsToRecord { get; private set; }
    [field: SerializeField]
    public TimeFlowMode InitialTimeFlowMode{ get; private set; }
}
