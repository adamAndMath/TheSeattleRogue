using System.Collections.Generic;
using UnityEngine;

public class ToolSpike : Tool
{
    private readonly List<LevelGenerator.Position> selectedPositions = new List<LevelGenerator.Position>();
    private Mode mode = Mode.None;

    public enum Mode
    {
        None, Place, Delete
    }

    public ToolSpike(RoomWindow window) : base(window, "Spike")
    {
    }

    public override bool PreRenderGrid(LevelGenerator.Position pos, Rect rect)
    {
        if (!selectedPositions.Contains(pos))
            return true;

        if (mode == Mode.Place)
        {
            GUI.DrawTextureWithTexCoords(rect, Room.spike.sprite.texture, RoomWindow.GetTextureRect(Room.spike.sprite));
        }

        return false;
    }

    public override bool OnMouseDown(LevelGenerator.Position pos)
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

    public override bool OnMouseDrag(LevelGenerator.Position pos)
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

    public override bool OnMouseUp(LevelGenerator.Position pos)
    {
        if (pos.x == -1 || pos.y == -1)
            return false;

        switch (mode)
        {
            case Mode.Place:
                foreach (LevelGenerator.Position p in selectedPositions)
                {
                    GetPropertyAtPos(p).FindPropertyRelative("wallID").intValue = -2;
                    GetPropertyAtPos(p).FindPropertyRelative("slope").boolValue = false;
                }

                break;
            case Mode.Delete:
                foreach (LevelGenerator.Position p in selectedPositions)
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
