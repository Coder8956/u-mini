using System.Collections.Generic;
using UMiniFramework.Editor.Common;
using UMiniFramework.Scripts.Modules.UIModule;
using UnityEditor;

[CustomEditor(typeof(UMUIPanel), true)]
public class UMUIPanelInspector : Editor
{
    private SerializedProperty m_isHasBtnCloseProp;
    private SerializedProperty m_btnCloseProp;
    protected List<string> m_exceptProps = new List<string>();

    protected virtual void OnEnable()
    {
        m_isHasBtnCloseProp = serializedObject.FindProperty("m_isHasBtnClose");
        m_btnCloseProp = serializedObject.FindProperty("m_btnClose");
        m_exceptProps.Add("m_isHasBtnClose");
        m_exceptProps.Add("m_btnClose");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        UMEditorUtils.DrawDefaultInspectorExcept(this, m_exceptProps.ToArray());
        DrawHasBtnClose();
        serializedObject.ApplyModifiedProperties();
    }

    protected void DrawHasBtnClose()
    {
        EditorGUILayout.PropertyField(m_isHasBtnCloseProp);
        if (m_isHasBtnCloseProp.boolValue)
        {
            EditorGUILayout.PropertyField(m_btnCloseProp);
        }
    }
}