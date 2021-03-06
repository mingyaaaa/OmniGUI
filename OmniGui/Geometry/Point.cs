namespace OmniGui.Geometry
{
    public struct Point
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }
        public static Point Zero => new Point(0, 0);
        
        public Point Offset(Point offset)
        {
            return new Point(offset.X + X, offset.Y + Y);
        }

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        }

        public static Point operator +(Point point, Vector vector)
        {
            return new Point(point.X + vector.X, point.Y + vector.Y);
        }

        public static Point operator -(Point point, Vector vector)
        {
            return new Point(point.X - vector.X, point.Y - vector.Y);
        }

        public static Point operator -(Point point)
        {
            return new Point(-point.X, -point.Y);
        }

        public static explicit operator Vector(Point a)
        {
            return new Vector(a.X, a.Y);
        }
    }
}