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

        // perform an action.
        Action(
            Input.GetAxisRaw("Horizontal"), // rotate
            Input.GetAxisRaw("Vertical"), // accelerate
            Input.GetAxisRaw("Jump") != 0.0F // jump (drifting)
            ); 
    }
}
