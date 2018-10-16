using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ortomachine.Model
{
    class ScanPoints
    {
        LinkedList<Points> PointList =  new LinkedList<Points>();
        string path="";
        UInt16[,] surface;
        ushort xwidth;
        ushort yheight;




        public ScanPoints()
        {
            StreamReader sr = new StreamReader(path);
            char[] delimiterChars = { ' ', ',', '\t' };
            //int filetype = 0;
            string line = sr.ReadLine();
            string[] numbers = line.Split(delimiterChars);
            int filetype = numbers.Length;

            if (filetype == 3)
            {
                Points point = new Points(double.Parse(numbers[0]), double.Parse(numbers[1]), double.Parse(numbers[2]));
                PointList.AddFirst(point);

                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    numbers = line.Split(delimiterChars);
                    {
                        point = new Points(double.Parse(numbers[0]), double.Parse(numbers[1]), double.Parse(numbers[2]));
                        PointList.AddLast(point);
                    }
                }
            }

                       
        }


        public void Surface(int x0, int y0, int xmax, int ymax, uint rastersize)
        {
            Points actual = PointList.First();
            xwidth = (ushort)((xmax - x0) / rastersize);
            yheight = (ushort)((ymax - y0) / rastersize);
            surface = new UInt16[xwidth, yheight];
            double minZ = -99999999;

            foreach (Points item in PointList)
            {
                uint i = (uint)(actual.X - x0) / rastersize;
                uint j = (uint)(actual.Y - y0) / rastersize;
                surface[i, j] = (ushort)(actual.Z * 1000);    //computing in mm
                //if (minZ > actual.Z)
                //    minZ = actual.Z;
            }
           
            
        }


        public void image()
        {
            // Bitmap c = new Bitmap("fromFile");
            Bitmap d = new Bitmap("surface.png");
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
        }

    }
}
