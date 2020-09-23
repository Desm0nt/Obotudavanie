using MongoDB.Bson.Serialization.Attributes;

namespace Obotudavanie.Classes
{
    [BsonIgnoreExtraElements]
    public class ElectroEngine: Oborudovanie
    {
        public Attribute<double> RotorFrequency { get; set; }
        public Attribute<double> Power { get; set; }
        public Attribute<double> Voltage { get; set; }
        public Attribute<string> FactoryNumber { get; set; }

        public ElectroEngine()
        {
            RotorFrequency = new Attribute<double> { DefValue = 0, EdIzm = "", Name = "Частота вращения ротора", Type = "вещественное", Value = 0 };
            Power = new Attribute<double> { DefValue = 0, EdIzm = "кВ (6, 10)", Name = "Мощность", Type = "Вещественное", Value = 0 };
            Voltage = new Attribute<double> { DefValue = 0, EdIzm = "кВА", Name = "Напряжение", Type = "Вещественное", Value = 0 };
            FactoryNumber = new Attribute<string> { DefValue = "", EdIzm = "КГ/ч", Name = "Заводской номер", Type = "Текст", Value = "" };
            ShifrByCalssificator_OsnovnSredstva.Value = 40251;
            Name_OsnovnSredstva.Value = "Электродвигатель";
        }


    }
}
