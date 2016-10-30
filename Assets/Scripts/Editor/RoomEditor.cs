using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Room)), CanEditMultipleObjects]
public class RoomEditor : Editor
{
    private SerializedProperty propSize;
    private SerializedProperty propPlatform;
    private SerializedProperty propSpike;
    private SerializedProperty propWalls;
    private SerializedProperty propSpawnables;

    private SerializedProperty propEntrencesUp;
    private SerializedProperty propEntrencesDown;
    private SerializedProperty propEntrencesLeft;
    private SerializedProperty propEntrencesRight;

    void OnEnable()
    {
        propSize = serializedObject.FindProperty("size");
        propPlatform = serializedObject.FindProperty("platform");
        propSpike = serializedObject.FindProperty("spike");
        propWalls = serializedObject.FindProperty("walls");
        propSpawnables = serializedObject.FindProperty("spawnables");

        propEntrencesUp = serializedObject.FindProperty("entrencesUp");
        propEntrencesDown = serializedObject.FindProperty("entrencesDown");
        propEntrencesLeft = serializedObject.FindProperty("entrencesLeft");
        propEntrencesRight = serializedObject.FindProperty("entrencesRight");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUI.BeginDisabledGroup(serializedObject.targetObjects.Length > 1);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(propSize);

        int width = propSize.FindPropertyRelative("x").intValue;
        int height = propSize.FindPropertyRelative("y").intValue;

        if (EditorGUI.EndChangeCheck())
            OnSizeChange(width, height);

        Entrences(width, height);

        EditorGUI.EndDisabledGroup();
        EditorGUILayout.PropertyField(propPlatform);
        EditorGUILayout.PropertyField(propSpike);
        EditorGUILayout.PropertyField(propWalls, true);
        EditorGUILayout.PropertyField(propSpawnables, true);

        serializedObject.ApplyModifiedProperties();
    }

    void Entrences(int width, int height)
    {
        int up = propEntrencesUp.intValue;
        int down = propEntrencesDown.intValue;
        int left = propEntrencesLeft.intValue;
        int right = propEntrencesRight.intValue;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical(GUILayout.Width(16));
        GUILayout.Space(20);

        for (int i = 0; i < height; i++)
        {
            bool toggle = GUILayout.Toggle((left & (1 << i)) != 0, GUIContent.none, GUILayout.ExpandWidth(false));
            left |= 1 << i;

            if (!toggle)
                left ^= 1 << i;
        }

        EditorGUILayout.EndVertical();

        for (int i = 0; i < width; i++)
        {
            bool toggle = GUILayout.Toggle((up & (1 << i)) != 0, GUIContent.none, GUILayout.ExpandWidth(false));
            up |= 1 << i;

            if (!toggle)
                up ^= 1 << i;
        }

        EditorGUILayout.BeginVertical(GUILayout.Width(16));
        GUILayout.Space(20);

        for (int i = 0; i < height; i++)
        {
            bool toggle = GUILayout.Toggle((right & (1 << i)) != 0, GUIContent.none, GUILayout.ExpandWidth(false));
            right |= 1 << i;

            if (!toggle)
                right ^= 1 << i;
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(22);

        for (int i = 0; i < width; i++)
        {
            bool toggle = GUILayout.Toggle((down & (1 << i)) != 0, GUIContent.none, GUILayout.ExpandWidth(false));
            down |= 1 << i;

            if (!toggle)
                down ^= 1 << i;
        }
        EditorGUILayout.EndHorizontal();

        propEntrencesUp.intValue = up;
        propEntrencesDown.intValue = down;
        propEntrencesLeft.intValue = left;
        propEntrencesRight.intValue = right;
    }

    void OnSizeChange(int width, int height)
    {
        SerializedProperty propColumns = serializedObject.FindProperty("columns");

        if (propColumns.arraySize != width*Room.RoomSize.x - 1)
            propColumns.arraySize = width*Room.RoomSize.x - 1;

        for (int i = 0; i < propColumns.arraySize; i++)
        {
            SerializedProperty propColumn = propColumns.GetArrayElementAtIndex(i).FindPropertyRelative("data");

            if (propColumn.arraySize != height*Room.RoomSize.y - 1)
                propColumn.arraySize = height*Room.RoomSize.y - 1;
        }
    }
}
