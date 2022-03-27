using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the race track script.
public class RaceTrack : MonoBehaviour
{
    // the track path.
    public CatmullRomSpline path;

    // Start is called before the first frame update
    void Start()
    {
        // checks for a race path.
        if (path == null)
            path = FindObjectOfType<CatmullRomSpline>(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
