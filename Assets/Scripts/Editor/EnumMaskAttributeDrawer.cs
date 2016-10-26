using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(EnumMaskAttribute))]
public class EnumMaskAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int val = EditorGUI.MaskField(position, label, property.intValue, property.enumDisplayNames);

        if (property.intValue != val)
            property.intValue = val;
    }
}
