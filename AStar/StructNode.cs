public struct NodeS
{
    public int X { get; set; }
    public int Y { get; set; }
    public int G { get; set; }
    public int H { get; set; }
    public int F => G + H;
    public int ParentX { get; set; }
    public int ParentY { get; set; }
    public bool Wall;

    public NodeS(int x, int y)
    {
        X = x;
        Y = y;
        G = 0;
        H = 0;
        ParentX = -1;
        ParentY = -1;

        Wall = false;

        

        Random random = new Random();
        if (random.Next(0, 100) < 20)
        {
            Wall = true;
        }

        
    }

     public bool Equals(NodeS other)
    {
        return this.X == other.X && this.Y == other.Y; 
    }

    public override bool Equals(object obj)
    {
        if (obj is NodeS)
            return Equals((NodeS)obj);
        return false;
    }
    
    public override int GetHashCode()    //to avoid using Any() and just use Contains()
    {
        return HashCode.Combine(X, Y); 
    }
}