using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obotudavanie.Classes
{
    class PowerTransformator : Oborudovanie
    {
        public Attribute<double> Power { get; set; }
        public Attribute<double> Voltage { get; set; }
        public Attribute<string> FactoryNumber { get; set; }
        public PowerTransformator()
        {
            Power = new Attribute<double> { DefValue = 0, EdIzm = "кВ", Name = "Мощность", Type = "Вещественное", Value = 0 };
            Voltage = new Attribute<double> { DefValue = 0, EdIzm = "кВА", Name = "Напряжение", Type = "Вещественное", Value = 0 };
            FactoryNumber = new Attribute<string> { DefValue = "", EdIzm = "КГ/ч", Name = "Заводской номер", Type = "Текст", Value = "" };
            ShifrByCalssificator_OsnovnSredstva.Value = 40701;
        }
    }
}
