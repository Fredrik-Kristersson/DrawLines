using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace DrawLines
{
	public class MainWindowViewModel : ViewModelBase
	{
        private int startX;
        private int startY;
        private int currentX;
        private int currentY;
        private ObservableCollection<MyPoint> points = new ObservableCollection<MyPoint>();

        private ObservableCollection<Line> lines = new ObservableCollection<Line>();

        public MainWindowViewModel() 
		{
		
		}

        public ObservableCollection<Line> Lines
        {
            get { return lines; }
            set
            {
                lines = value;
                OnPropertyChanged(nameof(Lines));
            }
        }

        public ObservableCollection<MyPoint> Points
        {
            get { return points; }
            set
            {
                points = value;
                OnPropertyChanged(nameof(Points));
            }
        }

		public int StartX 
		{
			get { return startX; }
			set 
			{
				startX = value;
				OnPropertyChanged(nameof(StartX));
			}
		}

        public int StartY
        {
            get { return startY; }
            set
            {
                startY = value;
                OnPropertyChanged(nameof(StartY));
            }
        }

        public int CurrentX
        {
            get { return currentX; }
            set
            {
                currentX = value;
                OnPropertyChanged(nameof(CurrentX));
            }
        }

        public int CurrentY
        {
            get { return currentY; }
            set
            {
                currentY = value;
                OnPropertyChanged(nameof(CurrentY));
            }
        }

        public MyPoint GetClosestPoint(int x, int y)
        {
            MyPoint closest = Points.FirstOrDefault();
            if (!Points.Any())
            {
                return null;
            }

            double closestDistance = Hypothenuse(x, y, closest.X, closest.Y);
                
            foreach (var point in Points)
            {
                var currentHyp = Hypothenuse(x, y, point.X, point.Y);
                if (currentHyp < closestDistance)
                {
                    closest = point;
                    closestDistance = currentHyp;
                }
            }

            if (closest == Points.Last())
            {
                return null;
            }

            return closest;
        }

        public double Hypothenuse(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }
    }
}
