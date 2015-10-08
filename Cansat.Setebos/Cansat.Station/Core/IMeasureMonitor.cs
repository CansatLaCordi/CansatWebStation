using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cansat.Station.Core
{

    public interface IMeasureMonitor
    {
        bool Listening { get; }

        string SerialPortName { get; set; }

        event EventHandler<MeasureEventArgs> MeasureReceived;
        void StartListening();
        void EndListening();
    }
}
