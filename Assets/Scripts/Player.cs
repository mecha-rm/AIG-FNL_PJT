using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the script for the player object.
public class Player : MonoBehaviour
{
    // the player's rigidbody.
    public new Rigidbody rigidbody;

    // force is applied in the forward direction of the object.
    public float rotationRate = 60.0F;

    // if 'true', force is applied.
    public bool applyForce = true;

    // the action direction (positive or negative)
    // (x) is for drifting.
    // (y) is unused.
    // (z) is for acceleration (applying force).
    protected Vector3 actionDirec = Vector3.zero;

    // the multiplied force for moving the player.
    public float forcePower = 20.0F;

    // caps the maximum velocity.
    public float maxVelocity = 50.0F;

    [Header("Drifting")]

    // if 'true', the player is drifting.
    public bool drifting;

    // the drift factor.
    public float driftInc = 50.0F;

    // the drift angle for the player.
    public float driftAngle = 0.0F;

    // the maximum drift angle.
    public float driftMaxAngle = 90.0F;

    // Start is called before the first frame update
    protected void Start()
    {
        // grabs the rigidbody if this is not set.
        if (rigidbody == null)
            rigidbody = GetComponent<Rigidbody>();

    }

    // rotates the entity. If 'right' is true, you turn right. If false, you turn left.
    // you turn at thes speed of the rotation rate.
    public void Rotate(float direc)
    {
        // rotation.
        float theta = direc * rotationRate * Time.deltaTime;

        // rotates around the y-axis.
        transform.Rotate(Vector3.up, theta);

        // // rigidbody has velocity.
        // if(rigidbody.velocity != Vector3.zero)
        // {
        //     // gets the magnitude.
        //     float vMag = rigidbody.velocity.magnitude;
        // 
        //     // gets and rotates the normalized vector.
        //     Vector3 vRot = Vector3.forward; // gets normalized vector.
        //     vRot = Rotation.RotateEulerY(vRot, transform.rotation.eulerAngles.y, true); // rotates
        //     vRot *= vMag; // multiplies by the vector magnitude.
        // 
        //     // change rigidbody.
        //     rigidbody.velocity = vRot;
        // }
        
    }

    // called to perform player actions
    // horizontal concerns the horizontal axis (rotating)
    // vertical concerns the vertical axis (acceleration)
    // drift concerns drifting.
    public void Action(float horizontal, float vertical, bool drift)
    {
        // rotates through
        if (horizontal != 0)
        {
            // applying action on the horizontal axis.
            actionDirec.x = horizontal;
            Rotate(actionDirec.x);
        }
        else
        {
            actionDirec.x = 0.0F;
        }


        // applying force
        if (vertical != 0)
        {
            // going to apply force.
            applyForce = true;

            // the vertical axis (1 or -1)
            float vertAxis = vertical;

            // moving on the forward axis.
            actionDirec.z = vertAxis;

        }
        else
        {
            // no longer applying force.
            applyForce = false;

            // reset to zero.
            actionDirec.z = 0.0F;
        }

        // player drift functions.
        drifting = drift;
    }

    // Update is called once per frame
    protected void Update()
    {
        // applies force for movement.
        if (applyForce)
        {
            // the final force to be applied.
            // since z is the forward axis, only the z-axis component is considered.
            Vector3 force = transform.forward * actionDirec.z * forcePower;

            // applies the force.
            rigidbody.AddForce(force);
        }

        // if the player is drifting (only happens when accelerating)
        if (drifting && applyForce && actionDirec.z != 0)
        {
            // the new drift angle.
            float newDriftAngle = driftAngle + driftInc * actionDirec.x * Time.deltaTime;
            newDriftAngle = Mathf.Clamp(newDriftAngle, -driftMaxAngle, +driftMaxAngle);

            // the player's (y) rotation
            float playerRotY = transform.eulerAngles.y;
            playerRotY -= driftAngle; // subtracts drift angle from it.
            playerRotY += newDriftAngle; // adds new drift angle.


            // the new rotation factor.
            Vector3 newRot = new Vector3(
                transform.eulerAngles.x,
                playerRotY,
                transform.eulerAngles.z
                );

            // sets the new player rotation.
            transform.eulerAngles = newRot;

            // sets this to the new drift angle.
            driftAngle = newDriftAngle;

        }
        // not drifting anymore.
        else if (!drifting && Mathf.Abs(driftAngle) > 0.0F)
        {
            // the subtraction direction.
            // (1) means you subtract, (-1) means you add.
            int direc = (driftAngle > 0.0F) ? 1 : -1;

            // the new drift angle.
            float newDriftAngle = driftAngle - driftInc * direc * Time.deltaTime;

            // clamping - checks the direction.
            switch (direc)
            {
                case 1: // subtracted (positive rotation, check for less than 0)
                    if(newDriftAngle < 0.0F)
                        newDriftAngle = 0.0f;
                    break;

                case -1: // added (negative rotation, check for more than 0).
                    if (newDriftAngle > 0.0F)
                        newDriftAngle = 0.0f;
                    break;
            }

            // the player's (y) rotation
            float playerRotY = transform.eulerAngles.y;
            playerRotY -= driftAngle; // subtracts drift angle from it.
            playerRotY += newDriftAngle; // boosts it by current drift angle.


            // the new rotation factor.
            Vector3 newRot = new Vector3(
                transform.eulerAngles.x,
                playerRotY,
                transform.eulerAngles.z
                );

            // sets the new player rotation.
            transform.eulerAngles = newRot;

            // sets this to the new drift angle.
            driftAngle = newDriftAngle;
        }

        // clamps the velocity.
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxVelocity);

    }
}
