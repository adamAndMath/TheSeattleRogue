using UnityEditor;

[CustomEditor(typeof(Room)), CanEditMultipleObjects]
public class RoomEditor : Editor
{
    private SerializedProperty propSize;
    private SerializedProperty propEntrences;
    private SerializedProperty propPlatform;
    private SerializedProperty propWalls;
    private SerializedProperty propSpawnables;

    void OnEnable()
    {
        propSize = serializedObject.FindProperty("size");
        propEntrences = serializedObject.FindProperty("entrences");
        propPlatform = serializedObject.FindProperty("platformSprite");
        propWalls = serializedObject.FindProperty("walls");
        propSpawnables = serializedObject.FindProperty("spawnables");
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
        EditorGUILayout.PropertyField(propEntrences);
        EditorGUILayout.PropertyField(propPlatform);
        EditorGUILayout.PropertyField(propWalls, true);
        EditorGUILayout.PropertyField(propSpawnables, true);

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
