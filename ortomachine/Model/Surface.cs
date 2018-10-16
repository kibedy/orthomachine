using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ortomachine.Model
{
    public class Surface
    {        
        public Surface(string filename)
        {
            ScanPoints sc = new ScanPoints(filename);
            sc.Surface(0.01f, 0.5);
            sc.image();
            ;
            //sc.Surface();
        }
    }
}
