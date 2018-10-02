using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewModelLib;

namespace DrawLines
{
    public class MyPoint : ViewModelBase
    {
        private int x;
        private int y;
        private bool isClose;

        public MyPoint(int x,int y)
        {
            X = x;
            Y = y;
        }

        public MyPoint(Point point)
        {
            X = (int)Math.Round(point.X);
            Y = (int)Math.Round(point.Y);
        }

        public bool IsClose
        {
            get { return isClose; }
            set
            {
                isClose = value;
                OnPropertyChanged(nameof(IsClose));
            }
        }

        public int X
        {
            get { return x; }
            set
            {
                x = value;
                OnPropertyChanged(nameof(X));
            }
        }

        public int Y
        {
            get { return y; }
            set
            {
                y = value;
                OnPropertyChanged(nameof(Y));
            }
        }
    }
}
