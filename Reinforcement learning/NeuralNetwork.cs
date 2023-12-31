using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class NeuralNetwork : MonoBehaviour
{
    private int inputSize = 3;
    private int hiddenLayerSize = 16;
    private int outputSize = 2;

    // Neural network weights and biases
    private float[,] inputToHiddenWeights;
    private float[] hiddenBiases;
    private float[,] hiddenToOutputWeights;
    private float[] outputBiases;

    private System.Random random = new System.Random();

    public TargetNetwork targetNetwork; // Reference to the target network

    // Function to initialize the current network Q
    public void InitializeCurrentNetwork()
    {
        // Initialize weights and biases randomly
        inputToHiddenWeights = InitializeWeights(inputSize, hiddenLayerSize);

        hiddenBiases = InitializeBiases(hiddenLayerSize);

        hiddenToOutputWeights = InitializeWeights(hiddenLayerSize, outputSize);

        outputBiases = InitializeBiases(outputSize);
    }

    public void UpdateTargetNetwork()
    {
        // Copy weights and biases from the current network to the target network
        targetNetwork.targetinputToHiddenWeights = (float[,])inputToHiddenWeights.Clone();
        targetNetwork.targethiddenBiases = (float[])hiddenBiases.Clone();
        targetNetwork.targethiddenToOutputWeights = (float[,])hiddenToOutputWeights.Clone();
        targetNetwork.targetoutputBiases = (float[])outputBiases.Clone(); 
    }

    private float[,] InitializeWeights(int input_size, int output_size)
    {
        float[,] weights = new float[input_size, output_size];
        float xavierFactor = Mathf.Sqrt(2.0f / (input_size + output_size));

        for (int i = 0; i < input_size; i++)
        {
            for (int j = 0; j < output_size; j++)
            {
                weights[i, j] = (float)random.NextDouble() * 2 * xavierFactor - xavierFactor;
            }
        }

        return weights;
    }

    private float[] InitializeBiases(int size)
    {
        float[] biases = new float[size];
        for (int i = 0; i < size; i++)
        {
            biases[i] = (float)random.NextDouble() * 2 - 1; // Random values between -1 and 1
        }
        return biases;
    }

    private (float[], float[]) ForwardPass(float[] inputVector)
    {
        float[] hiddenLayerOutput = new float[hiddenLayerSize];
        float[] outputLayerOutput = new float[outputSize];

        for (int i = 0; i < hiddenLayerSize; i++)
        {
            hiddenLayerOutput[i] = 0.0f;
            for (int j = 0; j < inputSize; j++)
            {
                hiddenLayerOutput[i] += inputVector[j] * inputToHiddenWeights[j, i];
            }
            hiddenLayerOutput[i] += hiddenBiases[i];
            //hiddenLayerOutput[i] = Mathf.Max(0.0f, hiddenLayerOutput[i]); // ReLU activation
            hiddenLayerOutput[i] = 1.0f / (1.0f + Mathf.Exp(-hiddenLayerOutput[i])); // Sigmoid activation


        }

        for (int i = 0; i < outputSize; i++)
        {
            outputLayerOutput[i] = 0.0f;
            for (int j = 0; j < hiddenLayerSize; j++)
            {
                outputLayerOutput[i] += hiddenLayerOutput[j] * hiddenToOutputWeights[j, i];
            }
            outputLayerOutput[i] += outputBiases[i];
        }

        return (outputLayerOutput, hiddenLayerOutput);
    }

    public int GetBestAction(float currentXPosition, float currentRelPosition, float currentTime)
    {
        float[] inputVector = { currentXPosition, currentRelPosition, currentTime };
        (float[] outputLayerOutput, float[] hiddenLayerOutput) = ForwardPass(inputVector);

        // Determine the best action (0 or 1) based on the Q-values
        int bestAction = (outputLayerOutput[0] > outputLayerOutput[1]) ? 0 : 1;

        return bestAction;
    }

    public float GetMaxQValue(float currentXPosition, float currentRelPosition, float currentTime)
    {
        float[] inputVector = { currentXPosition, currentRelPosition, currentTime };
        (float[] outputLayerOutput, float[] hiddenLayerOutput) = targetNetwork.ForwardPass(inputVector);

        return Mathf.Max(outputLayerOutput[0], outputLayerOutput[1]);
    }

    // Function to update the neural network using stochastic gradient descent
    public void StochasticGradientDescent(float xPosition, float relPosition, float timeMs, float target, float learningRate)
    {
        // Create an input vector from the provided parameters
        float[] inputVector = { xPosition, relPosition, timeMs };

        // Forward pass to compute the network's output
        (float[] output, float[] hiddenLayerOutput) = ForwardPass(inputVector);

        // Calculate the loss (e.g., mean squared error) based on the provided target
        float[] loss = new float[outputSize];
        for (int i = 0; i < outputSize; i++)
        {
            loss[i] = (target - output[i]) * (target - output[i]); // (yj − Q(φj , aj ; θ))^2
        }

        float[] delta_output = new float[outputSize];
        float[] delta_hidden = new float[hiddenLayerSize];

        // Calculate delta for output layer
        for (int i = 0; i < outputSize; i++)
        {
            delta_output[i] = loss[i] * output[i] * (1 - output[i]);
        }

        // Calculate delta for hidden layer
        for (int i = 0; i < hiddenLayerSize; i++)
        {
            float sum = 0;
            for (int j = 0; j < outputSize; j++)
            {
                sum += delta_output[j] * hiddenToOutputWeights[i, j];
            }
            delta_hidden[i] = hiddenLayerOutput[i] * (1 - hiddenLayerOutput[i]) * sum;
        }

        // Update output weights
        for (int i = 0; i < outputSize; i++)
        {
            for (int j = 0; j < hiddenLayerSize; j++)
            {
                hiddenToOutputWeights[j, i] += learningRate * delta_output[i] * hiddenLayerOutput[j];
            }
        }

        // Update hidden weights
        for (int i = 0; i < hiddenLayerSize; i++)
        {
            for (int j = 0; j < inputSize; j++)
            {
                inputToHiddenWeights[j, i] += learningRate * delta_hidden[i] * inputVector[j];
            }
        }

        // Update output biases
        for (int i = 0; i < outputSize; i++)
        {
            outputBiases[i] += learningRate * delta_output[i];
        }

        // Update hidden biases
        for (int i = 0; i < hiddenLayerSize; i++)
        {
            hiddenBiases[i] += learningRate * delta_hidden[i];
        }
    }
}