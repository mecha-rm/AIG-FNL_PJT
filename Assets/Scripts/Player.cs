using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the script for the player object.
public class Player : MonoBehaviour
{
    // the player's rigidbody.
    public new Rigidbody rigidbody;

    // force is applied in the forward direction of the object.
    public float rotationRate = 90.0F;

    // if 'true', force is applied.
    public bool applyForce = true;

    // the base force applied to move the player.
    // public float baseForce = 10.0F;

    // the direction of travel.
    protected Vector3 forceDirec = Vector3.one;

    // the multiplied force for moving the player.
    public Vector3 forcePower = new Vector3(20.0F, 20.0F, 20.0F);

    // caps the maximum velocity.
    // TODO: set to a proper value.
    public float maxVelocity = 1000.0F;

    // if 'true', the player is drifting.
    public bool drifting;

    // the drift factor.
    private float driftInc = 10.0F;

    // the drift angle for the player.
    public float driftAngle = 0.0F;

    // the maximum drift angle.
    private const float driftMaxAngle = 60.0F;

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
    }

    // Update is called once per frame
    protected void Update()
    {
        // applies force for movement.
        if(applyForce)
        {
            // // rotate the player
            // Vector3 newRot = transform.rotation.eulerAngles; // angles
            // newRot.y = driftAngle; // new angle
            // transform.rotation = Quaternion.identity; // base rotation
            // transform.Rotate(newRot); // new rotation

            // calculates the force.
            // TODO: adjust the drifting.

            // the final force to be applied.
            Vector3 force;
            force = Vector3.Scale(transform.forward, forceDirec); // direction
            force.Scale(forcePower); // multiplication

            // applies the force.
            rigidbody.AddForce(force);

            // TODO: have room for rotations.
        }

        // if the player is drifting. 
        if (drifting)
        {
            driftAngle += driftInc * Time.deltaTime;
        }
        // not drifting anymore.
        else if (!drifting && driftAngle > 0.0F)
        {
            driftAngle -= driftInc * Time.deltaTime;

            // no longer drifting.
            if (driftAngle < 0.0F)
                driftAngle = 0.0F;
        }

        // clamps the velocity.
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxVelocity);

    }
}
