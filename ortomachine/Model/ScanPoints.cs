using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
//using Emgu.CV;
//using Emgu.Util;
//using Emgu.CV.Structure;

namespace ortomachine.Model
{
    public class ScanPoints
    {
        LinkedList<Points> PointList =  new LinkedList<Points>();
        string path="";
        UInt16[,] surface;
        ushort xwidth;
        ushort yheight;
        uint offset;    //offset: black border
        float rastersize;
        double Xmax, Zmax,  X0,  Z0;


        public ScanPoints(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            char[] delimiterChars = { ' ', ',', '\t' };
            //int filetype = 0;
            string line = sr.ReadLine();
            string[] numbers = line.Split(delimiterChars);
            int filetype = numbers.Length;

            if (filetype == 3)
            {
                double X = double.Parse(numbers[0],System.Globalization.CultureInfo.InvariantCulture);
                double Y = double.Parse(numbers[1], System.Globalization.CultureInfo.InvariantCulture);
                double Z = double.Parse(numbers[2], System.Globalization.CultureInfo.InvariantCulture);
                //Points point = new Points(double.Parse(numbers[0]), double.Parse(numbers[1]), double.Parse(numbers[2]));
                //Points point = new Points(654817.204,237975.415,111.783);
                Points point = new Points(X,Y,Z);
                PointList.AddFirst(point);

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    numbers= line.Split(delimiterChars);
                    
                    {
                        point = new Points(
                            double.Parse(numbers[0], System.Globalization.CultureInfo.InvariantCulture), 
                            double.Parse(numbers[1], System.Globalization.CultureInfo.InvariantCulture), 
                            double.Parse(numbers[2], System.Globalization.CultureInfo.InvariantCulture));
                        PointList.AddLast(point);
                    }
                }
            }
            if (filetype == 4)      //intensity only
            {
                Points point = new Points(
                    double.Parse(numbers[0], System.Globalization.CultureInfo.InvariantCulture), 
                    double.Parse(numbers[1], System.Globalization.CultureInfo.InvariantCulture), 
                    double.Parse(numbers[2], System.Globalization.CultureInfo.InvariantCulture), 
                    int.Parse(numbers[3]));
                PointList.AddFirst(point);

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    numbers = line.Split(delimiterChars);
                    {
                        point = new Points(
                            double.Parse(numbers[0], System.Globalization.CultureInfo.InvariantCulture), 
                            double.Parse(numbers[1], System.Globalization.CultureInfo.InvariantCulture), 
                            double.Parse(numbers[2], System.Globalization.CultureInfo.InvariantCulture), 
                            int.Parse(numbers[3]));
                        PointList.AddLast(point);
                    }
                }
            }

