using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cansat.Station.Core
{
    class MeasureEventArgs: EventArgs
    {
        public Measure Measure { get; set; }

        public MeasureEventArgs() {

        }

        public MeasureEventArgs(Measure measure) {
            Measure = measure;
        }
    }
}
