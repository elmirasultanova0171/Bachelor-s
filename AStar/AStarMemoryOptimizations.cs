using System;

public class AStarMemoryOptimizations
{
    
    public static void AStarBlocking(Grid grid, Node start, Node end, int tileSize, int threshold)
    {
       
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node[,] nodes = grid.NodeGrid;
        
        openList.Add(start);

        while(openList.Count > 0){

           int lowestIndex = 0;

           if (openList.Count > threshold)
            {
                for (int i = 0; i < openList.Count; i += tileSize)
                {
                    for (int j = i; j < Math.Min(i + tileSize, openList.Count); j++)
                    {
                        if (openList[j].F < openList[lowestIndex].F)
                        {
                            lowestIndex = j;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < openList.Count; i++)
                {
                    if (openList[i].F < openList[lowestIndex].F)
                    {
                        lowestIndex = i;
                    }
                }
                
            }

            Node current = openList[lowestIndex];

            if(current==end){ 

                Node temp = current;
                while (temp.Parent != null)
                {
                    temp = temp.Parent;
                }
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
                    }   
                    neighbor.H = Heuristic(neighbor, end);
                    neighbor.Parent = current;
                }

                
            }

        }

        return;
       
    }
    
   

    public static void AStarStackAlloc(GridS grid, NodeS start, NodeS end)
    {
        List<NodeS> openList = new List<NodeS>();
        List<NodeS> closedList = new List<NodeS>();
        Span<NodeS> neighbors = stackalloc NodeS[8];


        openList.Add(start);

        while (openList.Count > 0)
        {
            int lowestIndex = 0;
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].F < openList[lowestIndex].F)
                {
                    lowestIndex = i;
                }
            }

            NodeS current = openList[lowestIndex];

            if (current.X == end.X && current.Y == end.Y)
            {
                NodeS temp = current;
                while (temp.ParentX != -1 && temp.ParentY != -1)
                {
                    int parentX = temp.ParentX;
                    int parentY = temp.ParentY;
                    temp = grid.NodeGrid[parentX, parentY];
                }
                return; // Path found, exit.
            }

            openList.RemoveAt(lowestIndex);
            closedList.Add(current);
            
            int neighborCount = AddNeighbors(grid, current, neighbors);  
            
            for (int i = 0; i < neighbors.Length; i++)
            {
                //Span?
                NodeS neighbor = neighbors[i];
                if (closedList.Any(n => n.X == neighbor.X && n.Y == neighbor.Y) || neighbor.Wall)
                {
                    continue; 
                }

                int tempG = current.G + 1; // Assuming uniform cost for neighbors

                if (!openList.Any(n => n.X == neighbor.X && n.Y == neighbor.Y))
                {
                    // New node: add it
                    neighbor.G = tempG;
                    neighbor.H = HeuristicS(neighbor, end);
                    neighbor.ParentX = current.X;
                    neighbor.ParentY = current.Y;
                    openList.Add(neighbor);
                }
                else
                {
                    
                    NodeS existingNode = openList.First(n => n.X == neighbor.X && n.Y == neighbor.Y);
                    if (tempG < existingNode.G)
                    {
                        // Update G, H, and parent if this path is better
                        existingNode.G = tempG;
                        existingNode.H = HeuristicS(existingNode, end);
                        existingNode.ParentX = current.X;
                        existingNode.ParentY = current.Y;
                    }
                }
            }

           // Console.WriteLine(current.X + "," + current.Y);
        }

        return; // No path found
    }


    public static void AStarStruct(GridS grid, NodeS start, NodeS end)
{
    
        List<NodeS> openList = new List<NodeS>();
        List<NodeS> closedList = new List<NodeS>();
        List<NodeS> neighbors = new List<NodeS>();


        openList.Add(start);

        while (openList.Count > 0)
        {
            int lowestIndex = 0;
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].F < openList[lowestIndex].F)
                {
                    lowestIndex = i;
                }
            }

            NodeS current = openList[lowestIndex];

            if (current.X == end.X && current.Y == end.Y)
            {
                NodeS temp = current;
                while (temp.ParentX != -1 && temp.ParentY != -1)
                {
                    int parentX = temp.ParentX;
                    int parentY = temp.ParentY;
                    temp = grid.NodeGrid[parentX, parentY];
                }
                return; // Path found, exit.
            }

            openList.RemoveAt(lowestIndex);
            closedList.Add(current);
            neighbors.Clear();
            int neighborCount = AddNeighbors2(grid, current, neighbors);  
            
            for (int i = 0; i < neighbors.Count; i++)
            {
                //Span?
                NodeS neighbor = neighbors[i];
                if (closedList.Any(n => n.X == neighbor.X && n.Y == neighbor.Y) || neighbor.Wall)
                {
                    continue; 
                }

                int tempG = current.G + 1; // Assuming uniform cost for neighbors

                if (!openList.Any(n => n.X == neighbor.X && n.Y == neighbor.Y))
                {
                    // New node: add it
                    neighbor.G = tempG;
                    neighbor.H = HeuristicS(neighbor, end);
                    neighbor.ParentX = current.X;
                    neighbor.ParentY = current.Y;
                    openList.Add(neighbor);
                }
                else
                {
                    
                    NodeS existingNode = openList.First(n => n.X == neighbor.X && n.Y == neighbor.Y);
                    if (tempG < existingNode.G)
                    {
                        // Update G, H, and parent if this path is better
                        existingNode.G = tempG;
                        existingNode.H = HeuristicS(existingNode, end);
                        existingNode.ParentX = current.X;
                        existingNode.ParentY = current.Y;
                    }
                }
            }

           // Console.WriteLine(current.X + "," + current.Y);
        }

        return; // No path found
}









    public static int AddNeighbors2(GridS grid, NodeS nodeS, List<NodeS> neighbors)
    {
        int x = nodeS.X;
        int y = nodeS.Y;
        int count = 0;

        if (x < grid.Columns - 1)
        {
            neighbors.Add(grid.NodeGrid[x + 1, y]);
            count++;
        }
        if (x > 0)
        {
            neighbors.Add(grid.NodeGrid[x - 1, y]);
            count++;
        }
        if (y < grid.Rows - 1)
        {
            neighbors.Add(grid.NodeGrid[x, y + 1]);
            count++;
        }
        if (y > 0)
        {
            neighbors.Add(grid.NodeGrid[x, y - 1]);
            count++;
        }

        // Adding diagonal neighbors
        if (x < grid.Columns - 1 && y < grid.Rows - 1)
        {
            neighbors.Add(grid.NodeGrid[x + 1, y + 1]);
            count++;
        }
        if (x < grid.Columns - 1 && y > 0)
        {
            neighbors.Add(grid.NodeGrid[x + 1, y - 1]);
            count++;
        }
        if (x > 0 && y < grid.Rows - 1)
        {
            neighbors.Add(grid.NodeGrid[x - 1, y + 1]);
            count++;
        }
        if (x > 0 && y > 0)
        {
            neighbors.Add(grid.NodeGrid[x - 1, y - 1]);
            count++;
        }

        return count; // Return how many valid neighbors were added
    }


   public static int AddNeighbors(GridS grid, NodeS nodeS, Span<NodeS> neighbors)
    {
        int x = nodeS.X;
        int y = nodeS.Y;
        int count = 0;

        if (x < grid.Columns - 1)
        {
            neighbors[count++] = grid.NodeGrid[x + 1, y];
        }
        if (x > 0)
        {
            neighbors[count++] = grid.NodeGrid[x - 1, y];
        }
        if (y < grid.Rows - 1)
        {
            neighbors[count++] = grid.NodeGrid[x, y + 1];
        }
        if (y > 0)
        {
            neighbors[count++] = grid.NodeGrid[x, y - 1];
        }

        // Adding diagonal neighbors
        if (x < grid.Columns - 1 && y < grid.Rows - 1)
        {
            neighbors[count++] = grid.NodeGrid[x + 1, y + 1];
        }
        if (x < grid.Columns - 1 && y > 0)
        {
            neighbors[count++] = grid.NodeGrid[x + 1, y - 1];
        }
        if (x > 0 && y < grid.Rows - 1)
        {
            neighbors[count++] = grid.NodeGrid[x - 1, y + 1];
        }
        if (x > 0 && y > 0)
        {
            neighbors[count++] = grid.NodeGrid[x - 1, y - 1];
        }

        return count; // Return how many valid neighbors were added
    }

    
    public static int Heuristic(Node a, Node b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    public static int HeuristicS(NodeS a, NodeS b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}