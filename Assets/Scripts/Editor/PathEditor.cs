using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator creator;
    Path path
    {
        get {
            return creator.path;
        }
    }

    const float segmentSelectDistThreshhold = .1f;
    int selectedSegmentIndex = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        if(GUILayout.Button("Create new"))
        {
            Undo.RecordObject(creator, "Create new");
            creator.CreatePath();
        }

        bool isClosed = GUILayout.Toggle(path.IsClosed, "Closed");
        if(isClosed != path.IsClosed)
        {
            Undo.RecordObject(creator, "Toggle closed");
            path.IsClosed = isClosed;
        }

        bool autoSetControlPoints = GUILayout.Toggle(path.AutoSetControlPoints, "Auto Set Control Points");
        if(autoSetControlPoints != path.AutoSetControlPoints)
        {
            Undo.RecordObject(creator, "Toggle auto set controls");
            path.AutoSetControlPoints = autoSetControlPoints;
        }

        if(EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    [System.Obsolete]
    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        Event guiEvent = Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;

        if(guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            if(selectedSegmentIndex != -1)
            {
                Undo.RecordObject(creator, "Split segment");
                path.SplitSegment(mousePos, selectedSegmentIndex); 
            }
            else if(!path.IsClosed)
            {
                Undo.RecordObject(creator, "Add segment");
                path.AddSegment(mousePos);
            }
        }

        if(guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
        {
            float minDistToAnchor = creator.anchorDiameter * .5f;
            int closestAnchorIndex = -1;

            for (int i = 0; i < path.numPoints; i+=3)
            {
                float dist = Vector2.Distance(mousePos, path[i]);
                if(dist < minDistToAnchor)
                {
                    minDistToAnchor = dist;
                    closestAnchorIndex = i;
                }
            }

            if(closestAnchorIndex != -1)
            {
                Undo.RecordObject(creator, "Delete segment");
                path.DeleteSegment(closestAnchorIndex);
            }
        }

        if(guiEvent.type == EventType.MouseMove)
        {
            float minDistToSegment = segmentSelectDistThreshhold;
            int newSelectedSegmentIndex = -1;
            for (int i = 0; i < path.numSegments; i++)
            {
                Vector2[] points = path.GetPointsInSegment(i);
                float dist = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
                if(dist<minDistToSegment)
                {
                    minDistToSegment = dist;
                    newSelectedSegmentIndex = i;
                }
            }
            if(newSelectedSegmentIndex != selectedSegmentIndex)
            {
                selectedSegmentIndex = newSelectedSegmentIndex;
                HandleUtility.Repaint();
            }

        }  

        HandleUtility.AddDefaultControl(0);
    }

    [System.Obsolete]
    void Draw() //draw Bezier curve
    {
        for(int i = 0; i < path.numSegments; i++)
        {
            Vector2[] points = path.GetPointsInSegment(i);
            if(creator.displayControlPoints)
            {
                Handles.color = Color.black;
                Handles.DrawLine(points[1], points[0]);
                Handles.DrawLine(points[2], points[3]);
            }
            Color segmentCol = (i==selectedSegmentIndex && Event.current.shift) ? creator.selectedSegmentCol : creator.segmentCol;
            Handles.color = segmentCol;
            Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentCol, null, 2);
        }

        for (int i = 0; i < path.numPoints; i++)
        {
            if(i%3==0 || creator.displayControlPoints)
            {
                Handles.color = (i%3==0) ? creator.anchorCol : creator.controlCol;
                float handleSize = (i%3==0) ? creator.anchorDiameter : creator.controlDiameter;
                Vector2 newPos = Handles.FreeMoveHandle(path[i], Quaternion.identity, handleSize, Vector2.zero, Handles.CylinderHandleCap);

                if(path[i] != newPos)
                {
                    Undo.RecordObject(creator, "Move point");
                    path.MovePoint(i, newPos);
                }
            }
        }
    }

    void OnEnable()
    {
        creator = (PathCreator)target;
        if(creator.path == null)
        {
            creator.CreatePath();
        }
    }
}

