using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FiguresLib
{
    public class Point : GeometricFigure
    {
        public double Area { get; }

        public Point() : base(0, 0) // конструктор без параметров
        {
            Area = CalculateArea();
        }

        public Point(double centerX, double centerY) : base(centerX, centerY)
        {
            Area = CalculateArea();
        }

        public override double[] BoundingRectangle
        {
            get
            {
                return new double[] { CenterX, CenterY, CenterX, CenterY };
            }
        }

        public override double CalculateArea()
        {
            return 0;
        }
        public override string ToString()
        {
            return $"({CenterX}, {CenterY})";
        }

    }

}
