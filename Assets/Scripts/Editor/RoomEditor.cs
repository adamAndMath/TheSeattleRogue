using UnityEditor;

[CustomEditor(typeof(Room)), CanEditMultipleObjects]
public class RoomEditor : Editor
{
    private SerializedProperty propSize;
    private SerializedProperty propWalls;

    void OnEnable()
    {
        propSize = serializedObject.FindProperty("size");
        propWalls = serializedObject.FindProperty("walls");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginDisabledGroup(serializedObject.targetObjects.Length > 1);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(propSize);

        if (EditorGUI.EndChangeCheck())
        {
            int width = propSize.FindPropertyRelative("x").intValue;
            int height = propSize.FindPropertyRelative("y").intValue;

            OnSizeChange(width, height);
        }

        EditorGUI.EndDisabledGroup();
        EditorGUILayout.PropertyField(propWalls, true);

        serializedObject.ApplyModifiedProperties();
    }

    void OnSizeChange(int width, int height)
    {
        SerializedProperty propColumns = serializedObject.FindProperty("columns");

        if (propColumns.arraySize != width)
            propColumns.arraySize = width;

        for (int i = 0; i < propColumns.arraySize; i++)
        {
            SerializedProperty propColumn = propColumns.GetArrayElementAtIndex(i).FindPropertyRelative("data");

            if (propColumn.arraySize != height)
                propColumn.arraySize = height;
        }
    }
}
