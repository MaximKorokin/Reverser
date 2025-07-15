using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorContextButtons : MonoBehaviourBase
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

    public void Show(GameObject obj)
    {
        _removeButton.gameObject.SetActive(true);
        _bindButton.gameObject.SetActive(false);
        _unbindButton.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
