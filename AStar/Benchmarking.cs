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
            .WithIterationCount(10) // Set the number of measurement iterations
            .WithRuntime(CoreRuntime.Core80)); // Use .NET 8.0 runtime
    }
}

[Config(typeof(CustomConfig))] // Use .NET 8.0 runtime
public class AStarBenchmarking
{
    private Grid grid;
    private Node startNode;
    private Node endNode;

    [Params(50, 100, 200)] // Different sizes of the grid
    public int GridSize;

    [Params(50, 100, 200)] 
    public int threshold;
    [Params(16, 32, 64)]
    public int tileSize;


    [GlobalSetup]
    public void Setup()
    {
        grid = new Grid(GridSize, GridSize); // Initialize the grid with the specified size
        startNode = grid.NodeGrid[0, 0]; 
        endNode = grid.NodeGrid[GridSize - 1, GridSize - 1]; 
        startNode.Wall = false;
        endNode.Wall = false;
        
    }

    [Benchmark]
    public void BenchmarkAStar()
    {
        AStarClassic.AStarNoVisuals(grid, startNode, endNode); // Run the A* algorithm with visualization
        AStarMemoryOptimizations.AStarBlocking(grid, startNode, endNode, tileSize, threshold); // Run the A* algorithm with blocking

    }


    [Benchmark]
    public void BenchmarkAStarBlocking()
    {
        AStarMemoryOptimizations.AStarBlocking(grid, startNode, endNode, tileSize, threshold); // Run the A* algorithm with blocking
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


/*



*/