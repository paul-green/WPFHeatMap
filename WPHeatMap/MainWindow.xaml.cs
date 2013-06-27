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
        private RGB[] spectrum = new RGB[100];
        private WriteableBitmap wb;
        private int stride;

        struct RGB
        {
            public byte Red;
            public byte Green;
            public byte Blue;
        }

        public MainWindow()
        {
            InitializeComponent();
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CreateSpectrum();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DepthRange dr = new DepthRange();
            dr.Build(DateTime.Parse("2013-06-07 00:00:11"), DateTime.Parse("2013-06-07 00:02:57"), (int)img.Width);

            wb = new WriteableBitmap((int)img.Width, (int)img.Height, 96, 96, PixelFormats.Rgb24, null);
            // Define the update square (which is as big as the entire image).
            Int32Rect rect = new Int32Rect(0, 0, (int)img.Width, (int)img.Height);
            RGB[] pixels = new RGB[(int)img.Width * (int)img.Height];
            for (int x = 0; x < img.Width; x++)
            {
                //DepthEntry entry = dr.Entries[x];
                double p = (double)x / img.Width;
                for (int y = 0; y < img.Height; y++)
                {
                    //int y = rr * 10;
                    int pixelOffset = (x + y * wb.PixelWidth);

                    pixels[pixelOffset] = GetColor(p);
                }

            }

            // Copy the byte array into the image in one step.
            stride = (wb.PixelWidth * wb.Format.BitsPerPixel) / 8;
            wb.WritePixels(rect, pixels, stride, 0);
            img.Source = wb;
        }

        

        /// <summary>
        /// Create spectrum from blue to red
        /// </summary>
        private void CreateSpectrum()
        {
            for (int i = 0; i < spectrum.Length; i++)
            {
                //240 -> 0 = Blue, Green, Yellow, Orange, Red
                double percentage = (double)i / (double)spectrum.Length;
                Color c = HSBtoRGB((240 * (1 - percentage)), 1, 1);
                spectrum[i].Red = c.R;
                spectrum[i].Green = c.G;
                spectrum[i].Blue = c.B;
            }
        }


        private RGB GetColor(double percentage)
        {
            return spectrum[(int)(percentage * 100)];
        }

        public static Color HSBtoRGB(double h, double s, double b)
        {
            double red = 0;
            double green = 0;
            double blue = 0;

            if (s == 0)
            {
                red = green = blue = b;
            }
            else
            {
                // the color wheel consists of 6 sectors. Figure out which sector
                // you're in.
                double sectorPos = h / 60.0;
                int sectorNumber = (int)(Math.Floor(sectorPos));
                // get the fractional part of the sector
                double fractionalSector = sectorPos - sectorNumber;

                // calculate values for the three axes of the color.
                double p = b * (1.0 - s);
                double q = b * (1.0 - (s * fractionalSector));
                double t = b * (1.0 - (s * (1 - fractionalSector)));

                // assign the fractional colors to r, g, and b based on the sector
                // the angle is in.
                switch (sectorNumber)
                {
                    case 0: red = b; green = t; blue = p; break;
                    case 1: red = q; green = b; blue = p; break;
                    case 2: red = p; green = b; blue = t; break;
                    case 3: red = p; green = q; blue = b; break;
                    case 4: red = t; green = p; blue = b; break;
                    case 5: red = b; green = p; blue = q; break;
                }
            }

            return Color.FromRgb(
                Convert.ToByte(Double.Parse(String.Format("{0:0.00}", red * 255.0))),
                Convert.ToByte(Double.Parse(String.Format("{0:0.00}", green * 255.0))),
                Convert.ToByte(Double.Parse(String.Format("{0:0.00}", blue * 255.0)))
            );
        }


    }
}

