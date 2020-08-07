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
        public Attribute<double> WorkPressure { get; set; }
        public Attribute<int> PlateCount { get; set; }
        public Attribute<double> HeatPower { get; set; }
        public Attribute<double> HeatExchangeSquare { get; set; }

        public HeatExchanger()
        {
            Type = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Тип", Type = "Текст", Value = "" };
            Purpose = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Назначение", Type = "Текст", Value = "" };
            Ispolnenie = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Исполнение", Type = "Текст", Value = "" };
            WorkPressure = new Attribute<double> { DefValue = 0, EdIzm = "МПа", Name = "Рабочее давление", Type = "Вещественное", Value = 0 };
            PlateCount = new Attribute<int> { DefValue = 0, EdIzm = "шт", Name = "Количество пластин", Type = "Целое", Value = 0 };
            HeatPower = new Attribute<double> { DefValue = 0, EdIzm = "кВт", Name = "Тепловая мощность", Type = "Вещественное", Value = 0 };
            HeatExchangeSquare = new Attribute<double> { DefValue = 0, EdIzm = "кв.м", Name = "Площадь поверхности теплообмена", Type = "Вещественное", Value = 0 };
            //ShifrByCalssificator_OsnovnSredstva.Value = 60000;
            Name_OsnovnSredstva.Value = "Теплообменник";
        }
    }
}
