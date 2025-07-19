using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorContextButtons : UIBehaviourBase
{
    [SerializeField]
    private Button _removeButton;
    [SerializeField]
    private Button _bindButton;
    [SerializeField]
    private Button _unbindButton;

    public event Action RemoveButtonClicked;

    protected override void Awake()
    {
        base.Awake();

        _removeButton.onClick.AddListener(() =>
        {
            RemoveButtonClicked?.Invoke();
            Hide();
        });
    }

    public override void Show()
    {
        base.Show();
        _removeButton.gameObject.SetActive(true);
        _bindButton.gameObject.SetActive(false);
        _unbindButton.gameObject.SetActive(false);
    }
}
