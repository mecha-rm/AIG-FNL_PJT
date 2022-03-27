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







