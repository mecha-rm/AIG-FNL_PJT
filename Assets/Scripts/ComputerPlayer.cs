using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is the script for the player that's controlled by the computer.
public class ComputerPlayer : Player
{
    [Header("Computer")]


    // the node index being traveled to.
    protected int destNodeIndex = -1;

    // node the player is heading towards.
    protected SplineNode destNode;

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
                int nodeIndex = node.GetIndexInSpline();

                // if the index is not equal to -1.
                if(nodeIndex != -1)
                {
                    // destination reached.
                    // TODO: the player should not even miss one.
                    if(nodeIndex >= destNodeIndex)
                    {
                        destNodeIndex++;

                        // save the node.
                        destNode = node;

                        // reached the end of the list, so set to the beginning.
                        if (destNodeIndex >= node.spline.nodes.Count)
                            destNodeIndex = 0;
                    }

                }
            }
        }
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        // COMPUTER INPUT FOR MOVEMENT.


    }
}
