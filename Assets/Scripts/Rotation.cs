using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a rotation class. This code was originally written as part of another project one of the teammembers worked on.
public class Rotation : MonoBehaviour
{
    // euler rotation (2D)
    public static Vector2 RotateEuler(Vector2 v, float angle, bool inDegrees)
    {
        // re-uses rotation calculation for 3D with z = 0.
        Vector3 result = RotateEuler(new Vector3(v.x, v.y, 0.0F), Vector3.forward, angle, inDegrees);
        return new Vector2(result.x, result.y);
    }

    // euler rotation (3D)
    private static Vector3 RotateEuler(Vector3 v, Vector3 axis, float angle, bool inDegrees)
    {
        // angle conversion
        float radAngle = (inDegrees) ? Mathf.Deg2Rad * angle : angle;

        // sin and cos angle
        float sinAngle = Mathf.Sin(radAngle);
        float cosAngle = Mathf.Cos(radAngle);

        // the three rows (set to identity by default)
        Vector3 r0 = Vector3.one;
        Vector3 r1 = Vector3.one;
        Vector3 r2 = Vector3.one;

        // set rotation values
        if (axis == Vector3.right) // x-axis
        {
            r0 = new Vector3(1.0F, 0.0F, 0.0F);
            r1 = new Vector3(0.0f, cosAngle, -sinAngle);
            r2 = new Vector3(0.0F, sinAngle, cosAngle);
        }
        else if (axis == Vector3.up) // y-axis
        {
            r0 = new Vector3(cosAngle, 0.0F, sinAngle);
            r1 = new Vector3(0.0f, 1.0F, 0.0F);
            r2 = new Vector3(-sinAngle, 0.0F, cosAngle);
        }
        else if (axis == Vector3.forward) // z-axis
        {
            r0 = new Vector3(cosAngle, -sinAngle, 0.0F);
            r1 = new Vector3(sinAngle, cosAngle, 0.0F);
            r2 = new Vector3(0.0F, 0.0F, 1.0F);
        }

        // calculation (modelled after matrix multiplication)
        Vector3 result = Vector3.zero;
        result.x = Vector3.Dot(r0, v);
        result.y = Vector3.Dot(r1, v);
        result.z = Vector3.Dot(r2, v);

        return result;
    }

    // rotates along the x-axis
    public static Vector3 RotateEulerX(Vector3 v, float angle, bool inDegrees)
    {
        return RotateEuler(v, Vector3.right, angle, inDegrees);
    }

    // rotates along the y-axis
    public static Vector3 RotateEulerY(Vector3 v, float angle, bool inDegrees)
    {
        return RotateEuler(v, Vector3.up, angle, inDegrees);
    }

    // rotates along the z-axis
    public static Vector3 RotateEulerZ(Vector3 v, float angle, bool inDegrees)
    {
        return RotateEuler(v, Vector3.forward, angle, inDegrees);
    }

    // TRIANGLE ANGLE //

    // consolidation function.
    // [1] = soh, [2] = cah, [3] = toa
    private float TriangleAngleBetween(int trigFunc, float value1, float value2, bool returnInDegrees)
    {
        // the angle
        float angle = 0;


        // checks what trig function to use.
        // all functions return the angle in radians.
        switch (trigFunc)
        {
            // soh
            case 1:
                // sin a = opp / hyp
                angle = Mathf.Asin(value1 / value2);
                break;

            // cah
            case 2:
                // cos a = adj / hyp
                angle = Mathf.Acos(value1 / value2);
                break;

            // toa
            case 3:
                // tan a = opp / adj
                angle = Mathf.Atan(value1 / value2);
                break;

            // could not discern function. This should never be reached.                
            default:
                Debug.LogError("Improper value provided. Cannot discern function.");
                return 0;
        }

        // if the result should be retuend in degrees.
        if (returnInDegrees)
            angle = Mathf.Rad2Deg * angle;

        // returns the angle.
        return angle;
    }

    // SINE - SOH//
    // value version
    public float TriangleAngleBetweenOppositeHypotenuse(float opp, float hyp, bool returnInDegrees)
    {
        return TriangleAngleBetween(1, opp, hyp, returnInDegrees);
    }

    // COSINE - CAH //
    // CAH
    public float AngleBetweenAdjacentHypotenuse(float adj, float hyp, bool returnInDegrees)
    {
        return TriangleAngleBetween(2, adj, hyp, returnInDegrees);
    }

    // TANGENT - TOA //
    // TOA 
    public float AngleBetweenOppositeAdjacent(float opp, float adj, bool returnInDegrees)
    {
        return TriangleAngleBetween(3, opp, adj, returnInDegrees);
    }
}
