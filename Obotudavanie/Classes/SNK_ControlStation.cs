using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obotudavanie.Classes
{
    class SNK_ControlStation :Oborudovanie
    {
        public Attribute<string> Type { get; set; }
        public Attribute<float> Power { get; set; }
        public Attribute<string> FactoryNumber { get; set; }
    }
}
