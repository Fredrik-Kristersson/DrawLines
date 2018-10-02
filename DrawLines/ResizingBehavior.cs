using System.Windows;
using System.Windows.Interactivity;

namespace DrawLines
{
	public class ResizingBehavior : Behavior<FrameworkElement>
	{
		private MainWindowViewModel viewModel;

		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.SizeChanged += AssociatedObjectOnSizeChanged;
			viewModel = AssociatedObject.DataContext as MainWindowViewModel;
		}

		private void AssociatedObjectOnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			viewModel.Resize(e.PreviousSize, e.NewSize);
		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			AssociatedObject.SizeChanged -= AssociatedObjectOnSizeChanged;
		}
	}
}
