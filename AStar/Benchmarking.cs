using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Diagnostics.Windows;
using System;
using System.Collections.Generic;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Diagnostics.Windows.Configs;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.CodeAnalysis.CSharp.Syntax;


/*
[MemoryDiagnoser] // Measures memory allocation
[EtwProfiler] // Captures detailed CPU usage and other ETW events
[SimpleJob(RuntimeMoniker.Net80)]*/

public class CustomConfig : ManualConfig
{
    public CustomConfig()
    {
        AddDiagnoser(MemoryDiagnoser.Default); // Measure memory usage
        AddDiagnoser(new EtwProfiler()); // Measure CPU usage and other ETW events
        AddJob(Job.Default
            .WithWarmupCount(3) // Set the number of warmup iterations
            .WithIterationCount(3) // Set the number of measurement iterations
            .WithRuntime(CoreRuntime.Core80)); // Use .NET 8.0 runtime
    }
}

[Config(typeof(CustomConfig))] // Use .NET 8.0 runtime
public class AStarBenchmarking
{
    /*
    private Grid grid;
    private Node startNode;
    private Node endNode;

    [Params(50, 100, 200)] // Different sizes of the grid
    public int GridSize;

    [Params(50, 100, 200)] 
    public int threshold;
    [Params(16, 32, 64)]
    public int tileSize;
    */

    public  IMemoryCache cache;

    public Dictionary<int, bool> hashMap;

    public int[] lookups;

    int lookAhead = 2;

    [GlobalSetup]
    public void Setup()
    {
        /*
        grid = new Grid(GridSize, GridSize); // Initialize the grid with the specified size
        startNode = grid.NodeGrid[0, 0]; 
        endNode = grid.NodeGrid[GridSize - 1, GridSize - 1]; 
        startNode.Wall = false;
        endNode.Wall = false;
        */
     
            
        // Prefetching
        cache = new MemoryCache(new MemoryCacheOptions());

        hashMap = new Dictionary<int, bool>
        {
            { 1, true }, { 2, false }, { 3, true }, { 4, true },
            { 5, true }, { 6, false }, { 7, true }, { 8, true }
        };

        lookups = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 3, 5, 7, 2, 4, 6, 8 };

        
        
    }

    [Benchmark]
    public void NoPrefetch()
    {
        int result = Prefetching.SolutionWithoutPrefetching(lookups, hashMap);
    }


    [Benchmark]
    public unsafe void Cache()
    {
        fixed (int* lookupPtr = lookups)
        {
            int resultPrefetching = Prefetching.SolutionWithPrefetching(lookupPtr, lookups.Length, hashMap, lookAhead);
            
        }
    }

}

public class Program
{
    
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<AStarBenchmarking>();
        Console.WriteLine(summary);
    }
    
}





