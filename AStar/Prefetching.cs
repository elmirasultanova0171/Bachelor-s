using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
public class Prefetching{

    static unsafe int GetSumOfDigits(int val)
    {
        int sum = 0;
        while (val > 0)
        {
            sum += val % 10;
            val /= 10;
        }
        return sum;
    }

    public static unsafe int SolutionWithPrefetching(int* lookupPtr, int lookupCount, Dictionary<int, bool> hashMap, int lookAhead)
    {
        int result = 0;

        // First loop: Use lookahead to "prefetch" elements
        for (int i = 0; i < lookupCount - lookAhead; i++)
        {
            int val = *(lookupPtr + i);
            if (hashMap.ContainsKey(val) && hashMap[val]) //try getValue
                result += GetSumOfDigits(val);

            // Simulate prefetching
            int prefetchVal = *(lookupPtr + i + lookAhead);
            bool prefetchExists = hashMap.ContainsKey(prefetchVal); // Simulates memory access
        }

        // Final loop: Process remaining elements without lookahead
        for (int i = lookupCount - lookAhead; i < lookupCount; i++)
        {
            int val = *(lookupPtr + i);
            if (hashMap.ContainsKey(val) && hashMap[val])
                result += GetSumOfDigits(val);
        }

        return result;
    }

    public static int SolutionWithoutPrefetching(int[] lookups, Dictionary<int, bool> hashMap)
    {
        int result = 0;

        // Loop through all elements without prefetching
        for (int i = 0; i < lookups.Length; i++)
        {
            int val = lookups[i];
            if (hashMap.ContainsKey(val) && hashMap[val])
                result += GetSumOfDigits(val);
        }

        return result;
    }


    static public int SolutionWithCaching(int[] lookups, Dictionary<int, bool> hashMap, IMemoryCache cache)
    {
        int result = 0;

        foreach (var val in lookups)
        {
            if (hashMap.TryGetValue(val, out bool value) && value)
            {
                if (!cache.TryGetValue(val, out int sum))
                {
                    sum = GetSumOfDigits(val);
                    cache.Set(val, sum, TimeSpan.FromMinutes(0.6));
                }
                result += sum;
            }
        }

        return result;
    }


/*
    static unsafe void Main(string[] args)
    {
        var hashMap = new Dictionary<int, bool>
        {
            { 1, true }, { 2, false }, { 3, true }, { 4, true },
            { 5, true }, { 6, false }, { 7, true }, { 8, true }
        };
        var lookups = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 3, 5, 7, 2, 4, 6, 8 };
        int lookAhead = 2;
            int result = SolutionWithoutPrefetching(lookups, hashMap);
                Console.WriteLine($"Result without Prefetching: {result}");


        // Prefetching
        fixed (int* lookupPtr = lookups)
        {
            int resultPrefetching = SolutionWithPrefetching(lookupPtr, lookups.Length, hashMap, lookAhead);
            Console.WriteLine($"Result with Prefetching: {resultPrefetching}");
        }



        // Caching
        //var cache = new MemoryCache(new MemoryCacheOptions());
       // int resultCaching = SolutionWithCaching(lookups, hashMap, cache);
        //Console.WriteLine($"Result with Caching: {resultCaching}");
    }
*/
}