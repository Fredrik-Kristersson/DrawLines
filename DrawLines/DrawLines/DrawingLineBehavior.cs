using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace DrawLines
{
	public class DrawingLineBehavior : Behavior<FrameworkElement>
	{
        private const int AttachPointDistance = 20;

		private bool isDrawing;
        private MainWindowViewModel viewModel;

        protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.PreviewMouseLeftButtonDown += AssociatedObject_PreviewMouseLeftButtonDown;
            viewModel = AssociatedObject.DataContext as MainWindowViewModel;
        }

		private void AssociatedObject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			if(viewModel == null)
			{
				return;
			}

            var pos = e.GetPosition(AssociatedObject);
            var x = (int)Math.Round(pos.X);
            var y = (int)Math.Round(pos.Y);

            if (isDrawing)
			{
                
                
                var closest = viewModel.GetClosestPoint(x,y);
                if (closest != null 
                    && viewModel.Hypothenuse(x, y, closest.X, closest.Y) < AttachPointDistance)
                {
                    // Attach to existing point
                    isDrawing = false;
                    viewModel.Lines.Add(
                        new Line
                        {
                            Start = new MyPoint(viewModel.StartX, viewModel.StartY),
                            End = new MyPoint(closest.X, closest.Y)
                        });
                    viewModel.StartX = 0;
                    viewModel.StartY = 0;
                    viewModel.CurrentX = 0;
                    viewModel.CurrentY = 0;
                    foreach (var point in viewModel.Points)
                    {
                        point.IsClose = false;
                    }
                }
                else
                {
                    viewModel.Points.Add(new MyPoint(pos));
                    viewModel.Lines.Add(
                        new Line
                        {
                            Start = new MyPoint(viewModel.StartX, viewModel.StartY),
                            End = new MyPoint(viewModel.CurrentX, viewModel.CurrentY)
                        });

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
                viewModel.Points.Add(new MyPoint(pos));

                AssociatedObject.PreviewMouseMove += AssociatedObject_PreviewMouseMove;
			}
		}

        private void AssociatedObject_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                var pos = e.GetPosition(AssociatedObject);
                var x = (int)Math.Round(pos.X);
                var y = (int)Math.Round(pos.Y);
                viewModel.CurrentX = (int)Math.Round(pos.X);
                viewModel.CurrentY = (int)Math.Round(pos.Y);
                var closest = viewModel.GetClosestPoint(x, y);
                if (closest == null)
                {
                    return;
                }

                if (viewModel.Hypothenuse(x, y, closest.X, closest.Y) < AttachPointDistance)
                {
                    closest.IsClose = true;
                }
                else
                {
                    closest.IsClose = false;
                }
            }
        }
    }
}
