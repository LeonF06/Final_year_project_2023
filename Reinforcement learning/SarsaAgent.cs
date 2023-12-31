using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class SarsaAgent : MonoBehaviour
{
    private StarManager starManager;
    private PlayerDataLoader dataLoader;

    // Define the number of states for position_interval and rel_position
    private int numRelPositionStates = 3; // BEHIND, ON, FRONT

    private int numStates = 30; // 30 states (State1 to State30)
    private int numActions = 2; // 2 actions (Drive and Boost)

    // Hyperparameters for the SARSA algorithm
    private float learningRate = 0.05f; // Learning rate (alpha)
    private float discountFactor = 0.9f; // Discount factor (gamma)
    private float epsilon = 0.1f; // Epsilon-greedy exploration parameter
    private int maxEpisodes = 900; // Maximum number of episodes

    // Initialize the Q-table with zeros
    private float[,] QTable;

    // Other variables
    private int time = 0;
    private int next_time = 0;
    private int agentPosition;
    // List that stores the cumulative rewards
    private int cumulativeReward = 0;
    public List<int> cumulativeRewards = new List<int>();
    // List that stores the reward at each time step
    public List<int> rewards = new List<int>();
    // List that stores the episode numbers
    public List<int> episodeNumbers = new List<int>();

    private void Awake()
    {
        // Find the existing StarManager instance in the scene
        starManager = FindObjectOfType<StarManager>();

        if (starManager == null)
        {
            Debug.LogError("StarManager not found in the scene!");
        }

        // Find the existing PlayerDataLoader instance in the scene
        dataLoader = FindObjectOfType<PlayerDataLoader>();
    }

    public void StartSarsa()
    {
        if (dataLoader != null && starManager.isAI == true)
        {
            // Load the player's data from the server
            dataLoader.LoadData(starManager.playerID);

            // Train the Sarsa agent after the data is loaded
            StartCoroutine(TrainSarsaAfterDataLoaded(maxEpisodes));
        }
        else
        {
            Debug.LogError("PlayerDataLoader not found in the scene!");
        }
    }

    private IEnumerator TrainSarsaAfterDataLoaded(int numEpisodes)
    {
        while (dataLoader.LoadingData) // Check if data is still being loaded
        {
            yield return null; // Wait for a frame
        }

        // Expand the recorded player data
        dataLoader.DoubleData((maxEpisodes / 9) - 1);

        while (dataLoader.DoublingData) // Check if data is still being loaded
        {
            yield return null; // Wait for a frame
        }

        // Print the Q-table
        /*Debug.Log("Q-table (begin):");
        for (int state = 0; state < numStates; state++)
        {
            for (int action = 0; action < numActions; action++)
            {
                Debug.Log("QTable[" + state + ", " + action + "] = " + QTable[state, action]);
            }
        }*/

        // Train the Sarsa agent
        TrainSarsaAgent(numEpisodes);
    }



    // Training the Sarsa agent
    private void TrainSarsaAgent(int numEpisodes)
    {
        // Step 1 - Initialize the Q-table with zeros
        QTable = new float[numStates, numActions];

        // Step 2 - Repeat for each episode
        for (int episode = 0; episode < numEpisodes; episode++)
        {
            // Variable initialization at the start of each race
            time = 0;
            next_time = 2;
            agentPosition = 1;
            
            // Step 3 - Initialize the state
            int currentState = MapState(time, agentPosition);
            
            // Step 4 - Choose an action using an epsilon-greedy policy
            int currentAction = EpsilonGreedyPolicy(currentState);

            // Step 5 - Repeat for each step of the episode
            while (!CheckTerminalState(episode))
            { 
                // Step 6 - Take the chosen action, observe reward and next state
                agentPosition = TakeAction(currentAction, agentPosition);
                int reward = CalculateReward(agentPosition);
                int nextState = MapState(time, agentPosition);
                
                // update the cummalative rewards and episode numbers lists
                cumulativeReward += reward;
                cumulativeRewards.Add(cumulativeReward);
                episodeNumbers.Add(episode+1);

                // Step 7 - Choose the next action from the next stae using an epsilon-greedy policy
                int nextAction = EpsilonGreedyPolicy(nextState);

                // Step 8 - Update Q(s, a) using the Sarsa update rule
                UpdateQValue(currentState, currentAction, reward, nextState, nextAction);

                // Step 9 - Transition to the next state
                currentState = nextState;
                
                // Step 10 - Transition to the next action
                currentAction = nextAction;
            }
        }

        Debug.Log("Training complete.");
        // print the cumulative rewards
        /*foreach (int reward in cumulativeRewards)
        {
            Debug.Log(reward);
        }*/

        // Print the Q-table
        /*Debug.Log("Q-table (end):");
        for (int state = 0; state < numStates; state++)
        {
            for (int action = 0; action < numActions; action++)
            {
                Debug.Log("QTable[" + state + ", " + action + "] = " + QTable[state, action]);
            }
        }*/

        // Save the Q-table to starManager
        starManager.QTable = QTable;

        // Print the bossActions list
        /*Debug.Log("bossActions:");
        foreach (int action in starManager.bossActions)
        {
            Debug.Log(action);
        }*/

        // Export the cumulative rewards to a CSV file
        ExportCumulativeRewardsToCSV();
    }

    // Export the cumulative rewards to a CSV file
    private void ExportCumulativeRewardsToCSV()
    {
        string filePath =  "C:/Users/Leon/OneDrive - North-West University/Documents/NWU 4/FYP/RL code/Sarsa_final/SarsaCumulativeRewards10.csv";
        string delimiter = ",";

        // Create a StringBuilder to populate the CSV file
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        // Add the header line
        sb.AppendLine("TimeStep" + delimiter + "Episode" + delimiter + "Reward" + delimiter + "Cumulative Reward");

        // Add the cumulative rewards
        for (int timeStep = 1; timeStep <= cumulativeRewards.Count; timeStep++)
        {
            sb.AppendLine(timeStep + delimiter + episodeNumbers[timeStep-1] + delimiter + rewards[timeStep-1] + delimiter +cumulativeRewards[timeStep-1]);
        }

        // Write the CSV file
        System.IO.File.WriteAllText(filePath, sb.ToString());

        Debug.Log("Cumulative rewards exported to " + filePath);
    }




    // Map (position_interval, rel_position) to a state in the range State1 - State30
    private int MapState(int time, int agentPosition)
    {
        int playerPosition;
        int index;
        if (time == 0)
        {
            playerPosition = 3;
        } else {
            index = dataLoader.LoadedTimes.IndexOf(time*1000);
            playerPosition = dataLoader.LoadedPositions[index];
            try
            {
                next_time = dataLoader.LoadedTimes[index + 2];
                next_time = next_time / 1000;
            }
            catch (ArgumentOutOfRangeException)
            {
                next_time = 1;
            }
        }

        int relativePosition = agentPosition - playerPosition;
        int relPosition;

        // Map the playerPosition and the relativePosition to State1 - State30
        int positionInterval = playerPosition / 100;
        // BEHIND
        if (relativePosition < -5)
        {
            relPosition = 0;
        }
        // ON
        else if (relativePosition > -5 && relativePosition < 15)
        {
            relPosition = 1;
        }
        // FRONT
        else
        {
            relPosition = 2;
        }

        // Calculate the mapped state
        int state = positionInterval * numRelPositionStates + relPosition + 1;


        //Debug.Log("state: " + state + " (positionInterval: " + positionInterval + ", relPosition: " + relPosition + ")");
        return state;
    }

    // Function to take an action
    private int TakeAction(int action, int position)
    {
        float driveSpeed = 12.67f;
        float boostSpeed = 14.63f;
        int duration = 2; // Drive and boost duration time in seconds
        int newPosition = 0;

        if (action == 0)
        {
            newPosition = position + (int)(driveSpeed * duration);
        }
        else if (action == 1)
        {
            newPosition = position + (int)(boostSpeed * duration);
        }

        time += duration;

        return newPosition;
    }

    // Epsilon-greedy policy for action selection
    private int EpsilonGreedyPolicy(int state)
    {
        if (UnityEngine.Random.value < epsilon)
        {
            // Explore: Choose a random action
            return UnityEngine.Random.Range(0, numActions);
        }
        else
        {
            // Exploit: Choose the action with the highest Q-value
            float maxQ = QTable[state, 0];
            int bestAction = 0;

            for (int action = 1; action < numActions; action++)
            {
                if (QTable[state, action] > maxQ)
                {
                    maxQ = QTable[state, action];
                    bestAction = action;
                }
            }

            return bestAction;
        }
    }

    // Update the Q-value using the SARSA update rule
    private void UpdateQValue(int state, int action, int reward, int nextState, int nextAction)
    {
        // SARSA update rule
        float currentQ = QTable[state, action]; // Q(s,a)
        float targetQ = QTable[nextState, nextAction]; // Q(s',a')
        float newQ = currentQ + learningRate * (reward + discountFactor * targetQ - currentQ);
        QTable[state, action] = newQ;
    }

    // Function to check if the current state is a terminal state
    private bool CheckTerminalState(int currentEpisode)
    {
        //Debug.Log("Time: " + time + ", Next time: " + next_time);
        
        if (time < next_time)
        {
            return false;
        }
        else
        {
            UpdatePlayerData(currentEpisode);
            return true;
        }
    }

    // Function to calculate the reward
    private int CalculateReward(int agentPosition)
    {
        int timeMS = time * 1000; // Time in milliseconds
        // Get the index of dataLoader.LoadedTimes where the time is equal to timeMS
        int index = dataLoader.LoadedTimes.IndexOf(timeMS);
        //Debug.Log("time: " + time);
        //Debug.Log("index: " + index);
        int playerPosition = dataLoader.LoadedPositions[index];
        int relativePosition = agentPosition - playerPosition;
        int reward;

        // Calculate the reward
        // BEHIND
        if (relativePosition < -5)
        {
            reward = -1;
        }
        // ON
        else if (relativePosition > -5 && relativePosition < 15)
        {
            reward = 1;
        }
        // FRONT
        else
        {
            reward = -1;
        }

        // Add reward to list
        rewards.Add(reward);

        return reward;

    }


    // Function to update the player's data
    private void UpdatePlayerData(int currentEpisode)
    {
        if (currentEpisode + 1 < 900)
        {
            // Determine the range of indices where dataLoader.EpisodeNumbers == currentEpisode + 1
            int startIndex = 0;
            int endIndex = 0;

            for (int i = 0; i < dataLoader.EpisodeNumbers.Count; i++)
            {
                if (dataLoader.EpisodeNumbers[i] == currentEpisode + 1)
                {
                    startIndex = i;
                    break;
                }
            }

            for (int i = startIndex; i < dataLoader.EpisodeNumbers.Count; i++)
            {
                if (dataLoader.EpisodeNumbers[i] == currentEpisode + 2)
                {
                    endIndex = i;
                    break;
                }
            }

            //Debug.Log("currentEpisode: " + currentEpisode + ", startIndex: " + startIndex + ", endIndex: " + endIndex);

            // Remove all the data in dataLoader.LoadedTimes and dataLoader.LoadedPositions that are in the range of indices
            dataLoader.LoadedTimes.RemoveRange(startIndex, endIndex - startIndex);
            dataLoader.LoadedPositions.RemoveRange(startIndex, endIndex - startIndex);
            //Debug.Log("Success");

            // Re-index the lists to start at index 0 again
            int numItemsToRemove = endIndex - startIndex;
            for (int i = 0; i < numItemsToRemove; i++)
            {
                dataLoader.LoadedTimes.Insert(i, dataLoader.LoadedTimes[startIndex + i]);
                dataLoader.LoadedPositions.Insert(i, dataLoader.LoadedPositions[startIndex + i]);
            }
        }
    }
    
}
