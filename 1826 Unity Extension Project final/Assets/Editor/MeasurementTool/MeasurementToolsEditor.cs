using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeasurementTools))]
public class MeasurementToolsEditor : Editor
{
    Vector3 snap = Vector3.one * 0.5f;
    public static int measurementToolType = 0; // 0 = nothing, 1 = distanceXY, 2 = distanceXZ, 3 = distanceZY

    void OnSceneGUI()
    {

        switch (measurementToolType)
        {
            case 0:
                break;

            case 1:
                DistanceXYAxis();
                break;

            case 2:
                DistanceXZAxis();
                break;

            case 3:
                DistanceYZAxis();
                break;
        }
    }

    void DistanceXYAxis()
    {
        MeasurementTools mt = (MeasurementTools)target; // casting the component script

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.black; // black is much easier to see in the scene view than the default grey

        // the value (in meters) that will be shown to the user is calculated here. it is composed of the distance between the x and y points of the distance handle tool
        float distance = Mathf.Abs(mt.distancePoint1Pos.x - mt.distancePoint2Pos.x) + Mathf.Abs(mt.distancePoint1Pos.y - mt.distancePoint2Pos.y);

        // average of the distance between the x values of the point handles
        float pointHandleXAvg = (mt.distancePoint1Pos.x + mt.distancePoint2Pos.x) / 2;

        // average of the distance between the y values of the point handles
        float pointHandleYAvg = (mt.distancePoint1Pos.y + mt.distancePoint2Pos.y) / 2; 

        // creates the label with the distance value in it
        Handles.Label(new Vector3(pointHandleXAvg, pointHandleYAvg, mt.distanceCenterPos.z), distance.ToString("F2") + "m", labelStyle);

        Handles.color = Color.red;
        // draws the dotted line between the two points
        Handles.DrawDottedLine(new Vector3(mt.distancePoint1Pos.x, mt.distancePoint1Pos.y, mt.distanceCenterPos.z), new Vector3(mt.distancePoint2Pos.x, mt.distancePoint2Pos.y, mt.distanceCenterPos.z), mt.dashSize);

        Handles.color = Color.blue;

        EditorGUI.BeginChangeCheck();

        // calculates new position of the point1 handle based on its movement
        Vector3 newDistancePoint1Pos = Handles.FreeMoveHandle(new Vector3(mt.distancePoint1Pos.x, 
            mt.distancePoint1Pos.y, mt.distanceCenterPos.z), Quaternion.identity, mt.distancePointSize, snap, Handles.CubeHandleCap);

        // calculates new position of the point2 handle based on its movement
        Vector3 newDistancePoint2Pos = Handles.FreeMoveHandle(new Vector3(mt.distancePoint2Pos.x, 
            mt.distancePoint2Pos.y, mt.distanceCenterPos.z), Quaternion.identity, mt.distancePointSize, snap, Handles.CubeHandleCap);

        // calculates new position of the center handle based on its movement
        Vector3 newDistanceCenterPos = Handles.Slider(new Vector3((mt.distancePoint1Pos.x + mt.distancePoint2Pos.x) / 2, 
            (mt.distancePoint1Pos.y + mt.distancePoint2Pos.y) / 2, mt.distanceCenterPos.z), Vector3.forward);

        if (EditorGUI.EndChangeCheck())
        {
            mt.distancePoint1Pos = newDistancePoint1Pos;
            mt.distancePoint2Pos = newDistancePoint2Pos;
            mt.distanceCenterPos = newDistanceCenterPos;
            EditorUtility.SetDirty(target);
            // changes the position of the handles to the new positions that were calculated above
        }
    }

