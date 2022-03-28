using UnityEngine;

// the script used for reinforcement learning.
public class ReinforcementLearning : MonoBehaviour
{
    // action
    public struct UpdateAction
    {
        // if 'true', this update action is usable.
        public bool use;

        // horizontal value
        public float horizontal;

        // vertical value
        public float vertical;

        // should be drifting.
        public bool drift;

        // the remaining distance.
        public float distance;
    }

    // reference table, size # of states by # of actions ((edit for accuracy, and figure out if it starts with all cells as 0/figure out how/what to initialize as))
    // the state rewards are shorter times to reach the next node. 

    // list of actions to be performed.
    /*
     * list of actions to be performed.
     *  - stopped: not moving (no input)
     *  - spinning: turning in place.
     *  - forward: moving forward
     *  - backward: moving backward
     *  - turning: turning some direction.
     *  - drifting: drifting (turning while drifting).
     */

    // the states.
    public enum state { stopped, spinning, forward, backward, turning, drifting };

    // notably, const variabes are already static.
    // the amount of states.
    public const int STATE_COUNT = 6;
    
    // list of actions
    /*
     * accel: accelerate to speed up or continue at the same speed.
     * decel: decelerate to slow down and go backwards or stop.
     * rotate: start rotating to initiate a turn.
     * drift: initiate a drift to go into a drifting state.
     */
    public enum stateAction {stay, spin, accel, decel, turn, drift};

    // the number of actions.
    public const int STATE_ACTION_COUNT = 6;

    // the computer player this learning AI belongs to.
    public ComputerPlayer computer;

    // the acton to be taken.
    private UpdateAction nextAction;

    // becomes true when a new action is available. False when the action is grabbed.
    private bool newAction = false;

    // the amount of iterations for a action simulation
    public int simIterations = 5;

    // the refresh rate of the AI. Refreshes on 0.
    public float refreshTimer = 0.0F;

    // refreshes.
    public float refreshTimerMax = 2.5F;

    // reference table for the number of states by the number of actions.
    float[,] qualityTable;

    // Start is called before the first frame update
    void Start()
    {
        // this is handled by the computer script.
        // // grabs the computer AI.
        // if (computer == null)
        // {
        //     computer = GetComponent<ComputerPlayer>();
        // } 

        qualityTable = new float[STATE_COUNT, STATE_ACTION_COUNT];
    }

    // gets the next action.
    public UpdateAction NextAction
    {
        get
        {
            newAction = false; // no longer a new action.
            return nextAction;
        }
    }

    // returns 'true' if this is a new action.
    public bool NewAction
    {
        get
        {
            return newAction;
        }
    }

    // retrieve quality value from reference table
    public float GetQualityValue(int state, int action)
    {
        return qualityTable[state, action];
    }

    // determine best action for a given state
    public int GetBestAction(int state)
    {
        int bestAction = 0;
        for (int x = 0; x < STATE_ACTION_COUNT; x++)
        {
            if (qualityTable[state, bestAction] < qualityTable[state, x])
            {
                bestAction = x;
            }
        }
        return bestAction;
    }

