using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ortomachine.ViewModel
{
    class ViemModelBase : INotifyPropertyChanged
    {
        public ViemModelBase()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        internal void OnPropertyChanged(string property)
        {
            if (PropertyChanged!=null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        bool? closedWindowFlag;

        public bool? ClosedWindowFlag
        {
            get { return closedWindowFlag; }
            set {
                closedWindowFlag = value;
                OnPropertyChanged("ClosedWindowFlag");
            }
        }

        public virtual void ClosedWindow(bool? result = true)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            { 
                ClosedWindowFlag = ClosedWindowFlag == null ? true : !ClosedWindowFlag;
            }));
        }
    }
}
