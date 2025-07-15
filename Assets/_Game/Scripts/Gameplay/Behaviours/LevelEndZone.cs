using UnityEngine;
using VContainer;

public class LevelEndZone : MonoBehaviourBase
{
    private LevelSharedContext _levelSharedContext;

    [Inject]
    private void Construct(LevelSharedContext levelSharedContext)
    {
        _levelSharedContext = levelSharedContext;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent<Character>(out var character))
        {
            _levelSharedContext.InvokeLevelCompleted();
        }
    }
}
