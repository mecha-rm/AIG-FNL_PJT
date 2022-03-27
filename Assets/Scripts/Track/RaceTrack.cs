using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the track
public class RaceTrack : MonoBehaviour
{
    // the track path.
    public CatmullRomSpline path;

    // Start is called before the first frame update
    void Start()
    {
        // checks itself for a spline.
        if (path == null)
            path = GetComponent<CatmullRomSpline>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
