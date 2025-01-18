using System;

public class AStarMemoryOptimizations
{
    
    public static void AStarBlocking(Grid grid, Node start, Node end, AStarVisualizer visualizer)
    {
       
       List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

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
           
           if(current!=end){
            //grid.SetValue(current.X, current.Y, 5); // closed color
            }
            
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
        }

        return;
       
    }
    
    
    
    
    
    public static int Heuristic(Node a, Node b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}