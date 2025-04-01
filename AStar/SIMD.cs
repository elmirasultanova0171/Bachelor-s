using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

public class SIMD {


   List<Node> openList = new List<Node>();
        
        // Generate test data (e.g., 10,000 nodes with random F values)
     public static int FindMinIndexWithSIMD(List<Node> openList)
    {
        if (openList.Count == 0) return -1; // Edge case: empty list

        int lowestIndex = 0;
        float minValue = float.MaxValue;
        Span<Node> nodeSpan = CollectionsMarshal.AsSpan(openList); // Avoids copying

        int simdSize = Vector<float>.Count;
        Span<float> buffer = stackalloc float[simdSize]; // Temporary stack buffer

        // Process SIMD chunks
        for (int i = 0; i + simdSize <= openList.Count; i += simdSize)
        {
            // Load F values into the buffer
            for (int j = 0; j < simdSize; j++)
                buffer[j] = nodeSpan[i + j].F;

            Vector<float> currentFValues = new Vector<float>(buffer);

            // Find the min in the vector
            Vector<float> comparison = Vector.Min(new Vector<float>(minValue), currentFValues);

            // Extract the scalar minimum
            float newMin = comparison[0];
            for (int j = 1; j < simdSize; j++)
                newMin = Math.Min(newMin, comparison[j]);

            // Update minValue and lowestIndex
            if (newMin < minValue)
            {
                minValue = newMin;
                for (int j = 0; j < simdSize; j++)
                    if (buffer[j] == newMin)
                        lowestIndex = i + j;
            }
        }

        // Process any remaining elements (non-SIMD tail case)
        for (int i = openList.Count - (openList.Count % simdSize); i < openList.Count; i++)
        {
            if (nodeSpan[i].F < minValue)
            {
                minValue = nodeSpan[i].F;
                lowestIndex = i;
            }
        }

        return lowestIndex;
    }
    
    public static int FindMinIndexWithoutSIMD(List<Node> openList)
    {
        if (openList.Count == 0) return -1;

        int lowestIndex = 0;
        float minValue = float.MaxValue;

        for (int i = 0; i < openList.Count; i++)
        {
            if (openList[i].F < minValue)
            {
                minValue = openList[i].F;
                lowestIndex = i;
            }
        }

        return lowestIndex;
    }

}