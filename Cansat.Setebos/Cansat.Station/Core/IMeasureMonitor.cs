using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cansat.Station.Core
{

    interface IMeasureMonitor
    {
        bool Listening { get; }
        event EventHandler<MeasureEventArgs> MeasureReceived;
        void StartListening();
        void EndListening();
    }
}
