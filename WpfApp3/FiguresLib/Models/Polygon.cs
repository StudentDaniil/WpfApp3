using System.Linq;
using System;
using System.Collections.Generic;

namespace FiguresLib
{
    public class Polygon : GeometricFigure
    {
        public List<Point> Vertices { get; set; }
        public double Area { get; }

        public Polygon() : base(0, 0) // конструктор без параметров
        {
            Vertices = new List<Point>();
            Area = CalculateArea();
        }

        public Polygon(List<Point> vertices) : base(CalculateCenterX(vertices), CalculateCenterY(vertices))
        {
            Vertices = vertices;
            Area = CalculateArea();
            if (Vertices.Count < 3)
            {
                throw new ArgumentException("A polygon must have at least 3 vertices.");
            }
        }

        private static double CalculateCenterX(List<Point> vertices)
        {
            return vertices.Average(v => v.CenterX);
        }

        private static double CalculateCenterY(List<Point> vertices)
        {
            return vertices.Average(v => v.CenterY);
        }

        public override double[] BoundingRectangle
        {
            get
            {
                double left = double.MaxValue;
                double top = double.MaxValue;
                double right = double.MinValue;
                double bottom = double.MinValue;

                foreach (var vertex in Vertices)
                {
                    left = Math.Min(left, vertex.CenterX);
                    top = Math.Min(top, vertex.CenterY);
                    right = Math.Max(right, vertex.CenterX);
                    bottom = Math.Max(bottom, vertex.CenterY);
                }

                return new double[] { left, top, right, bottom };
            }
        }

        public override double CalculateArea()
        {
            List<Point> sortedVertices = SortVertices(Vertices);

            double area = 0;
            int n = sortedVertices.Count;

            for (int i = 0; i < n - 1; i++)
            {
                area += (sortedVertices[i].CenterX * sortedVertices[i + 1].CenterY - sortedVertices[i + 1].CenterX * sortedVertices[i].CenterY);
            }

            area += (sortedVertices[n - 1].CenterX * sortedVertices[0].CenterY - sortedVertices[0].CenterX * sortedVertices[n - 1].CenterY);

            return Math.Abs(area / 2);
        }

        private List<Point> SortVertices(List<Point> vertices)
        {
            int leftmostIndex = 0;
            for (int i = 1; i < vertices.Count; i++)
            {
                if (vertices[i].CenterX < vertices[leftmostIndex].CenterX ||
                    (vertices[i].CenterX == vertices[leftmostIndex].CenterX && vertices[i].CenterY < vertices[leftmostIndex].CenterY))
                {
                    leftmostIndex = i;
                }
            }

            (vertices[leftmostIndex], vertices[0]) = (vertices[0], vertices[leftmostIndex]);

            vertices.Sort((v1, v2) => Angle(v1, v2).CompareTo(Angle(v2, v1)));

            return vertices;
        }

        private double Angle(Point v1, Point v2)
        {
            if (v1 == null || v2 == null || Vertices == null)
            {
                throw new ArgumentNullException("v1, v2 or Vertices cannot be null.");
            }

            double dx1 = v1.CenterX - Vertices[0].CenterX;
            double dy1 = v1.CenterY - Vertices[0].CenterY;
            double dx2 = v2.CenterX - Vertices[0].CenterX;
            double dy2 = v2.CenterY - Vertices[0].CenterY;

            double dotProduct = dx1 * dx2 + dy1 * dy2;
            double length1 = Math.Sqrt(dx1 * dx1 + dy1 * dy1);
            double length2 = Math.Sqrt(dx2 * dx2 + dy2 * dy2);

            double angle = Math.Acos(dotProduct / (length1 * length2));

            if (dx1 * dy2 - dy1 * dx2 < 0)
            {
                angle = 2 * Math.PI - angle;
            }

            return angle;
        }
    }
}