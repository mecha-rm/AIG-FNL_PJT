using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the script for the player that's controlled by the user.
public class UserPlayer : Player
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        // rotates through
        if (Input.GetAxisRaw("Horizontal") != 0)
            Rotate(Input.GetAxisRaw("Horizontal"));

        // applying force
        if(Input.GetAxisRaw("Vertical") != 0)
        {
            // going to apply force.
            applyForce = true;
        
        
            // the horizontal axis
            // float horiAxis = Input.GetAxisRaw("Horizontal");
        
            // the vertical axis.
            float vertAxis = Input.GetAxisRaw("Vertical");
        
            // applying a horizontal force.
            // if (horiAxis != 0.0F)
            //     direcForce.x = horiAxis; // side axis
        
            // applying a vertical force.
            if (vertAxis != 0.0F)
                forceDirec.z = vertAxis; // forward axis.
        
        }
        else
        {
            // no longer applying force.
            applyForce = false;
        
            // reset to base.
            forceDirec = Vector3.zero;
        }
    }
}