            if (filetype == 7)
            {
                Points point = new Points(
                        double.Parse(numbers[0], System.Globalization.CultureInfo.InvariantCulture), 
                        double.Parse(numbers[1], System.Globalization.CultureInfo.InvariantCulture), 
                        double.Parse(numbers[2], System.Globalization.CultureInfo.InvariantCulture), 
                        int.Parse(numbers[3]), 
                        int.Parse(numbers[4]), 
                        int.Parse(numbers[5]), 
                        int.Parse(numbers[6]));
                        
                
                PointList.AddFirst(point);

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    numbers = line.Split(delimiterChars);
                    {
                        point = new Points(
                            double.Parse(numbers[0], System.Globalization.CultureInfo.InvariantCulture), 
                            double.Parse(numbers[1], System.Globalization.CultureInfo.InvariantCulture), 
                            double.Parse(numbers[2], System.Globalization.CultureInfo.InvariantCulture), 
                            int.Parse(numbers[3]), 
                            int.Parse(numbers[4]), 
                            int.Parse(numbers[5]), 
                            int.Parse(numbers[6]));
                        PointList.AddLast(point);
                    }
                }
            }
            BoundingBox();
            ;

        }


        public void Surface(float rastersize, double offset)
        {
            //Points actual = PointList.First();
            xwidth = (ushort)((((Xmax - X0) + 2 * offset) / rastersize)+1);
            yheight = (ushort)((((Zmax - Z0) + 2 * offset) / rastersize)+1);
            
            surface = new UInt16[xwidth, yheight];
            double minY = -99999999;

            foreach (Points item in PointList)
            {
                uint i = (uint)(((item.X - X0) + offset) / rastersize);
                uint j = (uint)(((item.Z - Z0) + offset) / rastersize);
                if (item.Y>minY || surface[i,j]<item.Y)
                {
                    surface[i, j] = (ushort)(item.Z * 1000);    //computing in mm                
                }
                
            }
            ;
            
        }

        public void BoundingBox()
        {
             Xmax = 0;  Zmax = 0;  X0 = 999999999;  Z0 = 999999999; //global coordinates
            foreach (Points item in PointList)
            {
                if (item.X<X0)
                {
                    X0 = item.X;
                }
                if (item.X > Xmax)
                {
                    Xmax = item.X;
                }
                if (item.Z < Z0)
                {
                    Z0 = item.Z;
                }
                if (item.Z > Zmax)
                {
                    Zmax = item.Z;
                }
            }


        }

        public Bitmap image()
        {
            Bitmap image = new Bitmap(xwidth, yheight, System.Drawing.Imaging.PixelFormat.Format16bppGrayScale);

            var rect = new Rectangle(0, 0, xwidth, yheight);
            BitmapData bitmapData = image.LockBits(rect, ImageLockMode.WriteOnly, image.PixelFormat);

            //ushort a = 0;
            unsafe
            {
                ushort* imagePointer = (ushort*)bitmapData.Scan0;
                for (int y = 0; y < bitmapData.Height; y++)
                {
                    for (int x = 0; x < bitmapData.Width; x++)
                    {                        
                        imagePointer[0] = surface[x, y];
                        imagePointer++;
                    }
                    imagePointer += bitmapData.Stride - bitmapData.Width*2;
                }


            }
            image.UnlockBits(bitmapData);
            //image.Save("surface.bmp", ImageFormat.Bmp);

            BitmapSource source = BitmapSource.Create(image.Width,
                                                  image.Height,
                                                  image.HorizontalResolution,
                                                  image.VerticalResolution,
                                                  PixelFormats.Gray16,
                                                  null,
                                                  bitmapData.Scan0,
                                                  bitmapData.Stride * image.Height,
                                                  bitmapData.Stride);

            FileStream stream = new FileStream("surface.bmp", FileMode.Create);
            TiffBitmapEncoder encoder = new TiffBitmapEncoder();
            encoder.Compression = TiffCompressOption.Zip;
            encoder.Frames.Add(BitmapFrame.Create(source));
            encoder.Save(stream);

            Debug.WriteLine("saved");

            return image;
            /*
            var numberOfBytes = bitmapData.Stride * yheight;
            var bitmapBytes = new int[xwidth * yheight];
            

            for (int x = 0; x < xwidth; x++)
            {
                for (int y = 0; y < yheight; y++)
                {
                    var i = ((y * xwidth) + x);
                    bitmapBytes[i] = surface[x, y];

                    //Color pixelColor = surface[x, y];
                    //Color newColor = Color.FromArgb()
                    //image.SetPixel(x, y,co); // Now greyscale
                }
            }

            var ptr = bitmapData.Scan0;
            Marshal.Copy(, 0, ptr, numberOfBytes);
            image.UnlockBits(bitmapData);

            image.Save("surface.png",ImageFormat.Png);
            Debug.WriteLine("saved");

            return image;
            
            */
        }
            

        /*

            // Bitmap c = new Bitmap("fromFile");
            Bitmap d = new Bitmap("surface.png");
            d.Size = new Size(xwidth, yheight);
            int x, y;

            // Loop through the images pixels to reset color.
            for (x = 0; x < xwidth; x++)
            {
                for (y = 0; y < yheight; y++)
                {
                    //Color pixelColor = surface[x, y];
                    Color newColor = Color.FromArgb(surface[x, y]);
                    d.SetPixel(x, y, newColor); // Now greyscale
                }
            }
            //d = c;   // d is grayscale version of c  
            d.Save("surface.png", ImageFormat.Png);
        }*/

    }
}
