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

        public MainWindow()
        {
            InitializeComponent();
            SetMinAndMax();
        }

        private void SetMinAndMax()
        {
            int lines = 0;
            using (StreamReader r = File.OpenText("Data\\EURUSD_20130607_EBS_secondly.csv"))
            {
                while (!r.EndOfStream)
                {
                    lines++;
                    r.ReadLine();
                }
            }

            slider_start.Maximum = lines;
            slider_end.Maximum = lines;

        }


        private void Plot()
        {
            DepthRange d = new DepthRange();
            d.Build(1, 5000);




        }

        WriteableBitmap writeableBitmap;
        WriteableBitmap wb;
        byte[] pixels;
        int stride;
        private void Poo()
        {
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Create the bitmap, with the dimensions of the image placeholder.
            wb = new WriteableBitmap((int)img.Width,
           (int)img.Height, 96, 96, PixelFormats.Bgra32, null);
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
            int pixelOffset = 0;// (1 + 1 * wb.PixelWidth) * wb.Format.BitsPerPixel / 8;
             pixels = new byte[1 * 1 * wb.Format.BitsPerPixel / 8];
            pixels[pixelOffset] = (byte)128;
            pixels[pixelOffset + 1] = (byte)50;
            pixels[pixelOffset + 2] = (byte)50;

            pixels[pixelOffset + 3] = (byte)255;
            Int32Rect r = new Int32Rect(100, 100, 1, 1);
            wb.WritePixels(r, pixels, stride, 0);
        }


    }
}
