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
    /// Interaction logic for HeatMapControl.xaml
    /// </summary>
    public partial class HeatMapControl : UserControl
    {

        /// <summary>
        /// Blue to Red color spectrum which is initialised 
        /// </summary>
        private static RGB[] spectrum = new RGB[100];


        private int stride;
        private DepthRange heatData;

        static HeatMapControl()
        {
            CreateSpectrum();
        }

        public HeatMapControl()
        {
            InitializeComponent();
        }

        public DepthRange RangeModel
        {
            get { return heatData; }
            set
            {
                heatData = value;
                Rebuild();
            }
        }

        private void Rebuild()
        {
            double bidRange = heatData.HighestBid - heatData.LowestBid;
            double askRange = heatData.HighestAsk - heatData.LowestAsk;

            WriteableBitmap wb = new WriteableBitmap((int)ActualWidth, (int)ActualHeight, 96, 96, PixelFormats.Rgb24, null);

            // Define the update square (which is as big as the entire image).
            RGB[] pixels = new RGB[(int)ActualWidth * (int)ActualHeight];


            int zoomx = 10;
            int zoomy = (int)(ActualHeight / 20);

            //double entriesPerPixel = dr.Entries.Count / imgHost.ActualWidth;
            double x = 0;
            for (int ei = 0; ei < heatData.Entries.Count && x < ActualWidth; ei++)
            {
                DepthEntry entry = heatData.Entries[ei];

                double y = 0;
                for (int idx = entry.Bids.Length-1; idx > 0; idx--)
                {
                    double bidVal = entry.Bids[idx];
                    double intensity = bidVal / (heatData.LowestBid + bidRange);
                    RGB c = GetColor(intensity);

                    //
                    for (int ty = 0; ty < zoomy; ty++)
                    {
                        int pixelOffset = (int)(x + y * (double)(wb.PixelWidth));
                        pixels[pixelOffset] = c;
                        y++;
                    }

                }

                for (int idx = entry.Asks.Length-1; idx > 0; idx--)
                {
                    double askVal = entry.Asks[idx];
                    double intensity = askVal / (heatData.LowestAsk + askRange);
                    RGB c = GetColor(intensity);

                    //
                    for (int ty = 0; ty < zoomy; ty++)
                    {
                        int pixelOffset = (int)(x + y * (double)(wb.PixelWidth));
                        pixels[pixelOffset] = c;
                        y++;
                    }

                }

                x++;
            }



            // Copy the byte array into the image in one step.
            stride = (wb.PixelWidth * wb.Format.BitsPerPixel) / 8;
            Int32Rect rect = new Int32Rect(0, 0, (int)ActualWidth, (int)ActualHeight);
            wb.WritePixels(rect, pixels, stride, 0);
            img.Source = wb;
        }

        private RGB GetColor(double percentage)
        {
            int index = (int)(percentage * 100) - 1;
            if (index < 0)
                index = 0;
            else if (index >= spectrum.Length)
                index = spectrum.Length;
            return spectrum[index];
        }


        private static Color HSBtoRGB(double h, double s, double b)
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

        /// <summary>
        /// Create spectrum from blue to red
        /// </summary>
        private static void CreateSpectrum()
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


        /// <summary>
        /// Individually addressable parts of the Red-Green-Blue Color structure used by the image
        /// </summary>
        struct RGB
        {
            public byte Red;
            public byte Green;
            public byte Blue;
        }
    }
}
