using System;
using UnityEngine.UI;

public class LevelEditorOpenButton : MonoBehaviourBase
{
    public event Action LevelEditorOpenRequested;

    protected override void Awake()
    {
        base.Awake();

        GetRequiredComponent<Button>().onClick.AddListener(() => LevelEditorOpenRequested?.Invoke());
    }
}
