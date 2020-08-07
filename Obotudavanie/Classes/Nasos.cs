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
        public Attribute<double> EnginePower { get; set; }
        public Attribute<double> Productivity { get; set; }
        public Attribute<string> Ispolnenie { get; set; }
        public Attribute<double> Napor { get; set; }

        public Nasos()
        {
            Type = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Тип", Type = "Текст", Value = "" };
            Purpose = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Назначение", Type = "Текст", Value = "" };
            EnginePower = new Attribute<double> { DefValue = 0, EdIzm = "кВт", Name = "Мощность электродвигателя", Type = "Вещественное", Value = 0 };
            Productivity = new Attribute<double> { DefValue = 0, EdIzm = "куб. м/ч", Name = "Производительность", Type = "Вещественное", Value = 0 };
            Ispolnenie = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Исполнение", Type = "Текст", Value = "" };
            Napor = new Attribute<double> { DefValue = 0, EdIzm = "м", Name = "Напор", Type = "Вещественное", Value = 0 };
            ShifrByCalssificator_OsnovnSredstva.Value = 60000;
            Name_OsnovnSredstva.Value = "Насос";
        }
    }
}
