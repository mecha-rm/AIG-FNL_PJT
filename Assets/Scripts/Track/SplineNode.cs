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

    // Update is called once per frame
    void Update()
    {
        
    }
}
