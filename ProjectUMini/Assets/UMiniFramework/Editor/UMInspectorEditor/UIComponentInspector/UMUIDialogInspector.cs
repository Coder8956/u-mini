using UMiniFramework.Editor.Common;
using UMiniFramework.Runtime.Modules.UIModule;
using UnityEditor;

[CustomEditor(typeof(UMUIDialog), true)]
public class UMUIDialogInspector : UMUIPanelInspector
{
    private SerializedProperty m_isMaskProp;
    private SerializedProperty m_maskSpriteProp;
    private SerializedProperty m_maskColorProp;

    protected override void OnEnable()
    {
        base.OnEnable();
        m_isMaskProp = serializedObject.FindProperty("m_isMask");
        m_maskSpriteProp = serializedObject.FindProperty("m_maskSprite");
        m_maskColorProp = serializedObject.FindProperty("m_maskColor");
        m_exceptProps.Add("m_isMask");
        m_exceptProps.Add("m_maskSprite");
        m_exceptProps.Add("m_maskColor");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        UMEditorUtils.DrawDefaultInspectorExcept(this, m_exceptProps.ToArray());
        DrawSetBtnClosePanel();
        DrawHasMask();
        serializedObject.ApplyModifiedProperties();
    }

    protected void DrawHasMask()
    {
        EditorGUILayout.PropertyField(m_isMaskProp);
        if (m_isMaskProp.boolValue)
        {
            EditorGUILayout.PropertyField(m_maskSpriteProp);
            EditorGUILayout.PropertyField(m_maskColorProp);
        }
    }
}