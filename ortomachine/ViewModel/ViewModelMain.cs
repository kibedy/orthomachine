using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using ortomachine.Helpers;
using System.IO;
using Microsoft.Win32;
using ortomachine.Model;

namespace ortomachine.ViewModel
{
    class ViewModelMain : ViemModelBase
    {

        public  RelayCommand OpenMenuCommand { get; set; }
        //public List<PCPoints> PointClod;
        public string filename;

        public ViewModelMain()
        {
            OpenMenuCommand = new RelayCommand(OpenFile);
        }



        public void OpenFile(object parameter)
        {           
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Point cloud (*.txt)|*.txt|ASCII file (*.asc)|*.asc";
            ofd.DefaultExt = ".asc";

            Nullable<bool> result = ofd.ShowDialog();
            if (result == true)
            {                
                filename = ofd.FileName;
            }
            Surface sf = new Surface(filename);

        }

    }
}
