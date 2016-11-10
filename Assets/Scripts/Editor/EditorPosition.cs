using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Position))]
public class EditorPosition : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorGUI.MultiPropertyField(position, new[] { new GUIContent("x"), new GUIContent("y") }, property.FindPropertyRelative("x"));

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
