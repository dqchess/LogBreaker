using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility{



    public static Dictionary<T, int> Knapsack<T>(Dictionary<T, float> set, float capacity,
           Dictionary<T, int> knapsack = null)
    {
        var keys = new List<T>(set.Keys);
        // Sort keys by their weights in descending order
        keys.Sort((a, b) => -set[a].CompareTo(set[b]));

        if (knapsack == null)
        {
            knapsack = new Dictionary<T, int>();
            foreach (var key in keys)
            {
                knapsack[key] = 0;
            }
        }
        return Knapsack(set, keys, capacity, knapsack, 0);
    }

    private static Dictionary<T, int> Knapsack<T>(Dictionary<T, float> set, List<T> keys, float remainder,
           Dictionary<T, int> knapsack, int startIndex)
    {
        T smallestKey = keys[keys.Count - 1];
        if (remainder < set[smallestKey])
        {
            knapsack[smallestKey] = 1;
            return knapsack;
        }
        // Cycle through items and try to put them in knapsack
        for (var i = startIndex; i < keys.Count; i++)
        {
            T key = keys[i];
            float weight = set[key];
            // Larger items won't fit, smaller items will fill as much space as they can
            knapsack[key] += (int)(remainder / weight);
            remainder %= weight;
        }
        if (remainder > 0)
        {
            // Throw out largest item and try again
            for (var i = 0; i < keys.Count; i++)
            {
                T key = keys[i];
                if (knapsack[key] != 0)
                {
                    // Already tried every combination, return as is
                    if (key.Equals(smallestKey))
                    {
                        return knapsack;
                    }
                    knapsack[key]--;
                    remainder += set[key];
                    startIndex = i + 1;
                    break;
                }
            }
            knapsack = Knapsack(set, keys, remainder, knapsack, startIndex);
        }
        return knapsack;
    }

    public static void Shuffle<T>(this List<T> array)
    {
        for (int i = 0; i < array.Count; i++)
        {
            int j = Random.Range(i, array.Count);
            T tmp = array[j];
            array[j] = array[i];
            array[i] = tmp;
        }
    }
}
