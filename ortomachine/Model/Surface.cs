using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ortomachine.ViewModel;

namespace ortomachine.Model
{
    public class Surface
    {
        private ScanPoints sc;
        public Bitmap image;

        public Surface(string filename)
        {
            sc = new ScanPoints(filename);
            sc.Surface(0.01f, 0.5);
            image =  sc.image();
             
            ;
            //sc.Surface();
        }
    }
}
