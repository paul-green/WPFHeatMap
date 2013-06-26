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

namespace WPHeatMap
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        struct ARGB
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }

        public MainWindow()
        {
            InitializeComponent();
        }


        WriteableBitmap wb;
        byte[] pixels;
        int stride;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Create the bitmap, with the dimensions of the image placeholder.
            wb = new WriteableBitmap((int)img.Width, (int)img.Height, 96, 96, PixelFormats.Bgra32, null);
            // Define the update square (which is as big as the entire image).
            Int32Rect rect = new Int32Rect(0, 0, (int)img.Width, (int)img.Height);
            pixels = new byte[(int)img.Width * (int)img.Height * wb.Format.BitsPerPixel / 8];
            Random rand = new Random();
            for (int y = 0; y < wb.PixelHeight; y++)
            {
                for (int x = 0; x < wb.PixelWidth; x++)
                {
                    int alpha = 0;
                    int red = 0;
                    int green = 0;
                    int blue = 0;
                    // Determine the pixel's color.

                    /*
                    if ((x % 5 == 0) || (y % 7 == 0))
                    {
                        red = (int)((double)y / wb.PixelHeight * 255);
                        green = rand.Next(100, 255);
                        blue = (int)((double)x / wb.PixelWidth * 255);
                        alpha = 255;
                    }
                    else
                     * */
                    {
                        red = (int)((double)x / wb.PixelWidth * 255);
                        green = rand.Next(100, 255);
                        blue = (int)((double)y / wb.PixelHeight * 255);
                        alpha = 50;
                    }
                    int pixelOffset = (x + y * wb.PixelWidth) * wb.Format.BitsPerPixel / 8;
                    pixels[pixelOffset] = (byte)blue;
                    pixels[pixelOffset + 1] = (byte)green;
                    pixels[pixelOffset + 2] = (byte)red;

                    pixels[pixelOffset + 3] = (byte)alpha;
                }
                // Copy the byte array into the image in one step.
                stride = (wb.PixelWidth * wb.Format.BitsPerPixel) / 8;
                wb.WritePixels(rect, pixels, stride, 0);
            }
            // Show the bitmap in an Image element.
            img.Source = wb;
          

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DepthRange dr = new DepthRange();
            dr.Build(DateTime.Parse("2013-06-07 00:00:11"), DateTime.Parse("2013-06-07 00:02:57"), (int)img.Width);

            wb = new WriteableBitmap((int)img.Width, (int)img.Height, 96, 96, PixelFormats.Bgra32, null);
            // Define the update square (which is as big as the entire image).
            Int32Rect rect = new Int32Rect(0, 0, (int)img.Width, (int)img.Height);
            ARGB[] pixels = new ARGB[(int)img.Width * (int)img.Height];
            for (int x = 0; x < dr.Entries.Count; x++)
            {
                DepthEntry entry = dr.Entries[x];

                for (int rr = 0; rr < entry.Bids.Length; rr++)
                {

                    int y = 10d / img.Height;
                    int pixelOffset = (x + y * wb.PixelWidth);
                    pixels[pixelOffset].Red = 255;
                    pixels[pixelOffset].Alpha = 255;
                }

            }

            // Copy the byte array into the image in one step.
            stride = (wb.PixelWidth * wb.Format.BitsPerPixel) / 8;
            wb.WritePixels(rect, pixels, stride, 0);
            img.Source = wb;
        }


    }
}
