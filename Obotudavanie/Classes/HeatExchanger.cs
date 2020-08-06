using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obotudavanie.Classes
{
    class HeatExchanger : Oborudovanie
    {
        public Attribute<string> Type { get; set; }
        public Attribute<string> Purpose { get; set; }
        public Attribute<string> Ispolnenie { get; set; }
        public Attribute<float> WorkPressure { get; set; }
        public Attribute<int> PlateCount { get; set; }
        public Attribute<float> HeatPower { get; set; }
        public Attribute<float> HeatExchangeSquare { get; set; }
    }
}
