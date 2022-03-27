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
        // This is now handled by the spline this is added to.
        // // tries to get the component in the parent.
        // if (spline == null)
        //     spline = GetComponentInParent<CatmullRomSpline>();
    }


    // gets the index of the node in the spline.
    // if this node is in the list twice (which it shouldn't be) it will return the first instance of it.
    public int GetIndexInSpline()
    {
        if (spline != null)
            return spline.IndexOfNode(this);
        else
            return -1;
    }
}
