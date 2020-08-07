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
        public Attribute<double> Power { get; set; }
        public Attribute<string> FactoryNumber { get; set; }

        public SNK_ControlStation()
        {
            Type = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Тип", Type = "Текст", Value = "" };
            Power = new Attribute<double> { DefValue = 0, EdIzm = "кВА", Name = "Мощность", Type = "Вещественное", Value = 0 };
            FactoryNumber = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Заводской номер", Type = "Текст", Value = "" };
            Name_OsnovnSredstva.Value = "Станция управления СКН";
        }
    }
}
