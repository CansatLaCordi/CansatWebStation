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
using GMap.NET;
using GMap.NET.WindowsPresentation;
using System.Windows.Controls;
using System.Windows;
using GMap.NET.MapProviders;
using System.Windows.Media.Imaging;
using System.Threading;

namespace Cansat.Station.VM
{
    public class MainWindowViewModel : NotifyPropetryAdapter, IDisposable
    {
        private string measureLogText;
        Core.IMeasureMonitor monitor;
        private string selectedSerialPort;
        Flights selectedFlight;
        CansatEntities db = new CansatEntities();
        private ICommand startListeningCommand;
        private ICommand stopListeningCommand;
        private ICommand saveFlightsCommand;
        private ICommand removeSelectedGridFlightCommand;
        private ICommand refreshSerialPortCommand;
        private ICommand clearConsoleCommand;
        private ICommand loadDbMeasuresCommand;

        public ICommand StartListeningCommand
        {
            get { return startListeningCommand == null ? startListeningCommand = new ActionCommand(StartListening) : startListeningCommand; }
        }
        public ICommand StopListeningCommand
        {
            get { return stopListeningCommand == null ? stopListeningCommand = new ActionCommand(StopListening) : stopListeningCommand; }

        }
        public ICommand RefreshSerialPortCommand
        {
            get { return refreshSerialPortCommand == null ? refreshSerialPortCommand = new ActionCommand(RefreshSerialPorts) : refreshSerialPortCommand; }
        }
        public ICommand SaveFlightsCommand
        {
            get { return saveFlightsCommand == null ? saveFlightsCommand = new ActionCommand(SaveFlights) : saveFlightsCommand; }
        }
        public ICommand RemoveSelectedGridFlightCommand
        {
            get
            {
                return removeSelectedGridFlightCommand == null ? removeSelectedGridFlightCommand = new ActionCommand(RemoveSelectedGridFlight) : removeSelectedGridFlightCommand;
            }
        }
        public ICommand CleanConsoleCommand
        {
            get { return clearConsoleCommand == null ? clearConsoleCommand = new ActionCommand(ClearConsole) : clearConsoleCommand; }
        }
        public ICommand LoadDBMeasuresCommand
        {
            get { return loadDbMeasuresCommand == null ? loadDbMeasuresCommand = new ActionCommand(LoadDbMeasures) : loadDbMeasuresCommand; }
        }
        public string MeasureLogText
        {
            get { return measureLogText; }
            set
            {
                measureLogText = value;
                this.OnPropertyChanged();
            }
        }
        public MTObservableCollection<Measure> MeasureData { get; set; }
        public ObservableCollection<Flights> ActiveFlights { get; set; }
        ObservableCollection<string> serialPorts;
        public ObservableCollection<String> SerialPorts
        {
            get { return serialPorts; }
            set { serialPorts = value; OnPropertyChanged(); }
        }
        public string SelectedSerialPort
        {
            get
            {
                return selectedSerialPort;
            }
            set
            {
                if (value != null)
                {
                    selectedSerialPort = value;
                    OnPropertyChanged();
                }
            }
        }
        public Flights SelectedFlight
        {
            get { return selectedFlight; }
            set
            {
                if (value != null)
                {
                    selectedFlight = value;
                    OnPropertyChanged();
                    //Debug.WriteLine(value);
                    LoadDbMeasures();
                }
            }
        }
        public Flights GridSelectedFlight { get; set; }
        ObservableCollection<Flights> flights;
        public ObservableCollection<Flights> Flights
        {
            get
            {
                if (flights == null) LoadFlights();
                return flights;
            }
            private set { flights = value; OnPropertyChanged(); }
        }
        public Measure LastMeasure {
            get { return MeasureData.LastOrDefault(); }
        }
        public MTObservableCollection<GMapMarker> Route { get; set; }

        TabItem actualtab;
        public TabItem ActualTab
        {
            get { return actualtab; }
            set
            {
                actualtab = value;
                OnPropertyChanged("MapTabVisibility");
                OnPropertyChanged("VuelosTabVisibility");
            }
        }

        public bool EnableStop
        {
            get { return monitor.Listening; }
        }

        public bool EnableStart
        {
            get { return !monitor.Listening; }
        }

