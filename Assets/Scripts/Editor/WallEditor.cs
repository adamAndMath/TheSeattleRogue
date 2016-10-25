using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Room.Wall))]
public class WallEditor : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight*5;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position, label);
        position.y += EditorGUIUtility.singleLineHeight;
        position.height = EditorGUIUtility.singleLineHeight;

        SpriteField(new Rect(position.x, position.y, position.width / 4, position.height), property.FindPropertyRelative("upLeft"));
        SpriteField(new Rect(position.x + position.width / 4, position.y, position.width / 4, position.height), property.FindPropertyRelative("up"));
        SpriteField(new Rect(position.x + position.width / 2, position.y, position.width / 4, position.height), property.FindPropertyRelative("upRight"));
        SpriteField(new Rect(position.x + position.width * 3 / 4, position.y, position.width / 4, position.height), property.FindPropertyRelative("upOnly"));

        SpriteField(new Rect(position.x, position.y + position.height, position.width / 4, position.height), property.FindPropertyRelative("left"));
        SpriteField(new Rect(position.x + position.width / 4, position.y + position.height, position.width / 4, position.height), property.FindPropertyRelative("center"));
        SpriteField(new Rect(position.x + position.width / 2, position.y + position.height, position.width / 4, position.height), property.FindPropertyRelative("right"));
        SpriteField(new Rect(position.x + position.width * 3 / 4, position.y + position.height, position.width / 4, position.height), property.FindPropertyRelative("upDown"));

        SpriteField(new Rect(position.x, position.y + position.height * 2, position.width / 4, position.height), property.FindPropertyRelative("downLeft"));
        SpriteField(new Rect(position.x + position.width / 4, position.y + position.height * 2, position.width / 4, position.height), property.FindPropertyRelative("down"));
        SpriteField(new Rect(position.x + position.width / 2, position.y + position.height * 2, position.width / 4, position.height), property.FindPropertyRelative("downRight"));
        SpriteField(new Rect(position.x + position.width * 3 / 4, position.y + position.height * 2, position.width / 4, position.height), property.FindPropertyRelative("downOnly"));

        SpriteField(new Rect(position.x, position.y + position.height * 3, position.width / 4, position.height), property.FindPropertyRelative("leftOnly"));
        SpriteField(new Rect(position.x + position.width / 4, position.y + position.height * 3, position.width / 4, position.height), property.FindPropertyRelative("leftRight"));
        SpriteField(new Rect(position.x + position.width / 2, position.y + position.height * 3, position.width / 4, position.height), property.FindPropertyRelative("rightOnly"));
        SpriteField(new Rect(position.x + position.width * 3 / 4, position.y + position.height * 3, position.width / 4, position.height), property.FindPropertyRelative("only"));
    }

    private void SpriteField(Rect position, SerializedProperty prop)
    {
        prop.objectReferenceValue = EditorGUI.ObjectField(position, GUIContent.none, prop.objectReferenceValue, typeof(Sprite), false);        
    }
}
