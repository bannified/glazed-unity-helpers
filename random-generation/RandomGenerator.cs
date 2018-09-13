using UnityEngine;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using SharpNeat.Utility;

public class RandomGenerator {

    static ZigguratGaussianSampler normalSampler = new ZigguratGaussianSampler(new FastRandom());

    static float getTotalWeight(List<float> weightArray)
    {
        float totalWeight = 0;
        foreach (float weight in weightArray)
        {
            System.Console.WriteLine(weight);
            totalWeight += weight;
        }
        return totalWeight;
    }

    static float getRandomWeightPoint(float totalWeight)
    {
        return Random.value * totalWeight;
    }

    public static int getRandomWeightedIndex(List<float> weightArray)
    {
        float totalWeight = getTotalWeight(weightArray);
        float point = getRandomWeightPoint(totalWeight);
        for (int i = 0; i < weightArray.Count; i++)
        {
            if (point < weightArray[i])
            {
                return i;
            }
            else
            {
                point -= weightArray[i];
            }
        }
        return weightArray.Count - 1;
    }

    //public static float NextGaussianFloat()
    //{
    //    float U, u, v, S;

    //    do
    //    {
    //        u = 2 * Random.value - 1;
    //        v = 2 * Random.value - 1;
    //        S = u * u + v * v;
    //    } while (S>=1.0);

    //    float fac = Mathf.Sqrt(-2 * Mathf.Log(S) / S);
    //    return u * fac;
    //}
    
    public static int RandomGaussianIntInRange(int min, int max) //using ZigguratGaussianSampler.cs, for gold and exp
    {
        int mean = (min + max) / 2;
        int sigma = (max - mean) / 3;
        int returnValue;
        do
        {
            returnValue = (int)normalSampler.NextSample(mean, sigma);
        } while (returnValue < min || returnValue > max);

        return returnValue;
    }

    public static float RandomGaussianFloatInRange(float min, float max, float numberOfSigma = 3)
    {
        float mean = (min + max) / 2;
        float sigma = (max - mean) / numberOfSigma;
        float returnValue;
        do
        {
            returnValue = (float)normalSampler.NextSample(mean, sigma);
        } while (returnValue < min || returnValue > max);

        return returnValue;
    }


}
