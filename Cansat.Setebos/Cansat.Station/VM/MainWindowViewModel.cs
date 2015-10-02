using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrfrankLibrary.Core;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Cansat.Station.Core;
using Cansat.Station.Utility;
using Cansat.Setebos.Data;
using System.Diagnostics;

namespace Cansat.Station.VM
{
    public class MainWindowViewModel : NotifyPropetryAdapter, IDisposable
    {
        private string measureLogText;
        Core.IMeasureMonitor monitor;
        public string MeasureLogText
        {
            get { return measureLogText; }
            set
            {
                measureLogText = value;
                this.OnPropertyChanged();
            }
        }        
        Flights selectedFlight;
        CansatEntities db = new CansatEntities();
        private ICommand startListeningCommand;
        private ICommand stopListeningCommand;

        public ICommand StartListeningCommand
        {
            get { return startListeningCommand == null ? startListeningCommand = new ActionCommand(StartListening) : startListeningCommand; }
        }

        public ICommand StopListeningCommand
        {
            get { return stopListeningCommand == null ? stopListeningCommand = new ActionCommand(StopListening) : stopListeningCommand; }

        }

        private ICommand refreshSerialPortCommand;

        public ICommand RefreshSerialPortCommand {
            get { return refreshSerialPortCommand == null ? refreshSerialPortCommand = new ActionCommand(RefreshSerialPorts) : refreshSerialPortCommand; }
        }

        public MTObservableCollection<Measure> MeasureData { get; set; }

        public ObservableCollection<Flights> ActiveFlights { get; set; }

        ObservableCollection<string> serialPorts;
        public ObservableCollection<String> SerialPorts {
            get { return serialPorts; }
            set { serialPorts = value; OnPropertyChanged(); }
        }

        private string selectedSerialPort;
        public string SelectedSerialPort { get {
                return selectedSerialPort;
            } set{
                if(value !=null){
                    selectedSerialPort = value;
                    OnPropertyChanged();
                }
            } }

        public Flights SelectedFlight
        {
            get { return selectedFlight; }
            set { if (value != null) { selectedFlight = value; OnPropertyChanged(); } Trace.WriteLine(value); }
        }

        public bool EnableStop
        {
            get { return monitor.Listening; }
        }

        public bool EnableStart
        {
            get { return !monitor.Listening; }
        }

        public MainWindowViewModel()
        {
            MeasureLogText = "";
            monitor = new Core.RandomMeasureMonitor("Dev1");
            monitor.MeasureReceived += Monitor_MeasureReceived;
            MeasureData = new MTObservableCollection<Measure>();
            ActiveFlights = new MTObservableCollection<Flights>();

            LoadActiveFlights();
        }

        void LoadActiveFlights()
        {
            var activeflights = db.Flights.Where(f => f.Active).ToList();
            //ActiveFlights.Clear();
            foreach (var f in activeflights)
            {
                ActiveFlights.Add(f);
            }
            if (activeflights.Count > 0) {
                SelectedFlight = activeflights[0];
            }

        }

        void RefreshSerialPorts() {
            serialPorts = new ObservableCollection<string>(System.IO.Ports.SerialPort.GetPortNames());
        }

        public void StartListening()
        {
            monitor.StartListening();
            OnPropertyChanged("EnableStart");
            OnPropertyChanged("EnableStop");
        }
        public void StopListening()
        {
            monitor.EndListening();
            OnPropertyChanged("EnableStart");
            OnPropertyChanged("EnableStop");
        }
        void SaveMeasure(Flights fligh, Measure measure)
        {
            Data d = new Data();
            d.Altitude = measure.Altitude;
            d.CO = measure.PM10;
            d.Datetime = measure.MeasureDate;
            d.FlightId = fligh.FlightId;
            d.Humidity = measure.Humidity;
            d.InternalTemperature = measure.InternalTemperature;
            d.Latitude = measure.Latitude;
            d.Longitude = measure.Longitude;
            d.Presure = measure.Preasure;
            d.Temperature = measure.ExternalTemperature;
            d.Voltage = measure.BatteryVoltage;
            d.Ejected = measure.Ejected;
            d.Speed = measure.Speed;
            d.BarometricAltitude = measure.BarometricAltitude;

            db.Data.Add(d);
            db.SaveChanges();

        }
        protected void Monitor_MeasureReceived(object sender, Core.MeasureEventArgs e)
        {
            MeasureLogText += e.Measure.ToString() + "\n";
            MeasureData.Add(e.Measure);
            SaveMeasure(SelectedFlight, e.Measure);
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
