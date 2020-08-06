using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace Obotudavanie.Classes
{
    public class Excel
    {
        public List<ElectroEngine> dsOborudovanieInfoList { get; set; }

        public IList<ElectroEngine> ReadExcel(string path)
        {

            IList<ElectroEngine> objOborudovanieInfo = new List<ElectroEngine>();
            try
            {
                //OleDbConnection oledbConn = OpenConnection(path);
                //if (oledbConn.State == ConnectionState.Open)
                //{
                    objOborudovanieInfo = ExtractDataExcel2(path);
                //    oledbConn.Close();
                //}
            }
            catch (Exception ex)
            {

            }
            return objOborudovanieInfo;
        }

        private OleDbConnection OpenConnection(string path)
        {
            OleDbConnection oledbConn = null;
            try
            {
                if (System.IO.Path.GetExtension(path) == ".xls")
                    oledbConn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" + path + "; Extended Properties= \"Excel 8.0;HDR=Yes;IMEX=2\"");
                else if (System.IO.Path.GetExtension(path) == ".xlsx")
                    oledbConn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + path + "; Extended Properties='Excel 12.0;HDR=YES;IMEX=1;';");

                oledbConn.Open();
            }
            catch (Exception ex)
            {

            }
            return oledbConn;
        }
        private IList<ElectroEngine> ExtractDataExcel2(string path)
        {
            DataSet dsOborudovanieInfo = new DataSet();
            using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                var reader = ExcelReaderFactory.CreateReader(stream);
                dsOborudovanieInfo = reader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    // Gets or sets a callback to obtain configuration options for a DataTable. 
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {

                        // Gets or sets a value indicating whether to use a row from the 
                        // data as column names.
                        UseHeaderRow = true,                    
                    }
                });
            }
            var dsOborudovanieInfoList2 = dsOborudovanieInfo.Tables[0].AsEnumerable();
            dsOborudovanieInfoList = dsOborudovanieInfo.Tables[0].AsEnumerable().Select(s => new ElectroEngine
            {
                InvNum_OsnovnSredstva = new Attribute<int> { DefValue = 0, EdIzm = "", Name = "Инвентарный номер основного средства", Type = "Целое", Value = Convert.ToInt32(s["Инвентарный номер основного средства"]) },
                Name_OsnovnSredstva = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Наименование основного средства ", Type = "Текст", Value = Convert.ToString(s["Наименование основного средства"] != DBNull.Value ? s["Наименование основного средства"] : "") },
                ShifrByCalssificator_OsnovnSredstva = new Attribute<int> { DefValue = 0, EdIzm = "", Name = "Шифр основного средства по классификатору", Type = "Целое", Value = Convert.ToInt32(s["Шифр основного средства по классификатору"]) },
                RUP_PartName = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Наименование подразделения РУП", Type = "Текст", Value = Convert.ToString(s["Наименование подразделения РУП"] != DBNull.Value ? s["Наименование подразделения РУП"] : "") },
                ORG_PartName = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Наименование подразделения внутри предприятия", Type = "Текст", Value = Convert.ToString(s["Наименование подразделения внутри предприятия"] != DBNull.Value ? s["Наименование подразделения внутри предприятия"] : "") },
                Year_OsnovnSredstva = new Attribute<int> { DefValue = 0, EdIzm = "", Name = "Год выпуска основного средства", Type = "Целое", Value = Convert.ToInt32(s["Год выпуска основного средства"]) },
                Vvod_v_Expl_Date = new Attribute<DateTime> { DefValue = DateTime.Now, EdIzm = "", Name = "Дата ввода в эксплуатацию на последнем месте работы", Type = "Дата", Value = Convert.ToDateTime(s["Дата ввода в эксплуатацию на последнем месте работы"]) },
                MatOtv_Person = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Материально-ответственное лицо", Type = "Текст", Value = Convert.ToString(s["Материально-ответственное лицо"]) },
                Dislocation_OsnovnSredstva = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Местоположение основного средства", Type = "Текст", Value = Convert.ToString(s["Местоположение основного средства"] != DBNull.Value ? s["Местоположение основного средства"] : "") },

                RotorFrequency = new Attribute<double> { DefValue = 0, EdIzm = "Гц", Name = "Частота вращения ротора", Type = "Вещественное", Value = Convert.ToDouble(s["Частота вращения ротора"]) },
                Power = new Attribute<double> { DefValue = 0, EdIzm = "кВ (6, 10)", Name = "Мощность", Type = "Вещественное", Value = Convert.ToDouble(s["Мощность"]) },
                Voltage = new Attribute<double> { DefValue = 0, EdIzm = "кВА", Name = "Напряжение", Type = "Вещественное", Value = Convert.ToDouble(s["Напряжение"]) },
                FactoryNumber = new Attribute<string> { DefValue = "", EdIzm = "КГ/ч", Name = "Шифр основного средства по классификатору", Type = "Текст", Value = Convert.ToString(s["Шифр основного средства по классификатору"]) }

            }).ToList();

            //for (int i = 0; i < dsOborudovanieInfoList.Count; i++)
            //{
            //    cmbox_dataSelection.Items.Add(dsOborudovanieInfoList[i].InvNum_OsnovnSredstva.Value.ToString() + "  " + dsOborudovanieInfoList[i].Name_OsnovnSredstva.Value.ToString());
            //}

            //textBox1.Text += "\n\n" + "Наименование:  " + dsOborudovanieInfoList[j].Name_OsnovnSredstva.Name.ToString() + "\n"
            //       + "Тип:  " + dsOborudovanieInfoList[j].Name_OsnovnSredstva.Type.ToString() + "\n"
            //       + "Единица измерения:  " + dsOborudovanieInfoList[j].Name_OsnovnSredstva.EdIzm.ToString() + "\n"
            //       + "Значение:  " + dsOborudovanieInfoList[j].Name_OsnovnSredstva.Value.ToString() + "\n"
            //       + "Материально-ответственное лицо:  " + dsOborudovanieInfoList[j].MatOtv_Person.Value.ToString() + "\n";

            return dsOborudovanieInfoList;
        }

        private IList<ElectroEngine> ExtractDataExcel(OleDbConnection oledbConn)
        {
            OleDbCommand cmd = new OleDbCommand(); ;
            OleDbDataAdapter oleda = new OleDbDataAdapter();
            DataSet dsOborudovanieInfo = new DataSet();

            cmd.Connection = oledbConn;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM [Test$]";
            oleda = new OleDbDataAdapter(cmd);
            oleda.Fill(dsOborudovanieInfo, "Test");

            dsOborudovanieInfoList = dsOborudovanieInfo.Tables[0].AsEnumerable().Select(s => new ElectroEngine
            {

                InvNum_OsnovnSredstva = new Attribute<int> { DefValue = 0, EdIzm = "", Name = "Инвентарный номер основного средства", Type = "Целое", Value = Convert.ToInt32(s["Инвентарный номер основного средства"]) },
                Name_OsnovnSredstva = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Наименование основного средства ", Type = "Текст", Value = Convert.ToString(s["Наименование основного средства"] != DBNull.Value ? s["Наименование основного средства"] : "") },
                ShifrByCalssificator_OsnovnSredstva = new Attribute<int> { DefValue = 0, EdIzm = "", Name = "Шифр основного средства по классификатору", Type = "Целое", Value = Convert.ToInt32(s["Шифр основного средства по классификатору"]) },
                RUP_PartName = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Наименование подразделения РУП", Type = "Текст", Value = Convert.ToString(s["Наименование подразделения РУП"] != DBNull.Value ? s["Наименование подразделения РУП"] : "") },
                ORG_PartName = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Наименование подразделения внутри предприятия", Type = "Текст", Value = Convert.ToString(s["Наименование подразделения внутри предприятия"] != DBNull.Value ? s["Наименование подразделения внутри предприятия"] : "") },
                Year_OsnovnSredstva = new Attribute<int> { DefValue = 0, EdIzm = "", Name = "Год выпуска основного средства", Type = "Целое", Value = Convert.ToInt32(s["Год выпуска основного средства"]) },
                Vvod_v_Expl_Date = new Attribute<DateTime> { DefValue = DateTime.Now, EdIzm = "", Name = "Дата ввода в эксплуатацию на последнем месте работы", Type = "Дата", Value = Convert.ToDateTime(s["Дата ввода в эксплуатацию на последнем месте работы"]) },
                MatOtv_Person = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Материально-ответственное лицо", Type = "Текст", Value = Convert.ToString(s["Материально-ответственное лицо"]) },
                Dislocation_OsnovnSredstva = new Attribute<string> { DefValue = "", EdIzm = "", Name = "Местоположение основного средства", Type = "Текст", Value = Convert.ToString(s["Местоположение основного средства"] != DBNull.Value ? s["Местоположение основного средства"] : "") },

                RotorFrequency = new Attribute<double> { DefValue = 0, EdIzm = "Гц", Name = "Частота вращения ротора", Type = "Вещественное", Value = Convert.ToDouble(s["Частота вращения ротора"]) },
                Power = new Attribute<double> { DefValue = 0, EdIzm = "кВ (6, 10)", Name = "Мощность", Type = "Вещественное", Value = Convert.ToDouble(s["Мощность"]) },
                Voltage = new Attribute<double> { DefValue = 0, EdIzm = "кВА", Name = "Напряжение", Type = "Вещественное", Value = Convert.ToDouble(s["Напряжение"]) },
                FactoryNumber = new Attribute<string> { DefValue = "", EdIzm = "КГ/ч", Name = "Шифр основного средства по классификатору", Type = "Текст", Value = Convert.ToString(s["Шифр основного средства по классификатору"]) }

            }).ToList();

            //for (int i = 0; i < dsOborudovanieInfoList.Count; i++)
            //{
            //    cmbox_dataSelection.Items.Add(dsOborudovanieInfoList[i].InvNum_OsnovnSredstva.Value.ToString() + "  " + dsOborudovanieInfoList[i].Name_OsnovnSredstva.Value.ToString());
            //}

            //textBox1.Text += "\n\n" + "Наименование:  " + dsOborudovanieInfoList[j].Name_OsnovnSredstva.Name.ToString() + "\n"
            //       + "Тип:  " + dsOborudovanieInfoList[j].Name_OsnovnSredstva.Type.ToString() + "\n"
            //       + "Единица измерения:  " + dsOborudovanieInfoList[j].Name_OsnovnSredstva.EdIzm.ToString() + "\n"
            //       + "Значение:  " + dsOborudovanieInfoList[j].Name_OsnovnSredstva.Value.ToString() + "\n"
            //       + "Материально-ответственное лицо:  " + dsOborudovanieInfoList[j].MatOtv_Person.Value.ToString() + "\n";

            return dsOborudovanieInfoList;
        }

    }
}
