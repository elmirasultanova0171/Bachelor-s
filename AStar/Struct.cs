using System;

public class Struct {

     public static void AStarStruct(ref GridS grid, ref NodeS start, ref NodeS end)
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
                return; 
            }

            openList.RemoveAt(lowestIndex);
            closedList.Add(current);
            neighbors.Clear();
            
            AddNeighbors( grid,  current,  neighbors);  
            
            
            for (int i = 0; i < neighbors.Count; i++)
            {
                NodeS neighbor = neighbors[i];
                if (closedList.Any(n => n.X == neighbor.X && n.Y == neighbor.Y) || neighbor.Wall)
                {
                    continue; 
                }

                int tempG = current.G + 1; 

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


    public static int Heuristic(NodeS a, NodeS b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }


       public static void AddNeighbors(GridS grid,  NodeS nodeS,  List<NodeS> neighbors)
    {
        int x = nodeS.X;
        int y = nodeS.Y;

        if (x < grid.Columns - 1)
        {
            neighbors.Add(grid.NodeGrid[x + 1, y]);
        }
        if (x > 0)
        {
            neighbors.Add(grid.NodeGrid[x - 1, y]);
        }
        if (y < grid.Rows - 1)
        {
            neighbors.Add(grid.NodeGrid[x, y + 1]);
        }
        if (y > 0)
        {
            neighbors.Add(grid.NodeGrid[x, y - 1]);
        }

        // Adding diagonal neighbors
        if (x < grid.Columns - 1 && y < grid.Rows - 1)
        {
            neighbors.Add(grid.NodeGrid[x + 1, y + 1]);
        }
        if (x < grid.Columns - 1 && y > 0)
        {
            neighbors.Add(grid.NodeGrid[x + 1, y - 1]);
        }
        if (x > 0 && y < grid.Rows - 1)
        {
            neighbors.Add(grid.NodeGrid[x - 1, y + 1]);
        }
        if (x > 0 && y > 0)
        {
            neighbors.Add(grid.NodeGrid[x - 1, y - 1]);
        }

    }


}