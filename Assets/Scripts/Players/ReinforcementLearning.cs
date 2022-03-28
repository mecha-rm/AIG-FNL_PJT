using UnityEngine;

public class ReinforcementLearning : MonoBehaviour
{
    // reference table, size # of states by # of actions ((edit for accuracy, and figure out if it starts with all cells as 0/figure out how/what to initialize as))
    
    // list of actions to be performed.
    /*
     * list of actions to be performed.
     *  - stopped: not moving (no input)
     *  - backward: moving backward
     *  - forward: moving forward
     *  - turning: turning some direction.
     *  - drifting: drifting (turning while drifting).
     */

    // the states.
    public enum state { stopped, backward, forward, turning, drifting };

    // notably, const variabes are already static.
    // the amount of states.
    public const int STATE_COUNT = 5;
    
    // list of actions
    /*
     * accel: accelerate to speed up or continue at the same speed.
     * decel: decelerate to slow down and go backwards or stop.
     * rotate: start rotating to initiate a turn.
     * drift: initiate a drift to go into a drifting state.
     */
    public enum action {accel, decel, rotate, drift};

    // the number of actions.
    public const int ACTION_COUNT = 4;

    // reference table for the number of states by the number of actions.
    float[,] qTable;

    // Start is called before the first frame update
    void Start()
    {
        qTable = new float[STATE_COUNT, ACTION_COUNT];
    }

    // retrieve Q-value from reference table
    public float GetQValue(int state, int action)
    {
        return qTable[state, action];
    }

    // determine best action for a given state
    public int GetBestAction(int state)
    {
        int bestAction = 0;
        for (int x = 0; x < ACTION_COUNT; x++)
        {
            if (qTable[state, bestAction] < qTable[state, x])
            {
                bestAction = x;
            }
        }
        return bestAction;
    }

    // store Q-value into reference table
    public void StoreQValue(int state, int action, float value)
    {
        qTable[state, action] = value;
    }

    // choose a random starting state for the problem.
    // returns a random state.
    public state GetRandomState()
    {
        return (state)Random.Range(0, STATE_COUNT);
    }

    // choose a random starting state for the problem.
    // return a random state as an int
    public int GetRandomStateAsInt()
    {
        return Random.Range(0, STATE_COUNT);
    }


    // Get the available actions for the given state.
    public int[] GetAvailableActions(int state)
    {
        //edit: if manually setting impossible actions to -1, then change this to not include -1 actions
        int[] temp = new int[ACTION_COUNT];

        for (int x = 0; x < ACTION_COUNT; x++)
        {
            temp[x] = x;
        }

        return temp;
    }

    // Take the given action and state, and return
    // a pair consisting of the reward and the new state. ((edit))
    public int[] TakeAction(int state, int action)
    {
        return new int[] { 0, 0 };
    }

    // Updates the AI's Q-table by investigating the problem iterations amount of times.
    // ((edit this line)) problem -> problem (this should be changed to refer to wherever ReinforcementProblem's functions are)
    // learnRate -> feedback value influence on stored Q-value (0-1) ((delete:try transition from 0.7-0.3))
    // discountRate -> how much the next state's Q-value affects the current action (0-1) ((delete: try 0.75))
    // exploreChance -> how often random action is taken (0-1) ((delete:try 0.2))
    // nu (edit rename later) -> chance for car to start a new chain of states and actions (0-1) ((delete:try 0))
    // edit/delete, figure out how to tell the AI that it's reached the end/figure out how to calculate reward
    public void QLearning(int iterations, float learnRate, float discountRate, float exploreChance, float nu)
    {
        // Get a starting state.
        int state = GetRandomStateAsInt();
         
        // Repeat a number of times.
        for (int x = 0; x < iterations; x++)
        {
            // Pick a new state every once in a while.
            if (Random.Range(0f, 1f) < nu)
                state = GetRandomStateAsInt();

            // Get the list of available actions.
            int[] temp = GetAvailableActions(state);
            int action;
            // Should we use a random action this time?
            if (Random.Range(0f, 1f) < exploreChance)
            {
                action = temp[Random.Range(0, temp.Length)];
            }
            else
            {
                // Otherwise pick the best action.
                action = GetBestAction(state);
            }
            // Carry out the action and retrieve the reward and new state.
            int[] temp2 = TakeAction(state, action);
            int reward = temp2[0];
            int newState = temp2[1];

            // Get the current q from the reference table.
            float Q = GetQValue(state, action);

            // Get the q of the best action from the new state.
            float maxQ = GetQValue(newState, GetBestAction(newState));

            // Perform the q learning.
            Q = (1 - learnRate) * Q + learnRate * (reward + discountRate * maxQ);

            // Store the new Q-value.
            StoreQValue(state, action, Q);

            // And update the state.
            state = newState;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
