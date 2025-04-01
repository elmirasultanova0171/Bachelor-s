using System;
using System.Numerics;

public class AStarVectorization
{
    



 public static void AStarSIMD(ref GridS grid, ref NodeS start, ref NodeS end)
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
            
            AddNeighbors(grid, current, neighbors);  
            HeuristicSIMD(neighbors, end); // Calculate heuristic for all neighbors
            for (int i = 0; i < neighbors.Length; i++)
            {
                NodeS neighbor = neighbors[i];
                if (closedList.Any(n => n.X == neighbor.X && n.Y == neighbor.Y) || neighbor.Wall)
                {
                    continue; 
                }

                int tempG = current.G + 1; // Assuming uniform cost for neighbors

                if (!openList.Any(n => n.X == neighbor.X && n.Y == neighbor.Y))
                {
                    neighbor.G = tempG;
                    
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
                        
                        existingNode.ParentX = current.X;
                        existingNode.ParentY = current.Y;
                    }
                }
            }

           // Console.WriteLine(current.X + "," + current.Y);
        }

        return; // No path found
    }

    public static void AddNeighbors(GridS grid, NodeS nodeS, Span<NodeS> neighbors)
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

    public static void HeuristicSIMD(Span<NodeS> neighbors, NodeS end)
    {

        int vectorSize = Vector<int>.Count; 
        int count = neighbors.Length / vectorSize * vectorSize;

        Span<int> heuristicValues = stackalloc int[vectorSize];

        for (int i = 0; i < count; i += vectorSize)
        {
            Vector<int> xDiff = new Vector<int>(neighbors.Slice(i).ToArray().Select(n => Math.Abs(n.X - end.X)).ToArray());
            Vector<int> yDiff = new Vector<int>(neighbors.Slice(i).ToArray().Select(n => Math.Abs(n.Y - end.Y)).ToArray());

            (xDiff + yDiff).CopyTo(heuristicValues);

            for (int j = 0; j < vectorSize; j++)
            {
                neighbors[i + j].H = heuristicValues[j];
            }
        }


            // in case some neighbors did not fit into registers 
        for (int i = count; i < neighbors.Length; i++)
        {
            neighbors[i].H = Math.Abs(neighbors[i].X - end.X) + Math.Abs(neighbors[i].Y - end.Y);
        }


    }
   












}
