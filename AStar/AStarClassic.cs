using System;

public class AStarClassic
{
    public static void Main(string[] args)
    {
       
        
    }

    public static void AStar(Grid grid, Node start, Node end){
       
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        List<Node> path = new List<Node>();

        Node[,] nodes = grid.NodeGrid;

        for (int i = 0; i<grid.Rows; i++ ){
            for (int j = 0; j < grid.Columns; j++ ){
                nodes[i,j].AddNeighbors(grid);
            }
        }
        
        openList.Add(start);

        while(openList.Count > 0){

           int lowestIndex = 0;

           for(int i = 0; i < openList.Count; i++){
            if(openList[i].F <openList[lowestIndex].F){
                lowestIndex=i;
            }
           }

            Node current = openList[lowestIndex];

            if(current==end){
                Console.WriteLine("done");    
            }

            openList.Remove(current);
            closedList.Add(current);
            List<Node> neighbors = current.Neighbors;
            for (int i = 0; i < neighbors.Count; i++){
                Node neighbor = neighbors[i];

                if(!closedList.Contains(neighbor)){
                    int tempG = current.G + 1;         //assuming all nodes have a cost of 1
                    if(openList.Contains(neighbor)){
                        if(tempG < neighbor.G){
                            neighbor.G = tempG;
                        }
                    }else{
                        neighbor.G = tempG;
                        openList.Add(neighbor);
                    }   
                }

                neighbor.H = Heuristic(neighbor, end);
                // In the class: neighbor.F = neighbor.G + neighbor.H;
                neighbor.Parent = current;
            }

            grid.PrintGrid();
        }

        

        return;
    }
    public static int Heuristic(Node a, Node b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
   

   //To visualize the path
    public List<Node> GetPath(Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode.Parent!=null)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }


 // Some tests form here

   public static void TestInitializeNodeGrid()
{
    int rows = 3;
    int columns = 3;
    Grid grid = new Grid(rows, columns);

   for (int x = 0; x < rows; x++)
    {
        for (int y = 0; y < columns; y++)
        {
            grid.SetValue(x, y, x * columns + y + 1); 
        }
    }


     Node[,] nodes = grid.NodeGrid;
    if (nodes != null && nodes.GetLength(0) == rows && nodes.GetLength(1) == columns)
    {
        Console.WriteLine("NodeGrid initialized correctly.");
        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                if (nodes[x, y] != null && nodes[x, y].X == x && nodes[x, y].Y == y)
                {
                    Console.WriteLine($"Node at ({x}, {y}) initialized correctly.");
                }
                else
                {
                    Console.WriteLine($"Node at ({x}, {y}) initialization failed.");
                }
            }
        }
    }
    else
    {
        Console.WriteLine("NodeGrid initialization failed.");
    }

      Node node = grid.NodeGrid[1, 1];
        node.AddNeighbors(grid);

        Console.WriteLine($"Node at (1, 1) has {node.Neighbors.Count} neighbors.");
        foreach (var neighbor in node.Neighbors)
        {
            Console.WriteLine($"Neighbor at ({neighbor.X}, {neighbor.Y}) ");
        }

        if (node.Neighbors.Count == 4 &&
            node.Neighbors.Exists(n => n.X == 2 && n.Y == 1) &&
            node.Neighbors.Exists(n => n.X == 0 && n.Y == 1) &&
            node.Neighbors.Exists(n => n.X == 1 && n.Y == 2) &&
            node.Neighbors.Exists(n => n.X == 1 && n.Y == 0))
        {
            Console.WriteLine("AddNeighbors test passed.");
        }
        else
        {
            Console.WriteLine("AddNeighbors test failed.");
        }
}
    
}




