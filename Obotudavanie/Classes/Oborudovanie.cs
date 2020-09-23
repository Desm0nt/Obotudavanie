using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace Obotudavanie.Classes
{
    [BsonIgnoreExtraElements]
    [BsonKnownTypes(typeof(ElectroEngine), typeof(ChemicalWaterCleaningSystem), typeof(HeatExchanger), typeof(Kotel), typeof(Nasos), typeof(KPT), typeof(PowerTransformator), typeof(SNK_ControlStation))]
    public class Oborudovanie
    {
        public Attribute<int> InvNum_OsnovnSredstva { get; set; }
        public Attribute<string> Name_OsnovnSredstva { get; set; }
        public Attribute<int> ShifrByCalssificator_OsnovnSredstva { get; set; }
        public Attribute<string> RUP_PartName { get; set; }
        public Attribute<string> ORG_PartName { get; set; }
        public Attribute<int> Year_OsnovnSredstva { get; set; }
        public Attribute<DateTime> Vvod_v_Expl_Date { get; set; }
        public Attribute<string> MatOtv_Person { get; set; }
        public Attribute<string> Dislocation_OsnovnSredstva { get; set; }

        public Oborudovanie()
        {
            InvNum_OsnovnSredstva = new Attribute<int> { DefValue = 0, EdIzm = "", Name = "Инвентарный номер основного средства ", Type = "Целое", Value = 0 };
            Name_OsnovnSredstva = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Наименование основного средства ", Type = "Текст", Value = "" };
            ShifrByCalssificator_OsnovnSredstva = new Attribute<int> { DefValue = 0, EdIzm = "", Name = "Шифр основного средства по классификатору", Type = "Целое", Value = 0 };
            RUP_PartName = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Наименование подразделения РУП", Type = "Текст", Value = "" };
            ORG_PartName = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Наименование подразделения внутри предприятия", Type = "Текст", Value = "" };
            Year_OsnovnSredstva = new Attribute<int> { DefValue = 0, EdIzm = "", Name = "Год выпуска основного средства", Type = "Целое", Value = 0 };
            Vvod_v_Expl_Date = new Attribute<DateTime> { DefValue = DateTime.Now, EdIzm = "", Name = "Дата ввода в эксплуатацию на последнем месте работы", Type = "Дата", Value = DateTime.Now };
            MatOtv_Person = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Материально-ответственное лицо", Type = "Текст", Value = "" };
            Dislocation_OsnovnSredstva = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Местоположение основного средства", Type = "Текст", Value = "" };

        }


    }
}
