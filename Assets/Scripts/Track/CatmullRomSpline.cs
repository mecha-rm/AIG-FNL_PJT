using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: test to make sure the matrix multiplications work properly.
// a spline along the track, which uses catmull-rom.
public class CatmullRomSpline : MonoBehaviour
{
    // the track nodes.
    public List<SplineNode> nodes = new List<SplineNode>();

    // if 'true' child nodes are added.
    public bool addChildNodes = true;

    // the line variables.
    [Header("Line")]

    // the line renderer for the track spline.
    public LineRenderer lineRenderer;

    // the amount of line segments between points.
    public int lineSegments = 5;

    // the width of the line.
    public float lineWidth = 1.0F;

    // if 'true', the line should be drawn on start.
    public bool drawLineOnStart = true;

    // if 'true', this script controls the width of the line.
    public bool changeLineWidth = true;

    // if 'true', the line is shown.
    public bool showLine = true;

    // Start is called before the first frame update
    void Start()
    {
        // no nodes set, so find the components in the children.
        if(addChildNodes)
        {
            // new list
            List<SplineNode> list = new List<SplineNode>();

            // gets the components.
            GetComponentsInChildren<SplineNode>(list);

            // adds the list.
            nodes.AddRange(list);
        }

        // adds a line renderer
        if (lineRenderer == null)
        {
            // tries to get the component, adding a new one if not set.
            if(!TryGetComponent<LineRenderer>(out lineRenderer))
                lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
            

        // draws the line.
        if (drawLineOnStart)
            DrawLine();

        // should the line be shown?
        lineRenderer.enabled = showLine;
        
    }

    // finds a node.
    public SplineNode FindNode(int index)
    {
        // grabs and returns a nide.
        if (index < 0 || index >= nodes.Count)
            return null;
        else
            return nodes[index];
    }

    // gets the first node.
    public SplineNode GetFirstNode()
    {
        // check if nodes available.
        if (nodes.Count == 0)
            return null;
        else
            return nodes[0];
    }

    // gets the last node.
    public SplineNode GetLastNode()
    {
        // check if nodes available.
        if (nodes.Count == 0)
            return null;
        else
            return nodes[nodes.Count - 1];
    }

    // gets the index of the node.
    public int IndexOfNode(SplineNode node)
    {
        return nodes.IndexOf(node);
    }

    // checks if a list contains a node.
    public bool ContainsNode(SplineNode node)
    {
        return nodes.Contains(node);
    }

    // does the catmull rom equation.
    public static Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
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
        uMat.SetRow(0, new Vector4(Mathf.Pow(t, 3), Mathf.Pow(t, 2), Mathf.Pow(t, 1), Mathf.Pow(t, 0)));

        // result matrix from a calculation. 
        Matrix4x4 result;

        // order of [u^3, u^2, u, 0] * M * <points matrix>
        // the catmull-rom matrix has already had the (1/2) scalar applied.
        result = matCatmullRom * pointsMat;

        result = uMat * result; // [u^3, u^2, u, 0] * (M * points)

        // the resulting values are stored at the top.
        return result.GetRow(0);
    }

    // runs hte catmull rom interpolation using the starting node index with a t value.
    public Vector3 RunInterpolation(int startNodeIndex, float time)
    {
        // index out of bounds, so position of 0 sent back.
        if (startNodeIndex < 0 || startNodeIndex >= nodes.Count)
        {
            Debug.LogAssertion("Index out of bounds.");
            return Vector3.zero;
        }

        // not enough nodes.
        if(nodes.Count < 2)
        {
            Debug.LogAssertion("Not enough nodes to run spline.");
            return Vector3.zero;
        }

        // points
        Vector3 p0, p1, p2, p3;

        // indexes
        int i0, i1, i2, i3;

        // grabs the values.
        
        // P0 //
        // 1 less than startNodeIndex
        i0 = startNodeIndex - 1;

        // loop around to the end.
        if (i0 < 0)
            i0 = nodes.Count - 1;

        // grabs the point.
        p0 = nodes[i0].transform.position;

        // P1 //
        // just the value of startNodeIndex
        i1 = startNodeIndex;
        p1 = nodes[i1].transform.position;

        // P2 //
        // 1 more than startNodeIndex
        i2 = startNodeIndex + 1;

        // loops around to the start.
        if (i2 >= nodes.Count)
            i2 = 0;

        // grabs the position.
        p2 = nodes[i2].transform.position;

        // P3 //
        // 1 more than p2's index.
        i3 = i2 + 1; // startNodeIndex + 2

        // loops around to 0.
        if(i3 >= nodes.Count)
            i3 = 0;

        // grabs the position.
        p3 = nodes[i3].transform.position;


        // gets the result.
        return CatmullRom(p0, p1, p2, p3, time);
    }


    // draws a line along the path.
    public void DrawLine()
    {
        // not enough nodes.
        if (nodes.Count <= 1)
        {
            Debug.LogAssertion("You need 2 or more nodes to draw a line.");
            return;
        }

        // no line segments set.
        if(lineSegments <= 0)
        {
            Debug.LogAssertion("Variable lineSegments must be greater than 0.");
            return;
        }

        // sets the line widths
        if(changeLineWidth)
        {
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
        }  

        // total amount of line segments to draw.
        int totalSegments = nodes.Count * lineSegments;

        // the node index.
        // starts at -1 since it gets set to 0 at the start of the loop.
        int nodeIndex = -1;

        // when the amount of drawn lines is equal to the amount of lineSegments...
        // it is time to move onto the next node.

        // set position count.
        lineRenderer.positionCount = totalSegments; // total segments
        lineRenderer.loop = true; // loop around to the end automatically.

        // drawing the segments.
        for(int i = 0; i < lineRenderer.positionCount; i++)
        {
            // the t-value [0, 1]
            int remainder = i % lineSegments;

            float t = (float)remainder / lineSegments;

            // is it time to switch nodes?
            if (remainder == 0)
                nodeIndex++;

            // sets the position of the node.
            lineRenderer.SetPosition(i, RunInterpolation(nodeIndex, t));
        }

    }

    // Update is called once per frame
    void Update()
    {
        // for showing/hiding the line.
        lineRenderer.enabled = showLine;
    }
}
