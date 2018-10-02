using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace DrawLines
{
	public class DrawingLineBehavior : Behavior<FrameworkElement>
	{
		private const int AttachPointDistance = 15;

		private bool isDrawing;
		private MainWindowViewModel viewModel;

		private readonly List<MyPoint> currentShape = new List<MyPoint>();
		private MyPoint latest;

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
			viewModel = AssociatedObject.DataContext as MainWindowViewModel;
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.PreviewMouseLeftButtonDown -= AssociatedObject_PreviewMouseLeftButtonDown;
		}

		private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (viewModel == null)
			{
				return;
			}

			var pos = e.GetPosition(AssociatedObject);
			var x = pos.X;
			var y = pos.Y;

			if (isDrawing)
			{
				var closest = currentShape.First();//GetClosestPoint(x, y);
				if (closest != null
						&& Hypothenuse(x, y, closest.X, closest.Y) < AttachPointDistance)
				{
					// Attach to existing point
					isDrawing = false;

					AssociatedObject.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
					AssociatedObject.PreviewMouseRightButtonDown -= AssociatedObjectOnPreviewMouseRightButtonDown;

					viewModel.Lines.Add(
						new Line
						{
							Start = currentShape.Last(),
							End = closest
						});

					currentShape.ForEach(p => p.IsInShape = true);

					viewModel.StartX = 0;
					viewModel.StartY = 0;
					viewModel.CurrentX = 0;
					viewModel.CurrentY = 0;

					viewModel.CreatePoly(currentShape);

					currentShape.Clear();
					latest = null;
				}
				else
				{
					latest = new MyPoint(pos);

					viewModel.Points.Add(latest);
					viewModel.Lines.Add(
							new Line
							{
								Start = currentShape.Last(),
								End = latest
							});

					currentShape.Add(latest);

					viewModel.StartX = x;
					viewModel.StartY = y;
				}
			}
			else
			{
				isDrawing = true;
				viewModel.StartX = x;
				viewModel.StartY = y;
				viewModel.CurrentX = x;
				viewModel.CurrentY = y;
				latest = new MyPoint(pos);
				viewModel.Points.Add(latest);

				currentShape.Clear();
				currentShape.Add(latest);

				AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
				AssociatedObject.PreviewMouseRightButtonDown += AssociatedObjectOnPreviewMouseRightButtonDown;
			}
		}

		private void AssociatedObjectOnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			if (!isDrawing)
			{
				return;
			}

			if (currentShape.Any())
			{
				var currentCount = currentShape.Count;
				currentShape.Remove(latest);
				if (currentCount > 1)
				{
					// remove last line
					viewModel.Lines.Remove(viewModel.Lines.Last());
					viewModel.StartX = currentShape.Last().X;
					viewModel.StartY = currentShape.Last().Y;
					latest = currentShape.Last();
				}

				viewModel.Points.Remove(viewModel.Points.Last());

				if (!currentShape.Any())
				{
					latest = null;
					isDrawing = false;
					viewModel.StartX = 0;
					viewModel.StartY = 0;
					viewModel.CurrentX = 0;
					viewModel.CurrentY = 0;
					AssociatedObject.PreviewMouseMove -= AssociatedObject_PreviewMouseMove;
					AssociatedObject.PreviewMouseRightButtonDown -= AssociatedObjectOnPreviewMouseRightButtonDown;
				}
			}
		}

		private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (isDrawing)
			{
				var pos = e.GetPosition(AssociatedObject);
				var x = pos.X;
				var y = pos.Y;
				viewModel.CurrentX = pos.X;
				viewModel.CurrentY = pos.Y;
				var closest = GetClosestPoint(x, y);
				if (closest == null)
				{
					return;
				}

				closest = currentShape.First();

				if (Hypothenuse(x, y, closest.X, closest.Y) < AttachPointDistance)
				{
					viewModel.CurrentX = closest.X;
					viewModel.CurrentY = closest.Y;
				}

				//if (Hypothenuse(x, y, closest.X, closest.Y) < AttachPointDistance)
				//{
				//	viewModel.CurrentX = closest.X;
				//	viewModel.CurrentY = closest.Y;
				//}
			}
		}

		public MyPoint GetClosestPoint(double x, double y)
		{
			if (!currentShape.Any())
			{
				return null;
			}

			MyPoint closest = currentShape.First();

			double closestDistance = Hypothenuse(x, y, closest.X, closest.Y);

			foreach (var point in currentShape)
			{
				var currentHyp = Hypothenuse(x, y, point.X, point.Y);
				if (currentHyp < closestDistance)
				{
					closest = point;
					closestDistance = currentHyp;
				}
			}

			if (closest == currentShape.Last())
			{
				return null;
			}

			return closest;
		}

		public double Hypothenuse(double x1, double y1, double x2, double y2)
		{
			return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
		}
	}
}
