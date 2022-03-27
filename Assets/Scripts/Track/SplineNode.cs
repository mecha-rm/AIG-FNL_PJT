using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// a node in the track.
public class SplineNode : MonoBehaviour
{
    // the catmull rom spline this node is attached to.
    public CatmullRomSpline spline;

    // Start is called before the first frame update
    void Start()
    {
        // tries to get the component in the parent.
        if (spline == null)
            spline = GetComponentInParent<CatmullRomSpline>();
    }


    // gets the index of the node in the spline.
    public int GetIndexInSpline()
    {
        if (spline != null)
            return spline.IndexOfNode(this);
        else
            return -1;
    }
}
