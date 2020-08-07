namespace Obotudavanie.Classes
{
    class KPT:Oborudovanie
    {
        public Attribute<string> Type { get; set; }
        public Attribute<double> Power { get; set; }
        public Attribute<double> Voltage { get; set; }
        public Attribute<string> FactoryNumber { get; set; }

        public KPT()
        {
            Type = new Attribute<string> { DefValue = "", EdIzm = "КТПНД, КТПТАС", Name = "Тип", Type = "Текст", Value = "" };
            Power = new Attribute<double> { DefValue = 0, EdIzm = "кВ (6, 10)", Name = "Мощность", Type = "Вещественное", Value = 0 };
            Voltage = new Attribute<double> { DefValue = 0, EdIzm = "кВА", Name = "Напряжение", Type = "Вещественное", Value = 0 };
            FactoryNumber = new Attribute<string> { DefValue = "", EdIzm = "КГ/ч", Name = "Заводской номер", Type = "Текст", Value = "" };
            Name_OsnovnSredstva.Value = "КПТ";
        }
    }
}
