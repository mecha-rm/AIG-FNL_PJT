using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Q-value storing object
public class QValueStore
{
    // state, actions
    float[,] array = new float[4,4];
    public float getQValue(int state, int action)
    {
        return 0f;
    }
    public int getBestAction(int state)
    {
        return 0;
    }
    public void storeQValue(int state, int action, float value)
    {

    }
}

public class ReinforcementProblem
{
    // # Choose a random starting state for the problem.
    // function getRandomState()
    // 
    // # Get the available actions for the given state.
    // function getAvailableActions(state)
    // 
    // # Take the given action and state, and return
    // # a pair consisting of the reward and the new state.
    //  function takeAction(state, action)
}

// this is the script for the player that's controlled by the computer.
public class ComputerPlayer : Player
{
    public QValueStore store = new QValueStore();

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    // Updates the store by investigating the problem.
    //I am assuming that the random function returns a floating point number between zero and
    //one.The oneOf function picks an item from a list at random.
    //public void QLearning(problem, int iterations, alpha, gamma, rho, nu)
    //{
    //    // Get a starting state.
    //    state = problem.getRandomState()

    //    // Repeat a number of times.
    //    for (int x = 0; x < iterations; x++)
    //    {
    //        // Pick a new state every once in a while.
    //        if (random() < nu)
    //            state = problem.getRandomState();

    //        // Get the list of available actions.
    //        actions = problem.getAvailableActions(state);

    //        // Should we use a random action this time?
    //        if (random() < rho) {
    //            action = oneOf(actions);
    //        } else {
    //            // Otherwise pick the best action.
    //            action = store.getBestAction(state)
    //        }
    //        // Carry out the action and retrieve the reward and new state.
    //        reward, newState = problem.takeActions(state, action)

    //        // Get the current q from the store.
    //        Q = store.getQValue(state, action);

    //        // Get the q of the best action from the new state.
    //        maxQ = store.getQValue(newState, store.getBestAction(newState));

    //        // Perform the q learning.
    //        Q = (1 - alpha) * Q + alpha * (reward + gamma * maxQ);

    //        // Store the new Q-value.
    //        store.storeQValue(state, action, Q);

    //        // And update the state.
    //        state = newState;
    //    }
    //}
}







