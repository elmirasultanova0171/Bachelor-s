using System;

public class Grid
{
    public int Rows { get; set; } 
    public int Columns { get; set; }
    private int[,] matrix; 
    public Node[,] NodeGrid { get; set; } = null!;
 

    public Grid(int rows, int columns) { 
        this.Rows = rows; 
        this.Columns = columns;
        matrix = new int[rows, columns]; 
       
           InitializeNodeGrid();   
    }

    public Grid(int rows, int columns, bool x, int blockSize) { 
        this.Rows = rows; 
        this.Columns = columns;
        if(x){
            matrix = new int[rows, columns]; 
            InitializeNodeGridBlocking(blockSize);   
        }
        else{
            matrix = new int[rows, columns]; 
        }
    }

    public void SetValue(int row, int column, int value)
    {
        if (row >= 0 && row < matrix.GetLength(0) && column >= 0 && column < matrix.GetLength(1))
        {
            matrix[row, column] = value;
        }
        else
        {
            throw new ArgumentOutOfRangeException("Row or column is out of range.");
        }
    }

    public int GetValue(int row, int column)
    {
        if (row >= 0 && row < matrix.GetLength(0) && column >= 0 && column < matrix.GetLength(1))
        {
            return matrix[row, column];
        }
        else
        {
            throw new ArgumentOutOfRangeException("Row or column is out of range.");
        }
    }

    public void PrintGrid()
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write(matrix[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }

    private void InitializeNodeGrid() { 
        NodeGrid = new Node[matrix.GetLength(0), matrix.GetLength(1)]; 
        for (int x = 0; x < matrix.GetLength(0); x++) { 
            for (int y = 0; y < matrix.GetLength(1); y++) { 
                NodeGrid[x, y] = new Node(x, y);
                if(NodeGrid[x, y].Wall){
                    SetValue(x, y, 1); // color walls black
                }
                 // You can also initialize other properties here if needed 
            } 
        } 
    }

    private void InitializeNodeGridBlocking(int blockSize) { 
        NodeGrid = new Node[matrix.GetLength(0), matrix.GetLength(1)]; 
        for (int xb = 0; xb < matrix.GetLength(0); xb += blockSize)
        {
            for (int yb = 0; yb < matrix.GetLength(1); yb += blockSize)
            {
                // Process a small block at a time
                for (int x = xb; x < Math.Min(xb + blockSize, matrix.GetLength(0)); x++)
                {
                    for (int y = yb; y < Math.Min(yb + blockSize, matrix.GetLength(1)); y++)
                    {
                        NodeGrid[x, y] = new Node(x, y);

                        if (NodeGrid[x, y].Wall)
                        {
                            SetValue(x, y, 1); // Color walls black
                        }
                    }
                }
            }
        }

    }



}



