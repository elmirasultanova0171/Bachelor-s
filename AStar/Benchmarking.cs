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
        AddDiagnoser(MemoryDiagnoser.Default);
        AddDiagnoser(new EtwProfiler());
        AddJob(Job.Default
            .WithWarmupCount(15) // Set the number of warmup iterations
            .WithIterationCount(15) // Set the number of measurement iterations
            .WithRuntime(CoreRuntime.Core80)); // Use .NET 8.0 runtime
    }
}

[Config(typeof(CustomConfig))] // Use .NET 8.0 runtime
public class AStarBenchmarking
{
    private Grid grid;
    private Node startNode;
    private Node endNode;

    [Params(200)] // Different sizes of the grid
    public int GridSize;

    [GlobalSetup]
    public void Setup()
    {
        grid = new Grid(GridSize, GridSize); // Initialize the grid with the specified size
        startNode = grid.NodeGrid[0, 0]; // Set the start node
        endNode = grid.NodeGrid[GridSize - 1, GridSize - 1]; // Set the end node
        
    }

    [Benchmark]
    public void BenchmarkAStar()
    {
        AStarClassic.AStarNoVisuals(grid, startNode, endNode); // Run the A* algorithm with visualization
    }


    [Benchmark]
    public void BenchmarkAStarBlocking()
    {
        AStarMemoryOptimizations.AStarBlocking(grid, startNode, endNode, 16); // Run the A* algorithm with blocking
    }

}

public class Program
{
    /*
    public static void Main(string[] args)
    {
        var summary = BenchmarkRunner.Run<AStarBenchmarking>();
        Console.WriteLine(summary);
    }
    */
}


/*

| Method         | GridSize | Mean     | Error    | StdDev   | Allocated |
|--------------- |--------- |---------:|---------:|---------:|----------:|
| BenchmarkAStar | 100      | 175.9 ms | 34.08 ms | 31.88 ms |  10.33 KB |


*/