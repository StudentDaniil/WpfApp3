using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiguresLib{
    public abstract class GeometricFigure
    {
        public double CenterX { get; set; }
        public double CenterY { get; set; }

        public GeometricFigure(double centerX, double centerY)
        {
            CenterX = centerX;
            CenterY = centerY;
        }

        public abstract double[] BoundingRectangle { get; }

        public abstract double CalculateArea();
    } 
}