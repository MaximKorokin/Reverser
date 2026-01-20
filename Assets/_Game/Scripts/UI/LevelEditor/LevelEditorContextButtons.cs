using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorContextButtons : UIBehaviourBase
{
    [SerializeField]
    private Button _removeButton;
    [SerializeField]
    private Button _moveButton;
    [SerializeField]
    private Button _bindButton;
    [SerializeField]
    private Button _unbindButton;

    public event Action RemoveButtonClicked;
    public event Action MoveButtonClicked;
    public event Action BindButtonClicked;
    public event Action UnbindButtonClicked;

    protected override void Awake()
    {
        base.Awake();

        _removeButton.onClick.AddListener(() =>
        {
            RemoveButtonClicked?.Invoke();
            Hide();
        });

        _moveButton.onClick.AddListener(() =>
        {
            MoveButtonClicked?.Invoke();
            Hide();
        });

        _bindButton.onClick.AddListener(() =>
        {
            BindButtonClicked?.Invoke();
            Hide();
        });

        _unbindButton.onClick.AddListener(() =>
        {
            UnbindButtonClicked?.Invoke();
            Hide();
        });
    }

    public void Show(GameObject gameObject)
    {
        base.Show();
        _removeButton.gameObject.SetActive(true);
        _moveButton.gameObject.SetActive(true);
        if (gameObject.GetComponent<IStateBindable>() != null)
        {
            _bindButton.gameObject.SetActive(true);
            _unbindButton.gameObject.SetActive(true);
        }
    }

    public override void Hide()
    {
        base.Hide();

        _removeButton.gameObject.SetActive(false);
        _moveButton.gameObject.SetActive(false);
        _bindButton.gameObject.SetActive(false);
        _unbindButton.gameObject.SetActive(false);
    }
}
