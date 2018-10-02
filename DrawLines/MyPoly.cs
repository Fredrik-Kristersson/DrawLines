using System.Windows.Media;
using ViewModelLib;

namespace DrawLines
{
	public class MyPoly : ViewModelBase
	{
		public MyPoly(PointCollection points)
		{
			Points = points;
		}

		public PointCollection Points
		{
			get => Get<PointCollection>();
			set => Set(value);
		}
	}
}
