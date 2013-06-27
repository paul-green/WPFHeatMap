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
using System.IO;
using System.Runtime.InteropServices;

namespace WPHeatMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

       
         

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DepthRange dr = new DepthRange("Data\\EURUSD_20130607_EBS_secondly.csv");

            DateTime inputDateTime;
            DateTime startDateTime;
            DateTime endDateTime;
            if (DateTime.TryParse(StartDateTime.Text, out inputDateTime))
            {
                TimeSpan timeWindow;
                if (TimeSpan.TryParse(TimeWindow.Text, out timeWindow))
                {
                    startDateTime = inputDateTime;
                    endDateTime = inputDateTime.Add(timeWindow);
                }
                else
                    return;
            }
            else
                return;

            dr.Build(startDateTime, endDateTime);

            status.Text = string.Format("Entries : {0}", dr.Entries.Count);

            map.RangeModel = dr;

            return;

        }



    }
}

