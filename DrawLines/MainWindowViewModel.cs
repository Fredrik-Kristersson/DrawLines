using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using ViewModelLib;

namespace DrawLines
{
	public class MainWindowViewModel : ViewModelBase
	{
		public MainWindowViewModel()
		{
			Lines = new ObservableCollection<Line>();
			Points = new ObservableCollection<MyPoint>();
			Polys = new ObservableCollection<MyPoly>();
		}

		public ObservableCollection<MyPoly> Polys
		{
			get => Get<ObservableCollection<MyPoly>>();
			set => Set(value);
		}

		public ObservableCollection<Line> Lines
		{
			get => Get<ObservableCollection<Line>>();
			set => Set(value);
		}

		public ObservableCollection<MyPoint> Points
		{
			get => Get<ObservableCollection<MyPoint>>();
			set => Set(value);
		}

		public double StartX
		{
			get => Get<double>();
			set => Set(value);
		}

		public double StartY
		{
			get => Get<double>();
			set => Set(value);
		}

		public double CurrentX
		{
			get => Get<double>();
			set => Set(value);
		}

		public double CurrentY
		{
			get => Get<double>();
			set => Set(value);
		}

		public void CreatePoly(List<MyPoint> points)
		{
			var polyPoints = new PointCollection(points.Select(p => new Point(p.X, p.Y)));

			Polys.Add(new MyPoly(polyPoints));

			Lines.Clear();
			Points.Clear();
		}

		public void Resize(Size previousSize, Size newSize)
		{
			var xFactor = newSize.Width / previousSize.Width;
			var yFactor = newSize.Height / previousSize.Height;

			foreach (var line in Lines)
			{
				line.Start.X *= xFactor;
				line.Start.Y *= yFactor;
				line.End.X *= xFactor;
				line.End.Y *= yFactor;
			}

			foreach (var point in Points)
			{
				point.X *= xFactor;
				point.Y *= yFactor;
			}

			var newPolys = new ObservableCollection<MyPoly>();
			foreach (var poly in Polys)
			{
				var newPoly = new List<Point>();
				foreach (var point in poly.Points)
				{
					var x = point.X * xFactor;
					var y = point.Y * yFactor;
					newPoly.Add(new Point(x, y));
				}

				newPolys.Add(new MyPoly(new PointCollection(newPoly)));
			}

			Polys = newPolys;

			CurrentX *= xFactor;
			CurrentY *= yFactor;
			StartX *= xFactor;
			StartY *= yFactor;
		}
	}
}