        public Visibility MapTabVisibility
        {
            get
            {
                return ActualTab != null && ActualTab.Header.ToString() == "Mapa" ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public Visibility VuelosTabVisibility
        {
            get { return ActualTab != null && ActualTab.Header.ToString() == "Vuelos" ? Visibility.Visible : Visibility.Hidden; }
        }

        public List<GMapProvider> MapProviders { get; set; }


        public MainWindowViewModel()
        {
            MeasureLogText = "";
            monitor = new SerialMeasureMonitor(SelectedSerialPort);
            monitor.MeasureReceived += Monitor_MeasureReceived;
            MeasureData = new MTObservableCollection<Measure>();
            ActiveFlights = new MTObservableCollection<Flights>();
            Route = new MTObservableCollection<GMapMarker>();
            MapProviders = GMapProviders.List;
            LoadActiveFlights();
        }

        void LoadActiveFlights()
        {
            var activeflights = db.Flights.Where(f => f.Active).ToList();
            ActiveFlights.Clear();
            foreach (var f in activeflights)
            {
                ActiveFlights.Add(f);
            }
            if (activeflights.Count > 0)
            {
                SelectedFlight = activeflights[0];
            }

        }

        void RefreshSerialPorts()
        {
            var names = System.IO.Ports.SerialPort.GetPortNames();
            SerialPorts = new ObservableCollection<string>(names);
        }

        void RemoveSelectedGridFlight()
        {
            if (GridSelectedFlight != null)
            {
                db.Entry(GridSelectedFlight).State = System.Data.EntityState.Deleted;
                Flights.Remove(GridSelectedFlight);
                db.SaveChanges();
                LoadFlights();
            }


        }

        public void StartListening()
        {
            monitor.SerialPortName = SelectedSerialPort;
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
            try
            {
                Data d = new Data();
                d.Altitude = measure.Altitude;
                d.CO = measure.PM10;
                d.Datetime = measure.MeasureDate ?? DateTime.UtcNow;
                d.FlightId = fligh.FlightId;
                d.Humidity = measure.Humidity;
                d.InternalTemperature = measure.InternalTemperature;
                d.Latitude = measure.Latitude;
                d.Longitude = measure.Longitude;
                d.Presure = measure.Pressure;
                d.Temperature = measure.ExternalTemperature;
                d.Voltage = measure.Voltage;
                d.Ejected = measure.Ejected;
                d.Speed = measure.Speed;
                d.BarometricAltitude = measure.BarometricAltitude;

                db.Data.Add(d);
                db.SaveChanges();
            }
            catch (Exception ex)
            {

            }

        }

        protected void Monitor_MeasureReceived(object sender, Core.MeasureEventArgs e)
        {
            MeasureLogText += e.Measure.ToString() + "\n";
            MeasureData.Add(e.Measure);
            SaveMeasure(SelectedFlight, e.Measure);
            AddPointFromMeasure(e.Measure);
            OnPropertyChanged("LastMeasure");
        }

        void AddPointFromMeasure(Measure m, bool alertChange = true)
        {
            if (m.Latitude != null && m.Longitude != null)
            {
                var marker = new GMap.NET.WindowsPresentation.GMapMarker(new PointLatLng(m.Latitude.Value, m.Longitude.Value));
                marker.Offset = new Point(0, 0);
                marker.ZIndex = int.MaxValue;
                Route.Add(marker);
                //Route.Add(new GMap.NET.WindowsPresentation.GMapMarker(new GMap.NET.PointLatLng(20.5713651, -103.6362335)));
                if (alertChange)
                    OnPropertyChanged("Route");
            }
        }

        void LoadFlights()
        {
            var f = db.Flights.ToList();
            Flights = new ObservableCollection<Setebos.Data.Flights>(f);
        }

        void SaveFlights()
        {
            foreach (var item in Flights)
            {
                if (!db.Flights.Any(f => f.FlightId == item.FlightId))
                    db.Flights.Add(item);
                else
                    db.Entry(item).State = System.Data.EntityState.Modified;
            }
            db.SaveChanges();
            LoadActiveFlights();
        }

        void ClearConsole()
        {
            MeasureLogText = string.Empty;
        }

        void LoadDbMeasures()
        {
            DateTime thisInstant = DateTime.Now;
            bool islistening = monitor.Listening;
            monitor.EndListening();
            var dbMeasures = db.Data.Where(d => d.FlightId == SelectedFlight.FlightId && d.Datetime <= thisInstant)
                .Select(d => new Measure()
                {
                    Altitude = d.Altitude,
                    BarometricAltitude = d.BarometricAltitude,
                    Voltage = d.Voltage,
                    Ejected = d.Ejected,
                    ExternalTemperature = d.Temperature,
                    Humidity = d.Humidity,
                    InternalTemperature = d.InternalTemperature,
                    Latitude = d.Latitude,
                    Longitude = d.Longitude,
                    MeasureDate = d.Datetime.Value,
                    PM10 = d.PM10,
                    Pressure = d.Presure,
                    Speed = d.Speed
                })
                .OrderBy(m => m.MeasureDate);
            MeasureData.Clear();
            Route.Clear();
            foreach (var m in dbMeasures)
            {
                MeasureData.Add(m);
                AddPointFromMeasure(m);
            }
            if (islistening)
                monitor.StartListening();
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
