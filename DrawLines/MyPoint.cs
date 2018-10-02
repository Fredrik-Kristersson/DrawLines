using System.Windows;
using ViewModelLib;

namespace DrawLines
{
	public class MyPoint : ViewModelBase
	{
		private double percentageX;
		private double percentageY;

		public MyPoint(double x, double y)
		{
			X = x;
			Y = y;
		}

		public MyPoint(Point point) : this(point.X, point.Y)
		{
		}

		public double X
		{
			get => Get<double>();
			set => Set(value);
		}

		public double Y
		{
			get => Get<double>();
			set => Set(value);
		}

		public bool IsInShape
		{
			get => Get<bool>();
			set => Set(value);
		}
	}
}
