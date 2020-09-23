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
using System.IO;
using System.Windows.Data;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace Obotudavanie
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        Excel excel = new Excel();
        List<Oborudovanie> LoadedOborud = new List<Oborudovanie>();
        static List<Oborudovanie> LoadedOborud2 = new List<Oborudovanie>();
        List<Oborudovanie> oborudovanies = new List<Oborudovanie>();
        Dictionary<int, string> OborList = new Dictionary<int, string>();

        public MainWindow()
        {
            InitializeComponent();

            dtGrid_dataOutput.CellEditEnding += dtGrid_dataOutput_CellEditEnding;
            //if (File.Exists("BD.json"))
            //{
            //    JsonSerializerSettings settings = new JsonSerializerSettings
            //    {
            //        TypeNameHandling = TypeNameHandling.Auto
            //    };
            //    LoadedOborud = JsonConvert.DeserializeObject<List<Oborudovanie>>(File.ReadAllText("BD.json"), settings);
            //}
            string connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("BN");
            var collection = database.GetCollection<Oborudovanie>("BNCol");
            var filter = new BsonDocument();
            LoadedOborud = collection.Find(filter).ToList();
            UpdateOborList();

            oborudovanies.AddRange(new List<Oborudovanie>()
            { 
                new KPT(),
                new ElectroEngine(), 
                new Kotel(), 
                new Nasos(), 
                new HeatExchanger(),
                new PowerTransformator(),
                new SNK_ControlStation()
            });

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
                    if (!LoadedOborud.Any(item => item.InvNum_OsnovnSredstva.Value == int.Parse(jobj["ИнвентарныйНомер"].ToString())))
                    {
                        Oborudovanie obor = new Oborudovanie();
                        bool containsItem = oborudovanies.Any(item => item.ShifrByCalssificator_OsnovnSredstva.Value == int.Parse(jobj["Шифр"].ToString()));
                        if (containsItem)
                        {
                            var item = oborudovanies.Single(a => a.ShifrByCalssificator_OsnovnSredstva.Value == int.Parse(jobj["Шифр"].ToString()));
                            Type type = item.GetType();
                            obor = (Oborudovanie)Activator.CreateInstance(type);
                        }
                        obor.InvNum_OsnovnSredstva.Value = int.Parse(jobj["ИнвентарныйНомер"].ToString());
                        obor.Name_OsnovnSredstva.Value = jobj["Наименование"].ToString();
                        obor.ShifrByCalssificator_OsnovnSredstva.Value = int.Parse(jobj["Шифр"].ToString());
                        obor.RUP_PartName.Value = jobj["ПодразделениеРУП"].ToString();
                        obor.ORG_PartName.Value = jobj["Подразделение"].ToString();
                        obor.Year_OsnovnSredstva.Value = int.Parse(jobj["ГодВыпуска"].ToString());
                        obor.Vvod_v_Expl_Date.Value = DateTime.ParseExact(jobj["ДатаВводаВЭксплуатацию"].ToString().Replace("000000", ""), "yyyyMMdd", CultureInfo.InvariantCulture);
                        obor.MatOtv_Person.Value = jobj["МОЛ"].ToString();
                        obor.Dislocation_OsnovnSredstva.Value = jobj["Местонахождение"].ToString();
                        LoadedOborud.Add(obor);
                    }
                }
            }
            string connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("BN");
            var collection = database.GetCollection<Oborudovanie>("BNCol");
            var cust1 = new BsonDocument();
            collection.InsertMany(LoadedOborud);
            string json = JsonConvert.SerializeObject(LoadedOborud, Formatting.Indented);
            UpdateOborList();
        }

        private static async Task SaveDocs(List<Oborudovanie> Oborud)
        {
            string connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase("BN");
            var collection = database.GetCollection<Oborudovanie>("BNCol");
            var cust1 = new BsonDocument();
            cust1.Add("Items", new BsonArray(Oborud.Select(i =>
            i.ToBsonDocument())));
            await collection.InsertManyAsync(Oborud);
        }

        private void btn_GetData_Click2(object sender, RoutedEventArgs e)
        {
            //List<string> ToTable = new List<string>();
            //string url = "https://dev.beloil.by/cint/kisnpops/hs/ref/osbyperson/1090/";
            //var req = (HttpWebRequest)WebRequest.Create(url);
            //req.Credentials = new NetworkCredential("august", "august08");
            //var response = req.GetResponse();
            //using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
            //{
            //    string responseBody = reader.ReadToEnd();
            //    var jarray = (JArray)JsonConvert.DeserializeObject(responseBody);
            //    foreach (var jobj in jarray)
            //    {
            //        string str1 = jobj["Наименование"].ToString() + ";" + jobj["ИнвентарныйНомер"].ToString() + ";" + DateTime.ParseExact(jobj["ДатаВводаВЭксплуатацию"].ToString().Replace("000000", ""), "yyyyMMdd", CultureInfo.InvariantCulture).ToString() + ";" + jobj["ГодВыпуска"].ToString() + ";" + jobj["МОЛ"].ToString() + ";" + jobj["Подразделение"].ToString() + ";" + jobj["Шифр"].ToString() + ";" + jobj["Местонахождение"].ToString() + ";" + jobj["ПодразделениеРУП"].ToString() + ";\n";
            //        ToTable.Add(str1);
            //    }
            //    File.WriteAllLines("list.csv", ToTable);
            //}
            string connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("BN");
            var collection = database.GetCollection<Oborudovanie>("BNCol");
            var filter = new BsonDocument();
            var BNCol = collection.Find(filter).ToList();
            foreach (var doc in BNCol)
            {
                Console.WriteLine(doc);
            }
            FindDocs();
        }

        private static async Task FindDocs()
        {
            string connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("BN");
            var collection = database.GetCollection<Oborudovanie>("BNCol");
            var filter = new BsonDocument();
            var BNCol = await collection.Find(filter).ToListAsync();
            foreach (var doc in BNCol)
            {
                Console.WriteLine(doc);
            }
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

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            File.WriteAllText("BD.json", JsonConvert.SerializeObject(LoadedOborud, settings));
        }

        void dtGrid_dataOutput_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            //if (e.EditAction == DataGridEditAction.Commit)
            //{
            //    var column = e.Column as DataGridBoundColumn;
            //    if (column != null)
            //    {
            //        var bindingPath = (column.Binding as Binding).Path.Path;
            //            int rowIndex = e.Row.GetIndex();
            //        var MyRow = e.Row.Item.GetType().ToString();

            //            // rowIndex has the row index
            //            // bindingPath has the column's binding
            //            // el.Text has the new, user-entered value
            //    }
            //}

        }
    }
}

