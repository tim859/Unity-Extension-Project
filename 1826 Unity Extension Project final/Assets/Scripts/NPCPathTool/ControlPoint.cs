/* 
Disclaimer: The creation of some parts of this class were inspired by the second answer to this stack overflow post
(https://stackoverflow.com/questions/50788120/how-to-make-individual-anchor-points-of-bezier-continuous-or-non-continuous)
*/

using System;
using UnityEngine;

[Serializable]
public class ControlPoint // not inheriting from monobehaviour so that Unity won't complain when this class is directly instantiated
{
    [SerializeField] Vector3 position;
    [SerializeField] Vector3 backTangent;
    [SerializeField] Vector3 frontTangent;

    public ControlPoint(Vector3 initialPosition) // constructor for ControlPoint class, control points always require a position point,
                                                 // a front tangent point and a back tangent point
    {
        position = initialPosition;
        frontTangent = Vector3.one;
        backTangent = -Vector3.one;
    }

    // the majority of this classes methods are simply getters and setters for the different properties
    public Vector3 GetPosition()
    {
        return position;
    }

    public void SetPosition(Vector3 newPos)
    {
        position = newPos;
    }

    public Vector3 GetBackTangent()
    {
        return backTangent;
    }

    public void SetBackTangent(Vector3 newBackTangent)
    {
        backTangent = newBackTangent;
    }

    public Vector3 GetFrontTangent()
    {
        return frontTangent;
    }

    public void SetFrontTangent(Vector3 newFrontTangent)
    {
        frontTangent = newFrontTangent;
    }
}
