using System.Windows;
using ViewModelLib;

namespace DrawLines
{
    public class Line : ViewModelBase
    {
        private MyPoint start;
        private MyPoint end;

        public MyPoint Start
        {
            get { return start; }
            set
            {
                start = value;
                OnPropertyChanged(nameof(Start));
            }
        }

        public MyPoint End
        {
            get { return end; }
            set
            {
                end = value;
                OnPropertyChanged(nameof(End));
            }
        }
    }
}