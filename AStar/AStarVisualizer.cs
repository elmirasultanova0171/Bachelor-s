using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class AStarVisualizer
{
    private const int CellSize = 20; // Size of each cell in pixels
    private RenderWindow window;
    private Grid grid;

    public AStarVisualizer(Grid grid)
    {
        this.grid = grid;
        window = new RenderWindow(new VideoMode((uint)(grid.Columns * CellSize), (uint)(grid.Rows * CellSize)), "A* Visualizer");
        window.Closed += WindowClosed;
    }

    private void WindowClosed(object? sender, EventArgs e)
    {
        window.Close();
    }

    public void Run()
    {
        while (window.IsOpen)
        {
            window.DispatchEvents();
            window.Clear(Color.White);

            DrawGrid();

            window.Display();
        }
    }

    public void DrawGrid()
    {
        for (int x = 0; x < grid.Rows; x++)
        {
            for (int y = 0; y < grid.Columns; y++)
            {
                RectangleShape cell = new RectangleShape(new Vector2f(CellSize, CellSize))
                {
                    Position = new Vector2f(y * CellSize, x * CellSize),
                    FillColor = GetCellColor(grid.GetValue(x, y)),
                    OutlineColor = Color.Black, // Set the outline color to black
                    OutlineThickness = 1 // Set the outline thickness
                };
                window.Draw(cell);
            }
        }
    }

    private Color GetCellColor(int value)
    {
        switch (value)
        {
            case 1: return Color.Black; // Obstacle
            case 2: return Color.Green; // Start
            case 3: return Color.Red;   // End
            case 4: return Color.Blue;  // Path
            case 5: return Color.Yellow; // Closed list
            case 6: return Color.Cyan;  // Open list
            default: return Color.White; // Empty
        }
    }

    public void Update()
    {
        window.DispatchEvents();
        window.Clear(Color.White);
        DrawGrid();
        window.Display();
    }
}