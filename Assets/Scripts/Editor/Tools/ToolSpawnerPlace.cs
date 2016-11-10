using System.Linq;
using UnityEditor;
using UnityEngine;

public class ToolSpawnerPlace : Tool
{
    private int selectedObject;
    private Position selectedPosition;
    private int spawnerID;
    private Mode mode = Mode.None;

    public enum Mode
    {
        None, Place, Move, Delete
    }

    public ToolSpawnerPlace(RoomWindow window) : base(window, "Place Spawner")
    {
    }

    public override bool PreRenderGrid(Position pos, Rect rect)
    {
        if (selectedPosition != pos || mode == Mode.None) return true;

        if (mode == Mode.Place || mode == Mode.Move)
            GUI.DrawTextureWithTexCoords(rect, RoomWindow.QuestionMark, new Rect(0, 0, 1, 1));

        return false;
    }

    public override bool OnMouseDown(Position pos)
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

            mode = button == 0 ? (spawnerID == -1 ? Mode.Place : Mode.Move) : (spawnerID == -1 ? Mode.None : Mode.Delete);
        }
        else
        {
            mode = Mode.None;
        }

        return true;
    }

    public override bool OnMouseDrag(Position pos)
    {
        if (pos.x == -1 || pos.y == -1)
        {
            mode = Mode.None;
            return false;
        }

        if (mode != Mode.None)
        {
            if (selectedPosition != pos)
            {
                selectedPosition = pos;
                return true;
            }
        }

        return false;
    }

    public override bool OnMouseUp(Position pos)
    {
        if (pos.x == -1 || pos.y == -1)
            return false;

        int endSpawnerID = -1;

        for (int i = 0; i < Room.spawners.Length; i++)
        {
            if (Room.spawners[i].position == selectedPosition)
            {
                endSpawnerID = i;
                break;
            }
        }

        switch (mode)
        {
            case Mode.Place:
                if (endSpawnerID != -1) break;
                int id = FindProperty("spawners").arraySize++;
                SerializedProperty propPos = FindProperty("spawners").GetArrayElementAtIndex(id).FindPropertyRelative("position");
                propPos.FindPropertyRelative("x").intValue = selectedPosition.x;
                propPos.FindPropertyRelative("y").intValue = selectedPosition.y;
                break;
            case Mode.Move:
                if (endSpawnerID != -1) break;
                propPos = FindProperty("spawners").GetArrayElementAtIndex(spawnerID).FindPropertyRelative("position");
                propPos.FindPropertyRelative("x").intValue = selectedPosition.x;
                propPos.FindPropertyRelative("y").intValue = selectedPosition.y;
                break;
            case Mode.Delete:
                FindProperty("spawners").DeleteArrayElementAtIndex(endSpawnerID);
                break;
            case Mode.None: return false;
        }

        mode = Mode.None;
        return true;
    }
}
