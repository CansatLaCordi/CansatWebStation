using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrfrankLibrary.Core;
using System.Windows.Input;

namespace Cansat.Station.VM
{
    public class MainWindowViewModel : NotifyPropetryAdapter
    {
        private string measureLogText;
        Core.IMeasureMonitor monitor;
        public string MeasureLogText
        {
            get { return measureLogText; }
            set {
                measureLogText = value;
                this.OnPropertyChanged();
            }
        }
        private ICommand startListeningCommand;

        public ICommand StartListeningCommand
        {
            get { return startListeningCommand == null? startListeningCommand = new ActionCommand(StartListening): startListeningCommand; }
           
        }


        private ICommand stopListeningCommand;

        public ICommand StopListeningCommand
        {
            get { return stopListeningCommand == null ? stopListeningCommand = new ActionCommand(StopListening) : stopListeningCommand; }

        }

        public MainWindowViewModel() {
            MeasureLogText = "";
            monitor = new Core.RandomMeasureMonitor("Dev1");
            monitor.MeasureReceived += Monitor_MeasureReceived;
        }

        public void StartListening() {
            monitor.StartListening();
        }
        public void StopListening() {
            monitor.EndListening();
        }

        private void Monitor_MeasureReceived(object sender, Core.MeasureEventArgs e)
        {
            MeasureLogText += e.Measure.ToString() + "\n";
        }
    }
}
