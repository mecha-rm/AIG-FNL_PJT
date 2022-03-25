using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: test to make sure the matrix multiplications work properly.
// a spline along the track, which uses catmull-rom.
public class TrackSpline : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // does the catmull rom equation.
    public static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float u)
    {
        // the catmull-rom matrix, which has a 0.5F scalar applied from the start.
        Matrix4x4 matCatmullRom = new Matrix4x4();

        // setting the rows
        matCatmullRom.SetRow(0, new Vector4(0.5F * -1.0F, 0.5F * 3.0F, 0.5F * -3.0F, 0.5F * 1.0F));
        matCatmullRom.SetRow(1, new Vector4(0.5F * 2.0F, 0.5F * -5.0F, 0.5F * 4.0F, 0.5F * -1.0F));
        matCatmullRom.SetRow(2, new Vector4(0.5F * -1.0F, 0.5F * 0.0F, 0.5F * 1.0F, 0.5F * 0.0F));
        matCatmullRom.SetRow(3, new Vector4(0.5F * 0.0F, 0.5F * 2.0F, 0.5F * 0.0F, 0.5F * 0.0F));


        // Points
        Matrix4x4 pointsMat = new Matrix4x4();

        pointsMat.SetRow(0, new Vector4(p0.x, p0.y, p0.z, 0));
        pointsMat.SetRow(1, new Vector4(p1.x, p1.y, p1.z, 0));
        pointsMat.SetRow(2, new Vector4(p2.x, p2.y, p2.z, 0));
        pointsMat.SetRow(3, new Vector4(p3.x, p3.y, p3.z, 0));


        // matrix for u to the power of given functions.
        Matrix4x4 uMat = new Matrix4x4(); // the matrix for 'u' (also called 't').

        // setting the 'u' values to the proper row, since this is being used as a 1 X 4 matrix.
        uMat.SetRow(0, new Vector4(Mathf.Pow(u, 3), Mathf.Pow(u, 2), Mathf.Pow(u, 1), Mathf.Pow(u, 0)));

        // result matrix from a calculation. 
        Matrix4x4 result;

        // order of [u^3, u^2, u, 0] * M * <points matrix>
        // the catmull-rom matrix has already had the (1/2) scalar applied.
        result = matCatmullRom * pointsMat;

        result = uMat * result; // [u^3, u^2, u, 0] * (M * points)

        // the resulting values are stored at the top.
        return result.GetRow(0);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
