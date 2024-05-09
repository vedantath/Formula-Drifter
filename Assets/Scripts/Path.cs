using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path
{
    [SerializeField, HideInInspector]
    List<Vector2> points;
    [SerializeField, HideInInspector]
    bool isClosed;
    [SerializeField, HideInInspector]
    bool autoSetControlPoints;

    public Path(Vector2 center)
    {
        points = new List<Vector2>
        {
            center+Vector2.left,
            center+(Vector2.left+Vector2.up)*.5f,
            center + (Vector2.right+Vector2.down)*.5f,
            center + Vector2.right
        };
    }

    public Vector2 this[int i]
    {
        get {
            return points[i];
        }
    }

    public bool IsClosed
    {
        get {
            return isClosed;
        }
        set 
        {
            if(isClosed != value)
            {
                isClosed = value;
                if(isClosed)
                {
                    points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
                    points.Add(points[0] * 2 - points[1]);
                    if(autoSetControlPoints)
                    {
                        AutoSetAnchorControlPoints(0);
                        AutoSetAnchorControlPoints(points.Count-3);
                    }
                }
                else 
                {
                    points.RemoveRange(points.Count-2, 2);
                    if(autoSetControlPoints)
                    {
                        AutoSetStartAndEndControls();
                    }
                }
            }
        }
    }

    public bool AutoSetControlPoints
    {
        get {
            return autoSetControlPoints;
        }
        set {
            if(autoSetControlPoints != value)
            {
                autoSetControlPoints = value;
                if(autoSetControlPoints)
                {
                    AutoSetAllControlPoints();
                }
            }
        }
    }

    public int numPoints
    {
        get {
            return points.Count;
        }
    }

    public int numSegments
    {
        get {
            return points.Count/3;
        }
    }

    public void AddSegment(Vector2 anchorPos)
    {
        points.Add(points[points.Count - 1] * 2 - points[points.Count - 2]);
        points.Add((points[points.Count - 1] + anchorPos) * .5f);
        points.Add(anchorPos);

        if(autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(points.Count-1);
        }
    }

    public void SplitSegment(Vector2 anchorPos, int segmentIndex)
    {
        points.InsertRange(segmentIndex*3+2, new Vector2[] { Vector2.zero, anchorPos, Vector2.zero });
        if(autoSetControlPoints)
        {
            AutoSetAllAffectedControlPoints(segmentIndex*3+3);
        }
        else
        {
            AutoSetAnchorControlPoints(segmentIndex*3+3);
        }
    }

    public void DeleteSegment(int anchorIndex)
    {
        if(numSegments > 2 || !isClosed && numSegments > 1)
        {
            if(anchorIndex==0)
            {
                if(isClosed)
                {
                    points[points.Count-1] = points[2];
                }
                points.RemoveRange(0,3);
            }
            else if(anchorIndex==points.Count-1 && !isClosed)
            {
                points.RemoveRange(anchorIndex-2, 3);
            }
            else {
                points.RemoveRange(anchorIndex-1, 3);
            }
        }
    }

    public Vector2[] GetPointsInSegment(int i)
    {
        return new Vector2[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[LoopIndex(i * 3 + 3)] };

    }

    public void MovePoint(int i, Vector2 pos)
    {
        Vector2 deltaMove = pos - points[i];

        if(i%3==0 || !autoSetControlPoints)
        {
            points[i] = pos;

            if(autoSetControlPoints) 
            {
                AutoSetAllAffectedControlPoints(i);
            }
            else
            {

            //anchor points at index multiples of 3 in array
                if(i%3 ==0)
                {
                    if(i+1 < points.Count || isClosed)
                        points[LoopIndex(i+1)] += deltaMove;
                    if(i-1 >= 0 || isClosed)
                        points[LoopIndex(i-1)] += deltaMove;

                }
                else { //control points
                    bool nextPointIsAnchor = (i+1)%3 == 0;
                    int correspondingControlIndex = (nextPointIsAnchor) ? i+2 : i-2;
                    int anchorIndex = (nextPointIsAnchor) ? i+1 : i-1;

                    if(correspondingControlIndex >= 0 && correspondingControlIndex < points.Count || isClosed)
                    {
                        float dist = (points[LoopIndex(anchorIndex)] - points[LoopIndex(correspondingControlIndex)]).magnitude;
                        Vector2 dir = (points[LoopIndex(anchorIndex)] - pos).normalized;
                        points[LoopIndex(correspondingControlIndex)] = points[LoopIndex(anchorIndex)] + dir * dist;
                    }
                }
            }
        }
    }

    public Vector2[] CalculateEvenlySpacedPoints(float spacing, float resolution = 1)
    {
        List<Vector2> evenlySpacedPts = new List<Vector2>();
        evenlySpacedPts.Add(points[0]);
        Vector2 prevPoint = points[0];
        float distLastEvenPt = 0;

        for (int i = 0; i < numSegments; i++)
        {
            Vector2[] p = GetPointsInSegment(i);
            float controlNetLength = Vector2.Distance(p[0], p[1]) + Vector2.Distance(p[1], p[2]) + Vector2.Distance(p[2], p[3]);
            float estCurveLength = Vector2.Distance(p[0], p[3]) + controlNetLength/2;
            int divisions = Mathf.CeilToInt(estCurveLength * resolution * 10);
            float t = 0;
            while (t<=1)
            {
                t+=1f/divisions;
                Vector2 ptOnCurve = Bezier.EvaluateCubic(p[0], p[1], p[2], p[3], t);
                distLastEvenPt += Vector2.Distance(prevPoint, ptOnCurve);

                while (distLastEvenPt >= spacing)
                {
                    float overshootDist = distLastEvenPt-spacing;
                    Vector2 newEvenlySpacedPt = ptOnCurve + (prevPoint-ptOnCurve).normalized * overshootDist;
                    evenlySpacedPts.Add(newEvenlySpacedPt);
                    distLastEvenPt = overshootDist;
                    prevPoint = newEvenlySpacedPt;
                }

                prevPoint = ptOnCurve;
            }
        }
        return evenlySpacedPts.ToArray();
    }

    void AutoSetAllAffectedControlPoints(int updatedAnchorIndex)
    {
        for (int i = updatedAnchorIndex-3; i <= updatedAnchorIndex+3; i+=3)
        {
            if(i>=0 && i<points.Count || isClosed)
            {
                AutoSetAnchorControlPoints(LoopIndex(i));
            }
        }
        AutoSetStartAndEndControls();
    }

    void AutoSetAllControlPoints()
    {
        for (int i = 0; i < points.Count; i+=3)
        {
            AutoSetAnchorControlPoints(i);
        }
        AutoSetStartAndEndControls();
    }

    void AutoSetAnchorControlPoints(int anchorIndex)
    {
        Vector2 anchorPos = points[anchorIndex];
        Vector2 dir = Vector2.zero;
        float[] neighborDistances = new float[2];

        if(anchorIndex-3>=0 || isClosed)
        {
            Vector2 offset = points[LoopIndex(anchorIndex-3)] - anchorPos;
            dir += offset.normalized;
            neighborDistances[0] = offset.magnitude;
        }
        if(anchorIndex+3>=0 || isClosed)
        {
            Vector2 offset = points[LoopIndex(anchorIndex+3)] - anchorPos;
            dir -= offset.normalized;
            neighborDistances[1] = -offset.magnitude;
        }
        dir.Normalize();

        for(int i = 0; i<2; i++)
        {
            int controlIndex = anchorIndex + i * 2 - 1;
            if(controlIndex >= 0 && controlIndex < points.Count || isClosed)
            {
                points[LoopIndex(controlIndex)] = anchorPos + dir * neighborDistances[i] * .5f;

            }
        }

    }

    void AutoSetStartAndEndControls()
    {
        if(!isClosed)
        {
            points[1] = (points[0] + points[2]) * .5f;
            points[points.Count-2] = (points[points.Count-1] + points[points.Count-3]) * .5f;
        }
    }

    int LoopIndex(int i)
    {
        return (i+points.Count) % points.Count;
    }

}
