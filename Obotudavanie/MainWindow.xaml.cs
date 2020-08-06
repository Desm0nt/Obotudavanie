using Obotudavanie.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Obotudavanie
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        Excel excel = new Excel();
        List<Oborudovanie> LoadedOborud = new List<Oborudovanie>();
        List<Oborudovanie> oborudovanies = new List<Oborudovanie>();
        Dictionary<int, string> OborList = new Dictionary<int, string>();

        public MainWindow()
        {
            InitializeComponent();

            KPT kpt = new KPT();
            kpt.Name_OsnovnSredstva.Value = "КПТ";

            ElectroEngine electroEngine = new ElectroEngine();
            electroEngine.Name_OsnovnSredstva.Value = "Электродвигатель";

            Kotel Kotel = new Kotel();
            Kotel.Name_OsnovnSredstva.Value = "Котел";

            string output = JsonConvert.SerializeObject(Kotel);

            oborudovanies.Add(kpt);
            oborudovanies.Add(electroEngine);
            oborudovanies.Add(Kotel);
            Dictionary<int, string> namesList = new Dictionary<int, string>();
            for(int i = 0; i<oborudovanies.Count; i++)
            {
                namesList.Add((i+1), oborudovanies[i].Name_OsnovnSredstva.Value);
            }

            //var listOfFields1 = kpt2.GetType().GetProperties().ToList();
            //IList<IAttribute> attList = new List<IAttribute>();
            //foreach (var a in listOfFields1)
            //{
            //    var propvalue = a.GetValue(kpt2, null) as IAttribute;
            //    attList.Add(propvalue);
            //}

            ListGrid0.ItemsSource = namesList;
        }
        private void ListGrid0_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);

            var oborud = oborudovanies[dataGrid.SelectedIndex];
            Console.WriteLine(oborud.GetType().ToString());
            var listOfFields1 = oborud.GetType().GetProperties().ToList();
            IList<IAttribute> attList = new List<IAttribute>();
            foreach (var a in listOfFields1)
            {
                var propvalue = a.GetValue(oborud, null) as IAttribute;
                attList.Add(propvalue);
            }
            ClassGrid.ItemsSource = attList;
        }

        private void btn_openFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            openfile.DefaultExt = ".xlsx";
            openfile.Filter = "(.xlsx)|*.xlsx";
            string path = "";

            var browsefile = openfile.ShowDialog();

            if (browsefile == true)
            {
                path = openfile.FileName;
                IList<ElectroEngine> objExcelCon = excel.ReadExcel(path);
                foreach (var obj in objExcelCon)
                {
                    bool containsItem = LoadedOborud.Any(item => item.InvNum_OsnovnSredstva.Value == obj.InvNum_OsnovnSredstva.Value);
                    if (!containsItem)
                    {
                        LoadedOborud.Add(obj);
                    }
                }
                UpdateOborList();
            }

        }

        private void btn_GetData_Click(object sender, RoutedEventArgs e)
        {

            string url = "https://dev.beloil.by/cint/kisnpops/hs/ref/osbyperson/1090/";
            var req = (HttpWebRequest)WebRequest.Create(url);
            req.Credentials = new NetworkCredential("august", "august08");
            var response = req.GetResponse();
            using (var reader = new System.IO.StreamReader(response.GetResponseStream())) 
            { 
                string responseBody = reader.ReadToEnd();
                var jarray = (JArray)JsonConvert.DeserializeObject(responseBody);
                foreach (var jobj in jarray)
                {
                    Oborudovanie obor = new Oborudovanie();
                    obor.InvNum_OsnovnSredstva.Value = int.Parse(jobj["ИнвентарныйНомер"].ToString());
                    obor.Name_OsnovnSredstva.Value = jobj["Наименование"].ToString();
                    obor.ShifrByCalssificator_OsnovnSredstva.Value = int.Parse(jobj["Шифр"].ToString());
                    obor.RUP_PartName.Value = jobj["ПодразделениеРУП"].ToString();
                    obor.ORG_PartName.Value = jobj["Подразделение"].ToString();
                    obor.Year_OsnovnSredstva.Value = int.Parse(jobj["ГодВыпуска"].ToString());
                    obor.Vvod_v_Expl_Date.Value = DateTime.ParseExact(jobj["ДатаВводаВЭксплуатацию"].ToString().Replace("000000", ""), "yyyyMMdd",CultureInfo.InvariantCulture);
                    obor.MatOtv_Person.Value = jobj["МОЛ"].ToString();
                    obor.Dislocation_OsnovnSredstva.Value = jobj["Местонахождение"].ToString();
                    LoadedOborud.Add(obor);
                }
            }
            UpdateOborList();
        }

        private void ListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
            DataGridCell RowColumn = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
            int CellValue = Int32.Parse(((TextBlock)RowColumn.Content).Text);
            int selectedIndex = LoadedOborud.FindIndex(a => a.InvNum_OsnovnSredstva.Value == CellValue);

            DataSet dataSetData = new DataSet();
            var a22 = LoadedOborud[selectedIndex];

            Oborudovanie eEngine = LoadedOborud[selectedIndex];
            var listOfFields1 = eEngine.GetType().GetProperties().ToList();
            IList<IAttribute> attList = new List<IAttribute>();
            foreach (var a in listOfFields1)
            {
                var propvalue = a.GetValue(eEngine, null) as IAttribute;
                attList.Add(propvalue);
            }
            dtGrid_dataOutput.ItemsSource = attList;


        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //using (ExcelEngine excelEngine = new ExcelEngine())
            //{
            //IApplication application = excelEngine.Excel;
            //application.DefaultVersion = ExcelVersion.Excel2016;

            ////Assign the data to the customerObjects collection.
            //System.Collections.IEnumerable customerObjects = dsOborudovanieInfoList;

            ////Create a new workbook.
            //IWorkbook workbook = application.Workbooks.Create(1);
            //IWorksheet sheet = workbook.Worksheets[0];

            ////Import data from customerObjects collection.
            //sheet.ImportData(customerObjects, 1, 1, false);

            //sheet.UsedRange.AutofitColumns();

            ////Save the file in the given path.
            //Stream excelStream = File.Create(Path.GetFullPath(@"Output.xlsx"));
            //workbook.SaveAs(excelStream);
            //excelStream.Dispose();              

            //}
        }

        private void UpdateOborList()
        {
            OborList = new Dictionary<int, string>();
            for (int i = 0; i < LoadedOborud.Count; i++)
            {
                OborList.Add(LoadedOborud[i].InvNum_OsnovnSredstva.Value, LoadedOborud[i].Name_OsnovnSredstva.Value.ToString());
            }
            ListGrid.ItemsSource = OborList;
        }
    }
}

