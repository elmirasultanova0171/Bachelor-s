using System;

public class AStarClassic
{
   
    public static void Main2(string[] args)
    {
        GridS gridS = new GridS(3, 3);
       
        NodeS startS = gridS.NodeGrid[0, 0];
        startS.Wall = false;
        NodeS endS = gridS.NodeGrid[2, 2];
        endS.Wall = false;


        Grid grid = new Grid(3, 3);
       
        Node start = grid.NodeGrid[0, 0];
        start.Wall = false;
        Node end = grid.NodeGrid[2, 2];
        end.Wall = false;
       
      //  AStarMemoryOptimizations.AStarStackAlloc(gridS, startS, endS);
          AStarMemoryOptimizations.AStarStruct(gridS, startS, endS);
        Console.WriteLine("done");
        //AStarNoVisuals(grid, start, end);
        //Console.WriteLine("done");
        
    }  
    

    public static void AStar(Grid grid, Node start, Node end, AStarVisualizer visualizer){
       
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

       // InitializeGrid(grid);

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
                grid.SetValue(start.X, start.Y, 2); 

                Node temp = current;
                while (temp.Parent != null)
                {
                    //Console.WriteLine($"Setting path node at ({temp.X}, {temp.Y})");
                    grid.SetValue(temp.X, temp.Y, 4); // Path node
                    temp = temp.Parent;
                } 
                grid.SetValue(end.X, end.Y, 3); 
                visualizer.Update(); // Final update to show the path
                Console.WriteLine("done");
                return;
            }

            openList.Remove(current);
            closedList.Add(current);
           
           if(current!=end){
            grid.SetValue(current.X, current.Y, 5); // closed color
            }
            current.AddNeighbors(grid);
            List<Node> neighbors = current.Neighbors;
            for (int i = 0; i < neighbors.Count; i++){
                Node neighbor = neighbors[i];

                if(!closedList.Contains(neighbor) && !neighbor.Wall){
                    int tempG = current.G + 1;         //assuming all nodes have a cost of 1
                    if(openList.Contains(neighbor)){
                        if(tempG < neighbor.G){
                            neighbor.G = tempG;
                        }
                    }else{
                        neighbor.G = tempG;
                        openList.Add(neighbor);
                        grid.SetValue(neighbor.X, neighbor.Y, 6); //next open color
                    }   
                    neighbor.H = Heuristic(neighbor, end);
                    // In the class: neighbor.F = neighbor.G + neighbor.H;
                    neighbor.Parent = current;
                   // Console.WriteLine(neighbor.X + "," + neighbor.Y);
                }

                
            }

             visualizer.Update();
            //Console.WriteLine("-----------------------------------------------");
             //grid.PrintGrid();
        }

        return;
    }
    
    public static void AStarNoVisuals(Grid grid, Node start, Node end){
       
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node[,] nodes = grid.NodeGrid;

        
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
               // grid.SetValue(start.X, start.Y, 2); 

                Node temp = current;
                while (temp.Parent != null)
                {
                    //Console.WriteLine($"Setting path node at ({temp.X}, {temp.Y})");
                  //  grid.SetValue(temp.X, temp.Y, 4); // Path node
                    temp = temp.Parent;
                } 
                //grid.SetValue(end.X, end.Y, 3); 
                //visualizer.Update(); // Final update to show the path
                //Console.WriteLine("done");
                return;
            }

            openList.Remove(current);
            closedList.Add(current);
           
            current.AddNeighbors(grid);
            List<Node> neighbors = current.Neighbors;
            for (int i = 0; i < neighbors.Count; i++){
                Node neighbor = neighbors[i];

                if(!closedList.Contains(neighbor) && !neighbor.Wall){
                    int tempG = current.G + 1;         //assuming all nodes have a cost of 1
                    if(openList.Contains(neighbor)){
                        if(tempG < neighbor.G){
                            neighbor.G = tempG;
                        }
                    }else{
                        neighbor.G = tempG;
                        openList.Add(neighbor);
                        //grid.SetValue(neighbor.X, neighbor.Y, 6); //next open color
                    }   
                    neighbor.H = Heuristic(neighbor, end);
                    // In the class: neighbor.F = neighbor.G + neighbor.H;
                    neighbor.Parent = current;
                   // Console.WriteLine(neighbor.X + "," + neighbor.Y);
                }

                
            }

            // visualizer.Update();
            //Console.WriteLine("-----------------------------------------------");
             //grid.PrintGrid();
           // Console.WriteLine(current.X + "," + current.Y);
        }

        return;
    }
    

    public static int Heuristic(Node a, Node b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
   
   //To visualize the path



   


 // Some tests form here

   public static void TestInitializeNodeGrid()
{
    int rows = 30;
    int columns = 30;
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

      Node node = grid.NodeGrid[20, 20];
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




