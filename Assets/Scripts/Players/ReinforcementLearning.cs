using UnityEngine;

public class ReinforcementLearning : MonoBehaviour
{
    // reference table, size # of states by # of actions ((edit for accuracy, and figure out if it starts with all cells as 0/figure out how/what to initialize as))
    // number of states
    public int states = 4;
    
    // number of actions
    public int actions = 4;

    // reference table for the number of states by the number of actions.
    float[,] qTable;

    // Start is called before the first frame update
    void Start()
    {
        qTable = new float[states, actions];
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
        for (int x = 0; x < actions; x++)
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

    // Choose a random starting state for the problem.
    public int GetRandomState()
    {
        return Random.Range(0, states);
    }

    // Get the available actions for the given state.
    public int[] GetAvailableActions(int state)
    {
        //edit: if manually setting impossible actions to -1, then change this to not include -1 actions
        int[] temp = new int[actions];

        for (int x = 0; x < actions; x++)
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
        int state = GetRandomState();
         
        // Repeat a number of times.
        for (int x = 0; x < iterations; x++)
        {
            // Pick a new state every once in a while.
            if (Random.Range(0f, 1f) < nu)
                state = GetRandomState();

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
