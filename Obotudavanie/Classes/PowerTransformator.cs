using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obotudavanie.Classes
{
    class PowerTransformator : Oborudovanie
    {
        public Attribute<float> Power { get; set; }
        public Attribute<float> Voltage { get; set; }
        public Attribute<string> FactoryNumber { get; set; }

    }
}
