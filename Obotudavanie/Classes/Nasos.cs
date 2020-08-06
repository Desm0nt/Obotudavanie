using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obotudavanie.Classes
{
    class Nasos: Oborudovanie
    {
        public Attribute<string> Type { get; set; }
        public Attribute<string> Purpose { get; set; }
        public Attribute<float> EnginePower { get; set; }
        public Attribute<float> Productivity { get; set; }
        public Attribute<string> Ispolnenie { get; set; }
        public Attribute<float> Napor { get; set; }

    }
}
