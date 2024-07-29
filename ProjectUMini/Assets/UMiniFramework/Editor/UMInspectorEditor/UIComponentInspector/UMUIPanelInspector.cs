using System.Collections.Generic;
using UMiniFramework.Editor.Common;
using UMiniFramework.Runtime.Modules.UIModule;
using UnityEditor;

[CustomEditor(typeof(UMUIPanel), true)]
public class UMUIPanelInspector : Editor
{
    private SerializedProperty m_setBtnClosePanelProp;
    private SerializedProperty m_btnClosePanelProp;
    protected List<string> m_exceptProps = new List<string>();

    protected virtual void OnEnable()
    {
        m_setBtnClosePanelProp = serializedObject.FindProperty("m_setBtnClosePanel");
        m_btnClosePanelProp = serializedObject.FindProperty("m_btnClosePanel");
        m_exceptProps.Add("m_setBtnClosePanel");
        m_exceptProps.Add("m_btnClosePanel");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        UMEditorUtils.DrawDefaultInspectorExcept(this, m_exceptProps.ToArray());
        DrawSetBtnClosePanel();
        serializedObject.ApplyModifiedProperties();
    }

    protected void DrawSetBtnClosePanel()
    {
        EditorGUILayout.PropertyField(m_setBtnClosePanelProp);
        if (m_setBtnClosePanelProp.boolValue)
        {
            EditorGUILayout.PropertyField(m_btnClosePanelProp);
        }
    }
}