using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the script for the player that's controlled by the computer.
public class ComputerPlayer : Player
{
    [Header("Computer")]

    // the reinforcement learning AI.
    public ReinforcementLearning learningAI;

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

    // if 'true', the computer tries to follow the spline.
    public bool followSpline = true;

    // // threshold that must be passed for the computer to rotate towards the target.
    // public float rotationThreshold = 5.0F;
    // 
    // // threshold that must be passed for the computer to drift towards the target.
    // public float driftThreshold = 30.0F;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        // if the AI is not set.
        if(learningAI == null)
        {
            // grabs the component.
            if(!TryGetComponent<ReinforcementLearning>(out learningAI))
            {
                // adds the component.
                learningAI = gameObject.AddComponent<ReinforcementLearning>();
            }
        }
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

    // gets a position on the spline to follow based on the user's distance from the node..
    public Vector3 GetSplineTarget(SplineNode target, float offset)
    {
        // the spline.
        CatmullRomSpline spline;

        // spline not set.
        if(target.spline == null)
        {
            Debug.LogAssertion("This node isn't attached to a spline.");
            return target.transform.position;
        }
        else
        {
            spline = target.spline;
        }

        // checks if a spline target can be generated.
        if(spline.nodes.Count < 2)
        {
            Debug.LogAssertion("Not enough nodes to get a spline target.");
            return target.transform.position;
        }

        // does not contain the node.
        if(!spline.ContainsNode(target))
        {
            Debug.LogAssertion("Node is not in the list.");
            return target.transform.position;
        }

        // gets the node's index.
        int startIndex = spline.IndexOfNode(target);
        startIndex = (startIndex - 1 < 0) ? spline.nodes.Count - 1 : startIndex - 1;

        // grabs the starting node.
        SplineNode startNode = spline.nodes[startIndex];

        // Vector3.Distance is (a-b).magnitude
        float t = Mathf.InverseLerp(
            0.0F,
            Vector3.Distance(target.transform.position, startNode.transform.position),
            Vector3.Distance(target.transform.position, transform.position));

        // add the offset to the t-value.
        t += offset;

        // checks against the t-value to see if it's worth running the interpolation.
        if(t < 0.0F || t >= 1.0F)
        {
            // less than 0, so just go for the ending target's position.
            // or greater than 1, so just go for the target position.
            return target.transform.position;
        }
        else
        {
            // returns the new target. 
            Vector3 newTarget = spline.Interpolate(startIndex, t);

            Debug.Log("T: " + t.ToString() + " | NewTarget: " + newTarget.ToString());

            return newTarget;
        }

    }

    // travels toward the node.
    public void TravelTowardsNode()
    {
        TravelTowardsNode(destNode, followSpline);
    }

    // runs the computer's AI.
    public void TravelTowardsNode(SplineNode node, bool runSpline)
    {
        // distance between points.
        Vector3 nodeDist = node.transform.position - transform.position;

        // checks if a point on the spline should be found.
        if(runSpline && node.spline != null)
        {
            // gets the node index.
            int nodeIndex = node.spline.IndexOfNode(node);

            // gets the new target.
            Vector3 newTarget = GetSplineTarget(node, 0.1F);

            // target is the node.
            if(newTarget == node.transform.position)
            {
                nodeDist = node.transform.position - transform.position;
            }
            else // target is something different.
            {
                // this is now a shortened version of the node distance.
                nodeDist = newTarget - transform.position;
            }

        }
        else
        {
            // go for the node directly.
            nodeDist = node.transform.position - transform.position;
        }

        // old rotation value.
        // this always comes in positive.
        Vector3 oldRotEulers = transform.eulerAngles;

        // the rotation to face the target.
        float faceAngle = 0.0F;

        // the rotation direction.
        float rotDirec = 0.0F;

        // changes the forward to face the camera.
        transform.forward = nodeDist.normalized;

        // the rotation factor to face the target.
        faceAngle = transform.eulerAngles.y;

        // return to original
        transform.eulerAngles = oldRotEulers;

        // checks if the computer should rotate.
        if (Mathf.Abs(faceAngle) > 0.0F)
        {
            // if the computer should instantly face the target.
            if (alwaysFaceTarget)
            {
                // always face the entity.
                transform.eulerAngles = new Vector3(oldRotEulers.x, transform.eulerAngles.y, oldRotEulers.z); // keeps y-rotation (up-rotation)
            }
            else // gradual rotation
            {
                // how to use RotateTowards: https://docs.unity3d.com/ScriptReference/Vector3.RotateTowards.html

                // gets the two vectors.
                Vector3 comForward = transform.forward;
                Vector3 targetDist = nodeDist;
                Vector3 result;

                // y-values should stay the same.
                targetDist.y = comForward.y;

                // the maximum radians.
                float step = rotationRate * Mathf.Deg2Rad * Time.deltaTime;

                // rotate toards the value.
                result = Vector3.RotateTowards(comForward, targetDist,
                    step, 0.0F);

                // change forward transform.
                result.y = transform.forward.y;
                transform.forward = result;

            }
        }

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







