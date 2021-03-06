﻿using System.Linq;
using UnityEditor;
using UnityEngine;

public class RoomWindow : EditorWindow
{
    private static Texture2D questionMark;

    public static Texture2D QuestionMark
    {
        get
        {
            if (questionMark == null)
                questionMark = (Texture2D) EditorGUIUtility.Load("Question Mark.png");

            return questionMark;
        }
    }

    private readonly Tool[] Tools;
    public const float SideWidth = 128;

    public Room room;
    public SerializedObject serializedObject;

    private bool isLocked;
    private int toolID;
    private int selectedObject;
    private float gridSize = 64;
    private Vector2 scrollPosition;

    private Tool CurrentTool { get { return Tools[toolID]; } }
    private Rect ToolRect { get { return new Rect(0, 0, SideWidth, position.height); } }
    private Rect GridRect { get { return new Rect(SideWidth, 0, position.width - SideWidth, position.height); } }

    [MenuItem("Window/Room")]
    private static void Open()
    {
        RoomWindow window = GetWindow<RoomWindow>();
        window.Show();
    }

    public RoomWindow()
    {
        Tools = new Tool[] { new ToolWall(this), new ToolPlatform(this), new ToolSpike(this), new ToolSpawnerPlace(this), new ToolSpawnerEdit(this) };
        minSize = new Vector2(256, 256);
        titleContent.text = "Room Editor";
    }

    /// <summary>
    /// Hidden unity function. Adds buttons to the upper rigt of your window.
    /// </summary>
    /// <param name="position"></param>
    private void ShowButton(Rect position)
    {
        isLocked = GUI.Toggle(position, isLocked, GUIContent.none, "IN LockButton");
        OnSelectionChange();
    }

    /// <summary>
    /// Called when window gains focus
    /// </summary>
    private void OnFocus()
    {
        OnSelectionChange();
    }

    private void OnSelectionChange()
    {
        if (isLocked || room == Selection.activeObject) return;

        if (serializedObject != null)
            serializedObject.Dispose();

        room = Selection.activeObject as Room;
        serializedObject = room ? new SerializedObject(room) : null;
        Repaint();
    }

    private void OnGUI()
    {
        if (!room)
        {
            GUIToolBox(new Rect(0, 0, SideWidth, position.height));
            return;
        }

        if (serializedObject == null)
            serializedObject = new SerializedObject(room);

        serializedObject.Update();

        if (RunEvent())
            Repaint();

        GUIToolBox(ToolRect);
        GUIGrid(GridRect);

        serializedObject.ApplyModifiedProperties();
    }

    /// <summary>
    /// Runs an event on the current tool
    /// </summary>
    /// <returns>Requires repaint</returns>
    private bool RunEvent()
    {
        switch (Event.current.type)
        {
            case EventType.mouseDown: return CurrentTool.OnMouseDown(GetMousePos());
            case EventType.mouseUp: return CurrentTool.OnMouseUp(GetMousePos());
            case EventType.mouseDrag: return CurrentTool.OnMouseDrag(GetMousePos());
            case EventType.ValidateCommand: return true;
            case EventType.scrollWheel:
                if (!GridRect.Contains(Event.current.mousePosition))
                    return false;

                if (Event.current.alt)
                    gridSize = Mathf.Max(1, gridSize - Event.current.delta.y);
                else if (Event.current.control)
                    scrollPosition.x += Event.current.delta.y * 16;
                else
                    scrollPosition += Event.current.delta * 16;

                scrollPosition.x = Mathf.Clamp(scrollPosition.x, 0, GridRect.width);
                scrollPosition.y = Mathf.Clamp(scrollPosition.y, 0, GridRect.height);
                Event.current.Use();
                return true;
            default: return false;
        }
    }

    /// <summary>
    /// Returns what square of the grid the mouse is over.
    /// </summary>
    /// <returns>Grid position</returns>
    private Position GetMousePos()
    {
        Rect gridRect = new Rect(SideWidth, 0, position.width - SideWidth, position.height);

        if (gridRect.width < room.RealSize.x * gridSize)
        {
            gridRect.height -= 20;

            if (gridRect.height < room.RealSize.y * gridSize)
            {
                gridRect.width -= 20;
            }
        }
        else if (gridRect.height < room.RealSize.y * gridSize)
        {
            gridRect.width -= 20;

            if (gridRect.width < room.RealSize.x * gridSize)
            {
                gridRect.height -= 20;
            }
        }

        Vector2 pos = Event.current.mousePosition;
        if (!gridRect.Contains(pos))
            return new Position(-1, -1);

        pos += scrollPosition - gridRect.position;
        pos /= gridSize;
        return new Position(Mathf.FloorToInt(pos.x), room.RealSize.y - 1 - Mathf.FloorToInt(pos.y));
    }

    /// <summary>
    /// Renders the toolbox.
    /// </summary>
    /// <param name="position">Area to render in</param>
    private void GUIToolBox(Rect position)
    {
        GUILayout.BeginArea(position, GUI.skin.box);
        int newTool = GUILayout.SelectionGrid(toolID, Tools.Select(tool => tool.Name).ToArray(), 1);

        if (toolID != newTool)
        {
            toolID = newTool;
            CurrentTool.OnEnable();
        }

        if (room)
        {
            EditorGUILayout.Space();

            CurrentTool.OnGUI();
        }

        GUILayout.EndArea();
    }

    /// <summary>
    /// Renders a preview of the room
    /// </summary>
    /// <param name="position"></param>
    private void GUIGrid(Rect position)
    {
        scrollPosition = GUI.BeginScrollView(position, scrollPosition, new Rect(0, 0, room.RealSize.x * gridSize, room.RealSize.y * gridSize));

        for (Position pos = new Position(); pos.x < room.RealSize.x; pos.x++)
        {
            for (pos.y = 0; pos.y < room.RealSize.y; pos.y++)
            {
                Rect rect = new Rect(pos.x * gridSize, (room.RealSize.y - 1 - pos.y) * gridSize, gridSize, gridSize);
                //GUI.DrawTextureWithTexCoords(rect, room.background.texture, GetTextureRect(room.background));

                if (CurrentTool.PreRenderGrid(pos, rect))
                {
                    Room.RoomPosition objData = room[pos];

                    if (objData.wallID == -1)
                    {
                        GUI.DrawTextureWithTexCoords(rect, room.platform.sprite.texture, GetTextureRect(room.platform.sprite));
                    }
                    else if (objData.wallID == -2)
                    {
                        GUI.DrawTextureWithTexCoords(rect, room.spike.sprite.texture, GetTextureRect(room.platform.sprite));
                    }
                    else if (objData.wallID != 0)
                    {
                        Room.Wall wall = room.walls[objData.wallID - 1];
                        int dir = room.GetWallDir(pos, objData.wallID);
                        Sprite sprite = objData.slope ? wall.GetSlope(dir) : wall[dir];
                        GUI.DrawTextureWithTexCoords(rect, sprite.texture, GetTextureRect(sprite));
                    }

                    if (room.spawners != null && room.spawners.Any(spawner => spawner.position == pos))
                    {
                        GUI.DrawTextureWithTexCoords(rect, QuestionMark, new Rect(0, 0, 1, 1));
                    }
                }

                CurrentTool.PostRenderGrid(pos, rect);
            }
        }

        GUI.EndScrollView();
    }

    public static Rect GetTextureRect(Sprite sprite)
    {
        return new Rect(sprite.rect.x / sprite.texture.width, sprite.rect.y / sprite.texture.height, sprite.rect.width / sprite.texture.width, sprite.rect.height / sprite.texture.height);
    }
}
