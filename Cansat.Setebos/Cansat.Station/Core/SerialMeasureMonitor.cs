using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Cansat.Station.Core
{
    class SerialMeasureMonitor: IMeasureMonitor
    {
        
        public string SerialPortName { get; set; }

        public bool Listening
        {
            get
            {
                return port == null ? false : port.IsOpen;
            }

           
        }

        public event EventHandler<MeasureEventArgs> MeasureReceived;

        SerialPort port;

       

        public SerialMeasureMonitor(string portName)
        {
            SerialPortName = portName;
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                string line = port.ReadLine();
                Trace.WriteLine(string.Format("[{0}] {1}", DateTime.Now, line));
                var measure = Measure.ParseMeasure(line);
                if (measure != null)
                    OnMeasureReceived(measure);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //MessageBox.Show(ex.StackTrace);

            }
        }

        public void StartListening()
        {
            try
            {
                port = new SerialPort(SerialPortName, 9600);
                port.DataReceived += Port_DataReceived;
                port.Open();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //MessageBox.Show(ex.StackTrace);

            }

        }

        public void EndListening()
        {
            try
            {
                if (port != null)
                    port.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);

            }
        }

        protected virtual void OnMeasureReceived(Measure measure)
        {
            if (MeasureReceived != null)
            {
                var e = new MeasureEventArgs(measure);
                MeasureReceived(this, e);
            }
        }

    }
}