    // store Q-value into reference table
    public void StoreQualityValue(int state, int action, float value)
    {
        qualityTable[state, action] = value;
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
        int[] temp = new int[STATE_ACTION_COUNT];

        // fills array.
        for (int x = 0; x < STATE_ACTION_COUNT; x++)
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
            float Q = GetQualityValue(state, action);

            // Get the q of the best action from the new state.
            float maxQ = GetQualityValue(newState, GetBestAction(newState));

            // Perform the q learning.
            Q = (1 - learnRate) * Q + learnRate * (reward + discountRate * maxQ);

            // Store the new Q-value.
            StoreQualityValue(state, action, Q);

            // And update the state.
            state = newState;
        }
    }

    // SPLIT //
    // returns the current state.
    public state GetCurrentState()
    {
        // the current state.
        state currState = 0;

        // checks the current state of the computer.
        if(computer.rigidbody.velocity != Vector3.zero) // moving
        {
            // checks for direction.
            if (computer.drifting) // the computer is drifting.
            {
                currState = state.drifting;
            }
            else if (computer.rotating) // the computer is rotating.
            {
                currState = state.turning;
            }
            else if (computer.rigidbody.velocity.z > 0) // going forward
            {
                currState = state.forward;
            }
            else if (computer.rigidbody.velocity.z < 0) // going backwards
            {
                currState = state.backward;
            }

        } // not moving
        else
        {
            // checks if turning, drifting, or stopped.
            if (computer.rotating)
                currState = state.spinning;

            else
                currState = state.stopped; // not moving
        }

        return currState;
    }


    // evaluates a state and an action.
    // this applies the function to the computer, so make sure to back this content up.
    private UpdateAction EvaluatePolicy(state state, stateAction stateAction)
    {
        // the update action.
        UpdateAction updateAction = new UpdateAction();

        // checks the state.
        switch(stateAction)
        {
            case stateAction.stay: // don't move
                updateAction.vertical = 0.0F;
                break;

            case stateAction.spin: // spin in place

                // if the computer does not always face the target, rotate towards the node.
                if (!computer.alwaysFaceTarget)
                    computer.RotateTowardsNodeDistance();
                break;

            case stateAction.accel: // move forward
                updateAction.vertical = 1.0F;
                break;

            case stateAction.decel: // back ward.
                updateAction.vertical = -1.0F;
                break;

            case stateAction.turn: // turn
                computer.drifting = false;
                if (!computer.alwaysFaceTarget)
                    computer.RotateTowardsNodeDistance();
                break;

            case stateAction.drift: // drift
                updateAction.drift = true;
                break;
        }

        // has the computer perform the action.
        computer.Action(updateAction.horizontal, updateAction.vertical, updateAction.drift);
        computer.PerformDrift();
        updateAction.use = true;
        updateAction.distance = computer.GetDistanceFromDestinationNode(); // resulting distance.

        // returns the distance from the node.
        return updateAction;
    }

    // determines what action to take.
    public void DetermineNextAction(int iterations)
    {
        // iterations out of bounds.
        if (iterations < 1)
        {
            // will not be used.
            nextAction = new UpdateAction();
            return;
        }

        // each trial corresponds with a different state option.
        UpdateAction[,] trials = new UpdateAction[STATE_COUNT, iterations];

        // grabs the current state.
        state currState = GetCurrentState();

        // will reset the transformation values at the end.
        Vector3 origPos = computer.transform.position;
        Quaternion origRot = computer.transform.rotation;
        Vector3 origVel = computer.rigidbody.velocity;
        bool drifting = computer.drifting;

        // determines states
        for (int s = 0; s < trials.GetLength(0); s++) // rows (states)
        {
            // simulates actions
            for (int a = 0; a < trials.GetLength(1); a++)
            {
                // runs a trial for entering a state with a given action.
                trials[s, a] = EvaluatePolicy((state)s, (stateAction)s);
            }

            // reset the computer values.
            computer.transform.position = origPos;
            computer.transform.rotation = origRot;
            computer.rigidbody.velocity = origVel;
            computer.drifting = drifting;

        }

        // finds the highest end reward.
        int lastCol = trials.GetLength(1) - 1; // the last column.
        UpdateAction ua = trials[0, lastCol]; // grabs first value.

        // grabs the shortest distance.
        for (int i = 0; i < trials.GetLength(0); i++)
        {
            // new lowest value found.
            if(Mathf.Min(ua.distance, trials[i, lastCol].distance) != ua.distance)
            {
                ua = trials[i, lastCol];
            }
        }

        // sets next action.
        newAction = true; // this is a new action.
        nextAction = ua;
    }


    // Update is called once per frame
    void Update()
    {
        // refresh timer is less than or equal to 0.
        if(refreshTimer <= 0.0F)
        {
            DetermineNextAction(simIterations);
            refreshTimer = refreshTimerMax;
        }
        else
        {
            refreshTimer -= Time.deltaTime;
        }
    }
}
