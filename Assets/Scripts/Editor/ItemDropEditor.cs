using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ItemDrop))]
public class ItemDropEditor : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect objRect = new Rect(position.x, position.y, position.width/2 - 4, position.height);
        Rect chanceRect = new Rect(position.x + position.width / 2 + 4, position.y, position.width / 2 - 4, position.height);

        EditorGUI.PropertyField(objRect, property.FindPropertyRelative("obj"), GUIContent.none);
        property.FindPropertyRelative("chance").floatValue = Mathf.Clamp01(EditorGUI.FloatField(chanceRect, property.FindPropertyRelative("chance").floatValue));

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }
}
