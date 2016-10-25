using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(EnumMaskAttribute))]
public class EnumMaskAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumDisplayNames);
    }
}
