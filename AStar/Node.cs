public class Node
{
    public int X { get; set; }
    public int Y { get; set; }
    public int G { get; set; }
    public int H { get; set; }
    public int F => G + H;
    public Node? Parent { get; set; }
    public List<Node> Neighbors { get; set; }

    public Node(int x, int y)
    {
        X = x;
        Y = y;
       this.Neighbors = new List<Node>();
    }

    public void AddNeighbors(Grid grid) {    //can only handle grids that are square
        var x = this.X;
        var y = this.Y;

        if (x < grid.Columns-1){
            this.Neighbors.Add(grid.NodeGrid[x+1, y]);
        }
        if (x > 0){
            this.Neighbors.Add(grid.NodeGrid[x-1, y]);
        }
        if (y < grid.Rows-1){
            this.Neighbors.Add(grid.NodeGrid[x, y+1]);
        }
        if (y > 0){
            this.Neighbors.Add(grid.NodeGrid[x, y-1]);
        }  

        // Adding diagonal neighbors
        if (x < grid.Rows - 1 && y < grid.Columns - 1){
            Neighbors.Add(grid.NodeGrid[x + 1, y + 1]);
        }
        if (x < grid.Rows - 1 && y > 0){
            Neighbors.Add(grid.NodeGrid[x + 1, y - 1]);
        }
        if (x > 0 && y < grid.Columns - 1){
            Neighbors.Add(grid.NodeGrid[x - 1, y + 1]);
        }
        if (x > 0 && y > 0){
            Neighbors.Add(grid.NodeGrid[x - 1, y - 1]);
        }             
    }

  
}