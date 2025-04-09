using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Diagnostics.Windows;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;

public class CustomConfig : ManualConfig
{
    public CustomConfig()
    {
        AddDiagnoser(MemoryDiagnoser.Default); // Measure memory usage
        AddDiagnoser(new EtwProfiler()); // Measure CPU usage and other ETW events
        AddJob(Job.Default
            .WithWarmupCount(5) // Set the number of warmup iterations
            .WithIterationCount(5) // Set the number of measurement iterations
            .WithRuntime(CoreRuntime.Core90)); // Use .NET 9.0 runtime
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

    [Params(100, 200, 300)] 
    public int GridSize;
    [Params(16, 32)]
    public int tileSize;
    [Params(50, 100, 200)] 
    public int threshold;

    [GlobalSetup]
    public void Setup()
    {
        
        gridS = new GridS(GridSize, GridSize); 
        startNodeS = gridS.NodeGrid[0, 0];
        endNodeS = gridS.NodeGrid[GridSize - 1, GridSize - 1];
        startNodeS.Wall = false;
        endNodeS.Wall = false;
        
        grid = new Grid(GridSize, GridSize); 
        startNode = grid.NodeGrid[0, 0];
        endNode = grid.NodeGrid[GridSize - 1, GridSize - 1];
        startNode.Wall = false;
        endNode.Wall = false;

        grid2 = new Grid(GridSize, GridSize, true, tileSize);
        startNode = grid2.NodeGrid[0, 0];   
        endNode = grid2.NodeGrid[GridSize - 1, GridSize - 1];
        startNode.Wall = false; 
        endNode.Wall = false;
 
        
    }
    // Benchmark 1: Blocking Test
    [BenchmarkCategory("Blockingtest")]
    [Benchmark]
    public void BlockingTest() => Blocking.AStarBlocking(grid2, startNode, endNode, tileSize, threshold);
    [Benchmark]
    public void AStarClassicTest() => AStarClassic.AStarNoVisuals(grid2, startNode, endNode);

     // Benchmark 1: With vs Without Blocking
    [BenchmarkCategory("Blocking")]

    [Benchmark]
    public void WithBlocking() => Blocking.AStarBlocking(grid, startNode, endNode, tileSize, threshold);
    [Benchmark]
    public void WithoutBlocking() => AStarClassic.AStarNoVisuals(grid, startNode, endNode);

    // Benchmark 2: Class Node vs Struct Node
    [BenchmarkCategory("StructVsClass")]

    [Benchmark]
    public void ClassNode() => AStarClassic.AStarNoVisuals(grid, startNode, endNode);
    [Benchmark]
    public void StructNode() => Struct.AStarStruct(ref gridS, ref startNodeS, ref endNodeS);

    // Benchmark 3: Span Memory Safety (Leak vs Fix)
    [BenchmarkCategory("SpanMemorySafety")]
    [Benchmark]
    public void SpanMemoryLeak() => Span.AStarSpan(ref gridS, ref startNodeS, ref endNodeS);

    [Benchmark]
    public void SpanMemoryFix() => Span.AStarSpanWithHashSet(ref gridS, ref startNodeS, ref endNodeS);

    // Benchmark 5: Closed List - List vs Hash
    [BenchmarkCategory("ClosedList")]
    [Benchmark]
    public void ClosedListAsList() => Span.AStarSpan(ref gridS, ref startNodeS, ref endNodeS);

    [Benchmark]
    public void ClosedListAsHashSet() => Span.AStarSpanWithHashSet(ref gridS, ref startNodeS, ref endNodeS);

    // Benchmark 6: List vs Span for Neighbors
    [BenchmarkCategory("Neighbors")]
    [Benchmark]
    public void AStarClassic_() => AStarClassic.AStarNoVisuals(grid, startNode, endNode);
    [Benchmark]
    public void NeighborsAsList() => Struct.AStarStruct(ref gridS, ref startNodeS, ref endNodeS);
    [Benchmark]
    public void NeighborsAsSpan() => Span.AStarSpanWithHashSet(ref gridS, ref startNodeS, ref endNodeS);
    
    // Benchmark 7: Blocking + Span
    [BenchmarkCategory("BlockingWithSpan")]
    [Benchmark]
    public void AStarClassic2() => AStarClassic.AStarNoVisuals(grid, startNode, endNode);
    [Benchmark]
    public void SpanWithoutBlocking() => Span.AStarSpanWithHashSet(ref gridS, ref startNodeS, ref endNodeS);
    [Benchmark]
    public void BlockingWithSpan() => Blocking.AStarBlocking(grid, startNode, endNode, tileSize, threshold);

    // Benchmark 8: Normal vs Vector Heuristic
    [BenchmarkCategory("VectorHeuristic")]
    [Benchmark]
    public void NormalHeuristic() => Struct.AStarStruct(ref gridS, ref startNodeS, ref endNodeS);

    [Benchmark]
    public void VectorHeuristic() => Vectorization.AStarSIMD(ref gridS, ref startNodeS, ref endNodeS);

    // Benchmark 9: Basic Vector vs Intrinsic Vector vs Normal
    [BenchmarkCategory("VectorVariants")]
    [Benchmark]
    public void BasicVector() => Vectorization.AStarSIMD(ref gridS, ref startNodeS, ref endNodeS);

    //[Benchmark]
    //public void IntrinsicVector() => Vectorization.


    // Benchmark 10: All OOP (Old A) vs All DOP (New A)
    [BenchmarkCategory("OOPvsDOP")]
    [Benchmark]
    public void AllOOP() => AStarClassic.AStarNoVisuals(grid, startNode, endNode);

   // [Benchmark]
   // public void AllDOP() => 



}

public class Program
{
    
    public static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(new[] { "--category", "Blocking" });

    
    }
    
} 




