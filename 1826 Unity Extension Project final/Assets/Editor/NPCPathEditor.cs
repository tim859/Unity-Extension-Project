/* 
Disclaimer: The creation of some parts of this class were inspired by the second answer to this stack overflow post
(https://stackoverflow.com/questions/50788120/how-to-make-individual-anchor-points-of-bezier-continuous-or-non-continuous)
*/

// side note just to be clear:
// position point refers to the red handle used to change the position of a control point
// tangent point refers to the blue handle used to change the two tangent points for a position point
// control point refers to all three of these linked together and moving together

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NPCPath))]
public class NPCPathEditor : Editor
{
    Path path;
    SerializedProperty property;

    // variables defined here so i dont have to keep typing this stuff out
    Vector3 snap = Vector3.one * 0.5f;
    Quaternion rotation = Quaternion.identity;
    readonly Handles.CapFunction sphereCap = Handles.SphereHandleCap;
    const float positionHandleSize = 0.6f;
    const float tangentHandleSize = 0.4f;

    void OnSceneGUI()
    {
        NPCPath npcPath = (NPCPath)target; // casting the component script

        Event e = Event.current; // assigning the current event to e (seems to be the convention when dealing with Event.current)

        if (npcPath.closeLoop) // deals with having a closed loop or not
        {
            path.SetLoop(true);
        }
        else
        {
            path.SetLoop(false);
        }

        for (int i = 0; i < path.GetNumOfControlPoints(); i++) // loops through each control point in the list of control points that path holds
        {
            // handles new position of the control/position point handle

            Vector3 controlPointPosition = path.points[i].GetPosition(); // getting the current control point in the path

            if (npcPath.topDown) // sets y value of controlPointPosition to 0 if the user has selected top down mode
            {
                controlPointPosition = new Vector3(controlPointPosition.x, 0, controlPointPosition.z);
            }

            Handles.color = Color.red; // position point handles are red

            Vector3 newPosition = Handles.FreeMoveHandle(controlPointPosition, rotation, positionHandleSize, snap, sphereCap); // calculates the new position point handle position based on user input

            if (e.shift) // functionality for adding control points, hold shift and left click on an existing control point to add a new control point at that point in the path
            {
                if (Handles.Button(controlPointPosition, rotation, positionHandleSize, positionHandleSize, sphereCap))
                {
                    path.InsertPoint(i, controlPointPosition + new Vector3(positionHandleSize, 0, 0));
                }
            }

            if (e.control) // functionality for deleting control points, hold ctrl and left click on a control point to delete that specific control point
            {
                if (Handles.Button(controlPointPosition, rotation, positionHandleSize, positionHandleSize, sphereCap))
                {
                    if (path.GetNumOfControlPoints() <= 2) // don't want there to be any less than 2 control points, otherwise the code might start to misbehave
                    {
                        Debug.Log("Cannot delete any more control points, there must be at least 2.");
                    }
                    else
                    {
                        path.RemovePoint(i);
                    }
                }
            }

            if (controlPointPosition != newPosition) // if the position of the control point has not changed, we don't want to do unecessary operations
            {
                path.MovePositionPoint(i, newPosition); // updates the position point in the path as well
                controlPointPosition = newPosition; // required for the tangent point calculations below
            }


            // handles new position of the back tangent handle

            Vector3 backTangent = controlPointPosition + path.points[i].GetBackTangent(); // ensuring that the back tangent point will be moved with its linked control point

            if (npcPath.topDown) // sets y value of backTangent to 0 if the user has selected top down mode
            {
                backTangent = new Vector3(backTangent.x, 0, backTangent.z);
            }

            Handles.color = Color.black; // the lines between the position point and the tangent points are black

            Handles.DrawLine(controlPointPosition, backTangent); // draws the line between the position point and the tangent point

            Handles.color = Color.blue; // tangent handles are blue

            Vector3 newBackTangent = Handles.FreeMoveHandle(backTangent, rotation, tangentHandleSize, snap, sphereCap); // calculates the new back tangent handle position based on user input

            if (backTangent != newBackTangent) // if the position of the control point has not changed, we don't want to do unecessary operations
            {
                path.MoveBackTangent(i, newBackTangent - controlPointPosition);
            }


            // handles new position of the front tangent handle

            Vector3 frontTangent = controlPointPosition + path.points[i].GetFrontTangent(); // ensuring that the front tangent point will be moved with its linked control point

            if (npcPath.topDown) // sets y value of frontTangent to 0 if the user has selected top down mode
            {
                frontTangent = new Vector3(frontTangent.x, 0, frontTangent.z);
            }

            Handles.color = Color.black; // the lines between the position point and the tangent points are black

            Handles.DrawLine(controlPointPosition, frontTangent); // draws the line between the position point and the tangent point

            Handles.color = Color.blue; // tangent point handles are blue

            Vector3 newFrontTangent = Handles.FreeMoveHandle(frontTangent, rotation, tangentHandleSize, snap, sphereCap); // calculates the new front tangent handle position based on user input

            if (frontTangent != newFrontTangent) // if the position of the control point has not changed, we don't want to do unecessary operations
            {
                path.MoveFrontTangent(i, newFrontTangent - controlPointPosition);
            }
        }

        // drawing the bezier curves
        for (int i = 0; i < path.GetNumOfSegments(); i++)
        {
            Vector3[] points = path.GetBezierPointsInSegment(i);

            if (npcPath.topDown) // sets y values of the bezier points to 0 if the user has selected top down mode
            {
                points[0] = new Vector3(points[0].x, 0, points[0].z);
                points[1] = new Vector3(points[1].x, 0, points[1].z);
                points[2] = new Vector3(points[2].x, 0, points[2].z);
                points[3] = new Vector3(points[3].x, 0, points[3].z);
            }

            Handles.DrawBezier(points[0], points[3], points[1], points[2], Color.green, null, 2);
        }
    }

    private void OnEnable()
    {
        NPCPath npcPath = (NPCPath)target;

        path = npcPath.path ?? npcPath.CreatePath(); // i would like to expand this so its easier to read but when I do, the npc path stops being serialized and is lost after being deselected

        property = serializedObject.FindProperty("path"); // ensures that the path is serialized and therefore retained after deselection
    }
}
