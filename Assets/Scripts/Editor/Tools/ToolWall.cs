using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ToolWall : Tool
{
    private int selectedObject;
    private readonly List<Position> selectedPositions = new List<Position>();
    private Mode mode = Mode.None;
    private bool slope;

    public enum Mode
    {
        None, Place, Delete
    }

    public ToolWall(RoomWindow window) : base(window, "Wall")
    {
    }

    public override void OnGUI()
    {
        if (Room.walls.Length == 0) return;

        slope = GUILayout.Toggle(slope, "Slope");

        Sprite[] thumbnails = Room.walls.Select(wall => wall.only).ToArray();
        selectedObject = SelectionGrid(selectedObject, thumbnails, Mathf.FloorToInt((RoomWindow.SideWidth - 4) / 36));
    }

    public override bool PreRenderGrid(Position pos, Rect rect)
    {
        if (!selectedPositions.Contains(pos))
            return true;

        if (mode == Mode.Place)
        {
            Sprite sprite = Room.walls[selectedObject].only;
            GUI.DrawTextureWithTexCoords(rect, sprite.texture, RoomWindow.GetTextureRect(sprite));
        }

        return false;
    }

    public override bool OnMouseDown(Position pos)
    {
        int button = Event.current.button;

        if (pos.x == -1 || pos.y == -1 || button > 1)
            return false;

        selectedPositions.Clear();

        if (mode == Mode.None)
        {
            mode = button == 0 ? Mode.Place : Mode.Delete;
            selectedPositions.Add(pos);
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
            selectedPositions.Clear();
            mode = Mode.None;
            return false;
        }

        if (mode != Mode.None)
        {
            if (!selectedPositions.Contains(pos))
            {
                selectedPositions.Add(pos);
                return true;
            }
        }

        return false;
    }

    public override bool OnMouseUp(Position pos)
    {
        if (pos.x == -1 || pos.y == -1)
            return false;

        switch (mode)
        {
            case Mode.Place:
                foreach (Position p in selectedPositions)
                {
                    GetPropertyAtPos(p).FindPropertyRelative("wallID").intValue = selectedObject + 1;
                    GetPropertyAtPos(p).FindPropertyRelative("slope").boolValue = slope;
                }

                break;
            case Mode.Delete:
                foreach (Position p in selectedPositions)
                {
                    GetPropertyAtPos(p).FindPropertyRelative("wallID").intValue = 0;
                    GetPropertyAtPos(p).FindPropertyRelative("slope").boolValue = false;
                }

                break;
            case Mode.None: return false;
        }

        mode = Mode.None;
        selectedPositions.Clear();
        return true;
    }
}
