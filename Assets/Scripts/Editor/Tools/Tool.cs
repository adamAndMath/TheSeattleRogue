using UnityEngine;
using UnityEditor;

public abstract class Tool
{
    private readonly RoomWindow Window;
    public readonly string Name;

    protected Room Room { get { return Window.room; } }

    protected Tool(RoomWindow window, string name)
    {
        Window = window;
        Name = name;
    }

    public virtual void OnEnable() { }
    public virtual void OnGUI() { }
    public virtual bool PreRenderGrid(LevelGenerator.Position pos, Rect rect) { return true; }
    public virtual void PostRenderGrid(LevelGenerator.Position pos, Rect rect) { }

    /// <summary>
    /// Runs mouse down event on tool
    /// </summary>
    /// <returns>Requires repaint</returns>
    public abstract bool OnMouseDown(LevelGenerator.Position pos);

    /// <summary>
    /// Runs mouse drag event on tool
    /// </summary>
    /// <returns>Requires repaint</returns>
    public abstract bool OnMouseDrag(LevelGenerator.Position pos);

    /// <summary>
    /// Runs mouse up event on tool
    /// </summary>
    /// <returns>Requires repaint</returns>
    public abstract bool OnMouseUp(LevelGenerator.Position pos);

    protected SerializedProperty FindProperty(string propertyPath)
    {
        return Window.serializedObject.FindProperty(propertyPath);
    }

    protected SerializedProperty GetPropertyAtPos(LevelGenerator.Position pos)
    {
        return FindProperty("columns").GetArrayElementAtIndex(pos.x).FindPropertyRelative("data").GetArrayElementAtIndex(pos.y);
    }

    protected static int SelectionGrid(int index, Sprite[] sprites, int xCount)
    {
        Rect control = EditorGUILayout.GetControlRect(false, 0);
        control = EditorGUILayout.GetControlRect(false, sprites.Length / xCount);

        for (int i = 0; i < sprites.Length; i++)
        {
            int x = i % xCount;
            int y = Mathf.FloorToInt(i / xCount);

            if (GUI.Toggle(new Rect(control.x + x * 36, control.y + y * 36, 32, 32), i == index, "", GUI.skin.button))
                index = i;

            if (sprites[i] != null)
                GUI.DrawTextureWithTexCoords(new Rect(control.x + x * 36 + 2, control.y + y * 36 + 2, 28, 28), sprites[i].texture, RoomWindow.GetTextureRect(sprites[i]));
        }

        return index;
    }
}
