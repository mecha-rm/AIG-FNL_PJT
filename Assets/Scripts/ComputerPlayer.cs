using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Q-value storing object/reference table
public class QValueStore
{
    // reference table, size # of states by # of actions ((edit for accuracy, and figure out if it starts with all cells as 0/figure out how/what to initialize as))
    public static int states = 4;
    public static int actions = 4;
    float[,] Qtable = new float[states,actions];

    // retrieve Q-value from reference table
    public float getQValue(int state, int action)
    {
        return Qtable[state,action];
    }

    // determine best action for a given state
    public int getBestAction(int state)
    {
        int bestAction = 0;
        for (int x = 0; x < actions; x++)
        {
            if (Qtable[state,bestAction] < Qtable[state, x])
            {
                bestAction = x;
            }
        }
        return bestAction;
    }

    // store Q-value into reference table
    public void storeQValue(int state, int action, float value)
    {
        Qtable[state, action] = value;
    }
}

public class ReinforcementProblem
{
    // Choose a random starting state for the problem.
    public int getRandomState()
    {
        return Random.Range(0,QValueStore.states);
    }
    
    // Get the available actions for the given state.
    public int[] getAvailableActions(int state)
    {
        //edit: if manually setting impossible actions to -1, then change this to not include -1 actions
        int[] temp = new int[QValueStore.actions];
        for (int x = 0; x < QValueStore.actions; x++)
        {
            temp[x] = x;
        }
        return temp;
    }
    
    // Take the given action and state, and return
    // a pair consisting of the reward and the new state. ((edit))
    public int[] takeAction(int state, int action) 
    {
        return new int[] { 0, 0 };
    }
}

// this is the script for the player that's controlled by the computer.
public class ComputerPlayer : Player
{
    //edit should be in the cs file that ReinforcementProblem is
    public QValueStore AI1QTable = new QValueStore();
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
        Vector3 oldRotEulers = transform.eulerAngles;

        // the rotation to face the target.
        float faceAngle = 0.0F;

        // the rotation direction.
        float rotDirec = 0.0F;

        // changes the forward to face the camera.
        transform.forward = dist.normalized;

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
                Vector3 nodeDist = dist;
                Vector3 result;

                // y-values should stay the same.
                nodeDist.y = comForward.y;

                // the maximum radians.
                float step = rotationRate * Mathf.Deg2Rad * Time.deltaTime;

                // rotate toards the value.
                result = Vector3.RotateTowards(comForward, nodeDist,
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

    // Updates the AI's Q-table by investigating the problem iterations amount of times.
    // ((edit this line)) problem -> problem (this should be changed to refer to wherever ReinforcementProblem's functions are)
    // learnRate -> feedback value influence on stored Q-value (0-1) ((delete:try transition from 0.7-0.3))
    // discountRate -> how much the next state's Q-value affects the current action (0-1) ((delete: try 0.75))
    // exploreChance -> how often random action is taken (0-1) ((delete:try 0.2))
    // nu (edit rename later) -> chance for car to start a new chain of states and actions (0-1) ((delete:try 0))
    // edit/delete, figure out how to tell the AI that it's reached the end/figure out how to calculate reward
    public void QLearning(ReinforcementProblem problem, int iterations, float learnRate,
                          float discountRate, float exploreChance, float nu)
    {
        // Get a starting state.
        int state = problem.getRandomState();
        
        // Repeat a number of times.
        for (int x = 0; x < iterations; x++)
        {
            // Pick a new state every once in a while.
            if (Random.Range(0f, 1f) < nu)
                state = problem.getRandomState();

            // Get the list of available actions.
            int[] temp = problem.getAvailableActions(state);
            int action;
            // Should we use a random action this time?
            if (Random.Range(0f, 1f) < exploreChance)
            {
                action = temp[Random.Range(0,temp.Length)];
            }
            else
            {
                // Otherwise pick the best action.
                action = AI1QTable.getBestAction(state);
            }
            // Carry out the action and retrieve the reward and new state.
            int[] temp2 = problem.takeAction(state, action);
            int reward = temp2[0];
            int newState = temp2[1];

            // Get the current q from the reference table.
            float Q = AI1QTable.getQValue(state, action);

            // Get the q of the best action from the new state.
            float maxQ = AI1QTable.getQValue(newState, AI1QTable.getBestAction(newState));

            // Perform the q learning.
            Q = (1 - learnRate) * Q + learnRate * (reward + discountRate * maxQ);

            // Store the new Q-value.
            AI1QTable.storeQValue(state, action, Q);

            // And update the state.
            state = newState;
        }
    }
}







