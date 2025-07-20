using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using static TMPro.TMP_InputField;

public class LevelEditorInfoAreaController : UIBehaviourBase
{
    [SerializeField]
    private TMP_Text _textLabelPrefab;
    [SerializeField]
    private TMP_InputField _inputFieldPrefab;

    private Dictionary<FieldInfo, TMP_InputField> FieldsInputs => GetLazy(GenerateFieldsInputs);

    protected override void Awake()
    {
        base.Awake();
        var _ = FieldsInputs; // Trigger lazy evaluation
    }

    private Dictionary<FieldInfo, TMP_InputField> GenerateFieldsInputs()
    {
        return typeof(LevelData)
            .GetAllFields(flags: BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
            .Select(x => (Field: x, Info: x.GetCustomAttribute(typeof(LevelDataInfoAttribute)) as LevelDataInfoAttribute))
            .Where(x => x.Info != null)
            .ToDictionary(x => x.Field, x => CreateInputField(x.Field.FieldType, x.Info));
    }

    private TMP_InputField CreateInputField(Type contentType, LevelDataInfoAttribute levelDataInfo)
    {
        var label = Instantiate(_textLabelPrefab);
        label.transform.SetParent(transform, false);
        label.text = levelDataInfo.DisplayName;

        var inputField = Instantiate(_inputFieldPrefab);
        inputField.transform.SetParent(transform, false);
        inputField.contentType = contentType == typeof(float) ? ContentType.DecimalNumber : ContentType.Standard;
        inputField.text = levelDataInfo.DefaultValue;
        return inputField;
    }

    public void SetLevelData(LevelData levelData)
    {
        FieldsInputs.ForEach(x => x.Value.text = x.Key.GetValue(levelData)?.ToString());
    }

    public LevelData GetLevelData()
    {
        var levelData = new LevelData();

        FieldsInputs.ForEach(x => x.Key.SetValue(levelData, Convert.ChangeType(x.Value.text, x.Key.FieldType)));

        return levelData;
    }
}
