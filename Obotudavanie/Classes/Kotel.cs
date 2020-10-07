using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obotudavanie.Classes
{
    [BsonIgnoreExtraElements]
    class Kotel : Oborudovanie
    {
        public Attribute<string> Type { get; set; }
        public Attribute<double> Power { get; set; }
        public Attribute<double> Productivity { get; set; }
        public Attribute<string> FactoryNumber { get; set; }
        public Attribute<DateTime> NextCheckDate { get; set; }
        public Kotel()
        {
            Type = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Тип", Type = "Текст", Value = "" };
            Power = new Attribute<double> { DefValue = 0, EdIzm = "кВ (6, 10)", Name = "Мощность", Type = "Вещественное", Value = 0 };
            Productivity = new Attribute<double> { DefValue = 0, EdIzm = "...", Name = "Производительность", Type = "Вещественное", Value = 0 };
            FactoryNumber = new Attribute<string> { DefValue = "", EdIzm = "КГ/ч", Name = "Заводской номер", Type = "Текст", Value = "" };
            NextCheckDate = new Attribute<DateTime> { DefValue = DateTime.Parse("01/01/2001"), EdIzm = "КГ/ч", Name = "дата следующего осмотра", Type = "Текст", Value = DateTime.Parse("01/01/2031") };
            Name_OsnovnSredstva.Value = "Котел";
        }

    }
}
