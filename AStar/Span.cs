using System;
public class Span{

    public static void AStarSpan(ref GridS grid, ref NodeS start, ref NodeS end)
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

             NodeS current = openList[lowestIndex]; // Pass by reference, no copying


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
            AddNeighbors(ref grid, ref current, ref neighbors);  
            
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
                    neighbor.H = Heuristic(neighbor, end);
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
                        existingNode.H = Heuristic(existingNode, end);
                        existingNode.ParentX = current.X;
                        existingNode.ParentY = current.Y;
                    }
                }
            }

           // Console.WriteLine(current.X + "," + current.Y);
        }

        return; // No path found
    }

    public static void AStarSpanWithHashSet(ref GridS grid, ref NodeS start, ref NodeS end)
    {
        List<NodeS> openList = new List<NodeS>();
        HashSet<NodeS> closedList = new HashSet<NodeS>();
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

             NodeS current = openList[lowestIndex]; // Pass by reference, no copying


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
            AddNeighbors(ref grid, ref current, ref neighbors);  
            
            for (int i = 0; i < neighbors.Length; i++)
            {
                //Span?
                NodeS neighbor = neighbors[i];
                if (closedList.Contains(neighbor) || neighbor.Wall)
                {
                    continue; 
                }

                int tempG = current.G + 1; // Assuming uniform cost for neighbors

                if (!closedList.Contains(neighbor))
                {
                    // New node: add it
                    neighbor.G = tempG;
                    neighbor.H = Heuristic(neighbor, end);
                    neighbor.ParentX = current.X;
                    neighbor.ParentY = current.Y;
                    openList.Add(neighbor);
                }
                else
                {
                    
                    NodeS existingNode = openList.First(n => n.X == neighbor.X && n.Y == neighbor.Y);
                    if (tempG < existingNode.G)
                    {
                        existingNode.G = tempG;
                        existingNode.H = Heuristic(existingNode, end);
                        existingNode.ParentX = current.X;
                        existingNode.ParentY = current.Y;
                    }
                }
            }

           // Console.WriteLine(current.X + "," + current.Y);
        }

        return; // No path found
    }
    
    public static int Heuristic(NodeS a, NodeS b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

     public static void AddNeighbors(ref GridS grid, ref NodeS nodeS, ref Span<NodeS> neighbors)
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
    }

}