    void DistanceXZAxis()
    {
        MeasurementTools mt = (MeasurementTools)target; // casting the component script

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.black; // black is much easier to see in the scene view than the default grey

        // the value (in meters) that will be shown to the user is calculated here. it is composed of the distance between the x and z points of the distance handle tool
        float distance = Mathf.Abs(mt.distancePoint1Pos.x - mt.distancePoint2Pos.x) + Mathf.Abs(mt.distancePoint1Pos.z - mt.distancePoint2Pos.z);

        // average of the distance between the x values of the point handles
        float pointHandleXAvg = (mt.distancePoint1Pos.x + mt.distancePoint2Pos.x) / 2;

        // average of the distance between the z values of the point handles
        float pointHandleZAvg = (mt.distancePoint1Pos.z + mt.distancePoint2Pos.z) / 2;

        // creates the label with the distance value in it
        Handles.Label(new Vector3(pointHandleXAvg, mt.distanceCenterPos.y, pointHandleZAvg), distance.ToString("F2") + "m", labelStyle);

        Handles.color = Color.red;
        // draws the dotted line between the two points
        Handles.DrawDottedLine(new Vector3(mt.distancePoint1Pos.x, mt.distanceCenterPos.y, mt.distancePoint1Pos.z), new Vector3(mt.distancePoint2Pos.x, mt.distanceCenterPos.y, mt.distancePoint2Pos.z), mt.dashSize);

        Handles.color = Color.blue;

        EditorGUI.BeginChangeCheck();

        // calculates new position of the point1 handle based on its movement
        Vector3 newDistancePoint1Pos = Handles.FreeMoveHandle(new Vector3(mt.distancePoint1Pos.x, mt.distanceCenterPos.y, mt.distancePoint1Pos.z), Quaternion.identity, mt.distancePointSize, snap, Handles.CubeHandleCap);

        // calculates new position of the point2 handle based on its movement
        Vector3 newDistancePoint2Pos = Handles.FreeMoveHandle(new Vector3(mt.distancePoint2Pos.x, mt.distanceCenterPos.y, mt.distancePoint2Pos.z), Quaternion.identity, mt.distancePointSize, snap, Handles.CubeHandleCap);

        // calculates new position of the center handle based on its movement
        Vector3 newDistanceCenterPos = Handles.Slider(new Vector3((mt.distancePoint1Pos.x + mt.distancePoint2Pos.x) / 2, mt.distanceCenterPos.y, (mt.distancePoint1Pos.z + mt.distancePoint2Pos.z) / 2), Vector3.up);

        if (EditorGUI.EndChangeCheck())
        {
            mt.distancePoint1Pos = newDistancePoint1Pos;
            mt.distancePoint2Pos = newDistancePoint2Pos;
            mt.distanceCenterPos = newDistanceCenterPos;
            EditorUtility.SetDirty(target);
            // changes the position of the handles to the new positions that were calculated above
        }
    }

    void DistanceYZAxis()
    {
        MeasurementTools mt = (MeasurementTools)target; // casting the component script

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.normal.textColor = Color.black; // black is much easier to see in the scene view than the default grey

        // the value (in meters) that will be shown to the user is calculated here. it is composed of the distance between the y and z points of the distance handle tool
        float distance = Mathf.Abs(mt.distancePoint1Pos.y - mt.distancePoint2Pos.y) + Mathf.Abs(mt.distancePoint1Pos.z - mt.distancePoint2Pos.z);

        // average of the distance between the y values of the point handles
        float pointHandleYAvg = (mt.distancePoint1Pos.y + mt.distancePoint2Pos.y) / 2;

        // average of the distance between the z values of the point handles
        float pointHandleZAvg = (mt.distancePoint1Pos.z + mt.distancePoint2Pos.z) / 2;

        // creates the label with the distance value in it
        Handles.Label(new Vector3(mt.distanceCenterPos.x, pointHandleYAvg, pointHandleZAvg), distance.ToString("F2") + "m", labelStyle);

        Handles.color = Color.red;
        // draws the dotted line between the two points
        Handles.DrawDottedLine(new Vector3(mt.distanceCenterPos.x, mt.distancePoint1Pos.y, mt.distancePoint1Pos.z), new Vector3(mt.distanceCenterPos.x, mt.distancePoint2Pos.y, mt.distancePoint2Pos.z), mt.dashSize);

        Handles.color = Color.blue;

        EditorGUI.BeginChangeCheck();

        // calculates new position of the point1 handle based on its movement
        Vector3 newDistancePoint1Pos = Handles.FreeMoveHandle(new Vector3(mt.distanceCenterPos.x, mt.distancePoint1Pos.y, mt.distancePoint1Pos.z), Quaternion.identity, mt.distancePointSize, snap, Handles.CubeHandleCap);

        // calculates new position of the point2 handle based on its movement
        Vector3 newDistancePoint2Pos = Handles.FreeMoveHandle(new Vector3(mt.distanceCenterPos.x, mt.distancePoint2Pos.y, mt.distancePoint2Pos.z), Quaternion.identity, mt.distancePointSize, snap, Handles.CubeHandleCap);

        // calculates new position of the center handle based on its movement
        Vector3 newDistanceCenterPos = Handles.Slider(new Vector3(mt.distanceCenterPos.x, (mt.distancePoint1Pos.y + mt.distancePoint2Pos.y) / 2, (mt.distancePoint1Pos.z + mt.distancePoint2Pos.z) / 2), Vector3.right);

        if (EditorGUI.EndChangeCheck())
        {
            mt.distancePoint1Pos = newDistancePoint1Pos;
            mt.distancePoint2Pos = newDistancePoint2Pos;
            mt.distanceCenterPos = newDistanceCenterPos;
            EditorUtility.SetDirty(target);
            // changes the position of the handles to the new positions that were calculated above
        }
    }
}
