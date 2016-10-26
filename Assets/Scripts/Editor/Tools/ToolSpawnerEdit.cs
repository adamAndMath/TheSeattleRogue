using UnityEditor;
using UnityEngine;

public class ToolSpawnerEdit : Tool
{
    private int selectedObject;
    private LevelGenerator.Position selectedPosition;
    private SerializedProperty propSpawnMask;
    private int spawnerID;
    private Mode mode = Mode.None;

    public enum Mode
    {
        None, Sellect
    }

    public ToolSpawnerEdit(RoomWindow window) : base(window, "Place Edit")
    {
    }

    public override void OnGUI()
    {
        if (propSpawnMask != null)
        {
            propSpawnMask.intValue = SelectionGrid(propSpawnMask.intValue, Mathf.FloorToInt((RoomWindow.SideWidth - 4) / 36));
        }
    }

    private int SelectionGrid(int sellectionMask, int xCount)
    {
        if (Room.spawnables == null) return sellectionMask;

        Rect control = EditorGUILayout.GetControlRect(false, Room.spawnables.Length / xCount);

        for (int i = 0; i < Room.spawnables.Length; i++)
        {
            int x = i % xCount;
            int y = Mathf.FloorToInt(i / xCount);

            bool val = GUI.Toggle(new Rect(control.x + x * 36, control.y + y * 36, 32, 32), (sellectionMask & (1 << i)) != 0, "", GUI.skin.button);
            sellectionMask |= 1 << i;

            if (val)
                sellectionMask ^= 1 << i;

            if (Room.spawnables[i] != null)
                GUI.DrawTextureWithTexCoords(new Rect(control.x + x * 36 + 2, control.y + y * 36 + 2, 28, 28), Room.spawnables[i].idleSprite.texture, RoomWindow.GetTextureRect(Room.spawnables[i].idleSprite));
        }

        return sellectionMask;
    }

    public override bool PreRenderGrid(LevelGenerator.Position pos, Rect rect)
    {
        if (selectedPosition != pos || mode == Mode.None) return true;

        if (mode == Mode.Sellect)
            GUI.DrawTextureWithTexCoords(rect, RoomWindow.QuestionMark, new Rect(0.1F, 0.1F, 0.9F, 0.9F));

        return false;
    }

    public override bool OnMouseDown(LevelGenerator.Position pos)
    {
        int button = Event.current.button;

        if (pos.x == -1 || pos.y == -1 || button > 1)
            return false;

        if (mode == Mode.None)
        {
            selectedPosition = pos;
            
            spawnerID = -1;

            if (Room.spawners != null)
            {
                for (int i = 0; i < Room.spawners.Length; i++)
                {
                    if (Room.spawners[i].position == selectedPosition)
                    {
                        spawnerID = i;
                        break;
                    }
                }
            }

            if (spawnerID != -1)
                mode = Mode.Sellect;
        }
        else
        {
            mode = Mode.None;
        }

        return true;
    }

    public override bool OnMouseDrag(LevelGenerator.Position pos)
    {
        if (pos.x == -1 || pos.y == -1)
        {
            mode = Mode.None;
        }

        if (mode != Mode.None)
        {
            if (selectedPosition != pos)
            {
                mode = Mode.None;
            }
        }

        return false;
    }

    public override bool OnMouseUp(LevelGenerator.Position pos)
    {
        if (pos.x == -1 || pos.y == -1)
            return false;

        switch (mode)
        {
            case Mode.Sellect:
                if (spawnerID == -1)
                {
                    propSpawnMask = null;
                    break;
                }

                propSpawnMask = FindProperty("spawners").GetArrayElementAtIndex(spawnerID).FindPropertyRelative("spawnMask");
                break;
            case Mode.None: return false;
        }

        mode = Mode.None;
        return true;
    }
}
