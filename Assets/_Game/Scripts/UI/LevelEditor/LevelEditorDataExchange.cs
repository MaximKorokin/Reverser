using System;
using TMPro;
using UnityEngine;

public class LevelEditorDataExchange : UIBehaviourBase
{
    [SerializeField]
    private TMP_InputField _inputField;

    public event Action<LevelData> LevelDataImportRequested;

    public void SetData(LevelData levelData)
    {
        _inputField.text = JsonUtility.ToJson(levelData);
    }

    public void RequestDataImport()
    {
        var data = JsonUtility.FromJson<LevelData>(_inputField.text);
        if (data != null)
        {
            LevelDataImportRequested?.Invoke(data);
            Hide();
        }
    }
}
