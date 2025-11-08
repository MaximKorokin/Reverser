using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorConfirmDialog : UIBehaviourBase
{
    [SerializeField]
    private Button _confirmButton;
    [SerializeField]
    private Button _declineButton;

    private Action _onSuccessCallback;

    protected override void Awake()
    {
        base.Awake();

        _confirmButton.onClick.AddListener(OnConfirm);
        _declineButton.onClick.AddListener(OnDecline);
    }

    public void Show(Action onSuccessCallback)
    {
        _onSuccessCallback = onSuccessCallback;
        Show();
    }

    private void OnConfirm()
    {
        _onSuccessCallback?.Invoke();
        Hide();
    }

    private void OnDecline()
    {
        Hide();
    }
}
