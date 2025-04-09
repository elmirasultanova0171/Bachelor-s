using System;
public class Blocking{
    
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
    
   
    public static int Heuristic(Node a, Node b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }




}