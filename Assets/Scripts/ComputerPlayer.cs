using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the script for the player that's controlled by the computer.
public class ComputerPlayer : Player
{
    [Header("Computer")]

    // if 'true', the computer player should be driving.
    public bool drive = true;

    // the node index being traveled to.
    // TODO: maybe you don't need this?
    // protected int destNodeIndex = -1;

    // node the player is heading towards.
    public SplineNode destNode;

    // if 'true', the computer will always face its target.
    // if 'false', the computer will need to rotate to face its target.
    public bool alwaysFaceTarget = false;

    // // threshold that must be passed for the computer to rotate towards the target.
    // public float rotationThreshold = 5.0F;
    // 
    // // threshold that must be passed for the computer to drift towards the target.
    // public float driftThreshold = 30.0F;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        // finds the gameplay manager.
        manager = FindObjectOfType<GameplayManager>();
    }

    // trigger collision entered.
    private void OnTriggerEnter(Collider other)
    {
        // collided with spline node.
        if(other.tag == "Spline Node")
        {
            // the spline node.
            SplineNode node;

            // tries to grab the node.
            if(other.TryGetComponent<SplineNode>(out node))
            {
                // checks index of the spline.
                // grabs the destination node.
                int destNodeIndex = node.spline.IndexOfNode(destNode);
                int nodeIndex = node.GetIndexInSpline();

                // if the index is not equal to -1.
                if(nodeIndex != -1)
                {
                    // destination reached.
                    // TODO: the player should not even miss one.
                    if(nodeIndex >= destNodeIndex)
                    {
                        // TODO: account for the zero case.
                        destNodeIndex = nodeIndex + 1;

                        // save the node.
                        destNode = node;

                        // reached the end of the list, so set to the beginning.
                        if (destNodeIndex >= node.spline.nodes.Count)
                            destNodeIndex = 0;

                        // grabs the next node.
                        destNode = node.spline.nodes[destNodeIndex];
                    }

                }
            }
        }
    }

    // checks which bound the value is closest to.
    public static int ClosestBound(int value, int bound1, int bound2)
    {
        // grabs the closest bound.
        float bound = ClosestBound((float)value, (float)bound1, (float)bound2);

        // returns result.
        return Mathf.RoundToInt(bound);

    }

    // checks which bound the value is closest to.
    // returns bound1 if equally close to both.
    public static float ClosestBound(float value, float bound1, float bound2)
    {
        // uses inverse lerp to find where the value is across the line.
        float t = Mathf.InverseLerp(bound1, bound2, value);

        // checks for the closest bound.
        // returns bound1 if equally spread apart.
        if(t <= 0.5F) // bound1 is closer.
        {
            return bound1;
        }
        else // bound2 is closer
        {
            return bound2;
        }

    }

    // returns the destination node index.
    public int GetDestinationNodeIndex()
    {
        // destination node not set.
        if (destNode == null)
            return -1;
        
        // gets the index.
        return manager.raceTrack.path.IndexOfNode(destNode);
    }

    // travels toward the node.
    public void TravelTowardsNode()
    {
        TravelTowardsNode(destNode, true);
    }

    // runs the computer's AI.
    public void TravelTowardsNode(SplineNode node, bool runSpline)
    {
        // distance between points.
        Vector3 dist = node.transform.position - transform.position;

        // checks if a point on the spline should be found.
        if(runSpline && node.spline != null)
        {
            // gets the node index.
            int nodeIndex = node.spline.IndexOfNode(node);

            // grabbed the node and checks that there are more than two points.
            if(nodeIndex >= 0 && node.spline.nodes.Count > 1)
            {
                // TODO: implement checks against the spline.

                dist = node.transform.position - transform.position;
            }
            else // node not in list, or spline only has one node.
            {
                dist = node.transform.position - transform.position;
            }

        }
        else
        {
            // go for the node directly.
            dist = node.transform.position - transform.position;
        }

        // old rotation value.
        // this always comes in positive.
        Vector3 oldRot = transform.eulerAngles;

        // the rotation to face the target.
        float faceAngle = 0.0F;

        // the rotation direction.
        float rotDirec = 0.0F;

        // changes the forward to face the camera.
        transform.forward = dist.normalized;

        // the rotation factor to face the target.
        faceAngle = transform.eulerAngles.y;

        // checks if the computer should rotate.
        if(Mathf.Abs(faceAngle) > 0.0F)
        {
            // if the computer should instantly face the target.
            if (alwaysFaceTarget)
            {
                // always face the entity.
                transform.eulerAngles = new Vector3(oldRot.x, transform.eulerAngles.y, oldRot.z); // keeps y-rotation (up-rotation)
            }
            else // gradual rotation
            {
                // the current angle (must be positive)
                float currAngle = oldRot.y;

                // make sure the current angle is the lowest it can be. 
                // can't use modulus since this is a float.
                if (currAngle >= 360.0F) // if the computer has made a positive rotation.
                {
                    // reduce amount while greater than 360 degrees.
                    while (currAngle >= 360.0F)
                    {
                        currAngle -= 360.0F;
                    }
                }
                else if (currAngle < 0.0F) // if the computer has made a negative rotation.
                {
                    // increase the amount while less than or equal to 0.
                    while (currAngle < 0.0F)
                    {
                        currAngle += 360.0F;
                    }
                }

                // reset to original rotation.
                oldRot.y = currAngle; // give positive rotation.
                transform.rotation = Quaternion.identity;
                transform.eulerAngles = oldRot; // set rotation.

                // // make sure the current angle is the lowest it can be. 
                // // can't use modulus since this is a float.
                // if (currAngle > 360.0F) // if the computer has made a full positive rotation.
                // {
                //     // reduce amount while greater than 360 degrees.
                //     while (currAngle > 360.0F)
                //     {
                //         currAngle -= 360.0F;
                //     }
                // }
                // else if (currAngle < -360.0F) // if the computer has made a full negative rotation.
                // {
                //     // increase the amount while less than -360
                //     while (currAngle < -360.0F)
                //     {
                //         currAngle += 360.0F;
                //     }
                // }
                
                // // POSITIVE AND NEGATIVE ANGLES //
                // 
                // // the positive and negative version to the angle.
                // float posFaceAngle = 0.0F;
                // float negFaceAngle = 0.0F;
                // 
                // // the positive and negative current angle.
                // // the current angle is always returned as positive, so we must find the negative.
                // float posCurrRot = 0.0F;
                // float negCurrRot = 0.0F;
                // 
                // // the minimum rotation.
                // float minRot = 0.0F;
                // 
                // // GETS POSITIVE AND NEGATIVE VERSIONS OF ANGLES
                // 
                // // checks if face theta is positive or negative to get both versions.
                // if (faceAngle > 0.0f) // positive rotation
                // {
                //     posFaceAngle = faceAngle;
                //     negFaceAngle = -360.0F + faceAngle;
                // }
                // else if (faceAngle < 0.0F) // negative rotation 
                // {
                //     negFaceAngle = faceAngle;
                //     posFaceAngle = 360.0F + faceAngle;
                // }
                // 
                // // checks if current angle is positive or negative.
                // if (currRot > 0.0f) // positive rotation
                // {
                //     posCurrRot = currRot;
                //     negCurrRot = -360.0F + currRot;
                // }
                // else if (currRot < 0.0F) // negative rotation 
                // {
                //     negCurrRot = currRot;
                //     posCurrRot = 360.0F + currRot;
                // }
                // 
                // /*
                //  * there are four combinations:
                //  *  - positive face angle and positive current angle
                //  *  - positive face angle and negative current angle
                //  *  - negative face angle and positive current angle
                //  *  - negative face angle and negative current angle
                //  *  
                //  *  You need to check the shortest path for all four.
                //  */
                // 
                // // finds the lowest rotation factor.
                // minRot = Mathf.Min(
                //     posFaceAngle - posCurrRot, // +f and +c
                //     posFaceAngle - negCurrRot, // +f and -c
                //     
                //     negFaceAngle - posCurrRot, // -f and +c
                //     negFaceAngle - negCurrRot // -f and -c
                //     );
                // 
                // // it faster to turn left or turn right?
                // rotDirec = (minRot > 0.0F) ? 1.0F : -1.0F;

                // Debug.Log("Face-Theta: " + faceTheta.ToString());

                // NEW //

                // // is it faster to rotate left or right?
                // // the positive and negative version to the face angle.
                // float posFaceAngle = 0.0F;
                // float negFaceAngle = 0.0F;
                // 
                // // checks if face theta is positive or negative to get both versions.
                // if (faceAngle > 0.0f) // positive rotation
                // {
                //     posFaceAngle = faceAngle;
                //     negFaceAngle = -360.0F + faceAngle;
                // }
                // else if (faceAngle < 0.0F) // negative rotation 
                // {
                //     negFaceAngle = faceAngle;
                //     posFaceAngle = 360.0F + faceAngle;
                // }
                // 
                // 
                // 
                // float posFaceAngleLooped = posFaceAngle + 360.0F;
                // float negFaceAngleLooped = negFaceAngle + 360.0F;


                // creates 2 bounds to see which one the computer is closest to.
                // it is the base angle and the angle after 1 complete loop.
                float bound1 = faceAngle;
                float bound2 = faceAngle + 360.0F;

                // checks what bound the face angle is closest to.
                float closestBound = ClosestBound(currAngle, bound1, bound2);

                // if the angle after 1 revolution is closer, use that.
                // that way it will go for the shortest path.
                if (closestBound == bound2) 
                {
                    faceAngle = bound2;
                }

                // sets the rotation direction (always goes the same direction)
                rotDirec = (faceAngle > currAngle) ? 1.0F : -1.0F;
            }
        }

        // rotates in the direction of the entity.
        // Rotate((theta - oldRot.y > 0.0F) ? 1.0F : -1.0F);
        // Debug.Log("Rotation Direction: " + (theta - oldRot.y).ToString());

        // turn in the shortest direction.

        // moves in the direction of the node.
        // Action(dist.normalized.x, dist.normalized.z, false);

        // horizontal is for rotating, vertical is for accelerating.
        Action(rotDirec, 1.0F, false);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        // COMPUTER INPUT FOR MOVEMENT.

        // do not drive.
        if (!drive)
            return;

        // no destination node set.
        if(destNode == null)
        {
            // grabs node 0.
            destNode = manager.raceTrack.path.nodes[0];
            // destNodeIndex = 0;
        }

        // travel towards the destination node.
        TravelTowardsNode();


    }
}
