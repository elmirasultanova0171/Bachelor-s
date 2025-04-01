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
            .WithWarmupCount(5) // Set the number of warmup iterations
            .WithIterationCount(5) // Set the number of measurement iterations
            .WithRuntime(CoreRuntime.Core80)); // Use .NET 8.0 runtime
    }
}



[Config(typeof(CustomConfig))] // Use .NET 8.0 runtime
public class AStarBenchmarking
{
    private GridS gridS;
    private NodeS startNodeS;
    private NodeS endNodeS;

    
    private Grid grid;

    private Grid grid2;
    private Node startNode;
    private Node endNode;

    [Params(300)] // Different sizes of the grid
    public int GridSize;




    /*

        [Params(50, 100, 200)]
    public int threshold;

    [Params(16, 32)]
    public int tileSize;
    [Params(50, 100, 200)] 
    public int threshold;
    [Params(16, 32, 64)]
    public int tileSize;
    

    public  IMemoryCache cache;

    public Dictionary<int, bool> hashMap;

    public int[] lookups = new int[100];

    [Params(1, 2, 3, 4, 5)]
    public int lookAhead;

    Random rnd = new Random();


    [Params(100, 1000, 10000)]
    public int size;
    */

    [GlobalSetup]
    public void Setup()
    {
        
        gridS = new GridS(GridSize, GridSize); // Initialize the grid with the specified size
        startNodeS = gridS.NodeGrid[0, 0];
        endNodeS = gridS.NodeGrid[GridSize - 1, GridSize - 1];
        startNodeS.Wall = false;
        endNodeS.Wall = false;

        grid = new Grid(GridSize, GridSize); // Initialize the grid with the specified size
        startNode = grid.NodeGrid[0, 0];
        endNode = grid.NodeGrid[GridSize - 1, GridSize - 1];
        startNode.Wall = false;
        endNode.Wall = false;
 
         /*   
        // Prefetching
        cache = new MemoryCache(new MemoryCacheOptions());

        hashMap = new Dictionary<int, bool>
        {
            { 1, true }, { 2, false }, { 3, true }, { 4, true },
            { 5, true }, { 6, false }, { 7, true }, { 8, true }
        };

        for (int i = 0; i < lookups.Length; i++)
        {
            lookups[i] = rnd.Next(1, 9);
        }
            */
        
        
    }

    
  
   

    /*

     

     [Benchmark]
    public void WithStackAlloc()
    {
        AStarMemoryOptimizations.AStarStackAlloc(gridS, startNodeS, endNodeS);
    }

    [Benchmark]
    public void WithStruct()
    {
        AStarMemoryOptimizations.AStarStruct(gridS, startNodeS, endNodeS);
    }

    [Benchmark]
    public void WithoutStackAlloc()
    {
        AStarClassic.AStarNoVisuals(grid, startNode, endNode);
    }
    

   */

   [Benchmark]
    public void WithoutStackAllocOrStruct()
    {
        AStarClassic.AStarNoVisuals(grid, startNode, endNode);
    }

    [Benchmark]
    public void WithStackAlloc()
    {
        AStarMemoryOptimizations.AStarStackAlloc(ref gridS, ref startNodeS, ref endNodeS);
    }

    [Benchmark]
    public void WithStack2()
    {
        AStarMemoryOptimizations.AStarStackAlloc2(ref gridS, ref startNodeS, ref endNodeS);
    }

   /* [Benchmark]
    public void WithStack2()
    {
       // AStarMemoryOptimizations.AStarStackAlloc2(ref gridS, ref startNodeS, ref endNodeS);
    }

    /*
      [Benchmark]
    public void WithStruct()
    {
        AStarMemoryOptimizations.AStarStruct(ref gridS, ref startNodeS, ref endNodeS);
    }
    */


}

public class Program
{
    
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<AStarBenchmarking>();
        Console.WriteLine(summary);
    }
    
} 




