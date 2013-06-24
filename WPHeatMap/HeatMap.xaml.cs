using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPHeatMap
{
    /// <summary>
    /// Interaction logic for HeatMap.xaml
    /// </summary>
    public partial class HeatMap : UserControl
    {
        public HeatMap()
        {
            InitializeComponent();
        }
        protected override void OnRender(DrawingContext dc)
        {
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();
            mySolidColorBrush.Color = Colors.LimeGreen;
            Pen myPen = new Pen(Brushes.Blue, 10);
            Rect myRect = new Rect(0, 0, 500, 500);
            dc.DrawRectangle(mySolidColorBrush, myPen, myRect);
        }
    }
}
