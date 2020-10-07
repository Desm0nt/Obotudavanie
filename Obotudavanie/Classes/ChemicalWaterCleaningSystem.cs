using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obotudavanie.Classes
{
    [BsonIgnoreExtraElements]
    class ChemicalWaterCleaningSystem : Oborudovanie
    {
        public Attribute<string> Type { get; set; }
        public Attribute<string> Purpose { get; set; }
        public Attribute<string> Ispolnenie { get; set; }
        public Attribute<int> NumOfColumn { get; set; }
        public Attribute<float> Manufacturer { get; set; }
    }
}
