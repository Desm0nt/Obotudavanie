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
using System.Windows.Input;
using System.Windows.Media;

namespace Obotudavanie
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        Excel excel = new Excel();
        List<Oborudovanie> LoadedOborud = new List<Oborudovanie>();
        List<Oborudovanie> LoadedOborud2 = new List<Oborudovanie>();
        List<Oborudovanie> oborudovanies = new List<Oborudovanie>();
        List<Oborudovanie> oborudovanies_editor = new List<Oborudovanie>();
        Dictionary<int, string> OborList = new Dictionary<int, string>();

        public MainWindow()
        {
            InitializeComponent();

            dtGrid_dataOutput.CellEditEnding += dtGrid_dataOutput_CellEditEnding;
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

            oborudovanies_editor.AddRange(new List<Oborudovanie>()
            {
                new KPT(),
                new ElectroEngine(),
                new Kotel(),
                new Nasos(),
                new HeatExchanger(),
                new PowerTransformator(),
                new SNK_ControlStation(),
                new Oborudovanie()
            });

            Dictionary<int, string> namesList = new Dictionary<int, string>();
            Dictionary<int, string> namesList2 = new Dictionary<int, string>();
            for (int i = 0; i<oborudovanies.Count; i++)
            {
                namesList.Add((i+1), oborudovanies[i].Name_OsnovnSredstva.Value);
            }

            for (int i = 0; i < oborudovanies_editor.Count; i++)
            {
                namesList2.Add((i + 1), oborudovanies_editor[i].Name_OsnovnSredstva.Value);
            }
            //var listOfFields1 = kpt2.GetType().GetProperties().ToList();
            //IList<IAttribute> attList = new List<IAttribute>();
            //foreach (var a in listOfFields1)
            //{
            //    var propvalue = a.GetValue(kpt2, null) as IAttribute;
            //    attList.Add(propvalue);
            //}

            ListGrid0.ItemsSource = namesList;
            ListGrid01.ItemsSource = namesList2;
        }


        #region выбор
        private void ListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.SelectedIndex >= 0)
            {
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
        }

        private void ListGrid0_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);

            var oborud = oborudovanies_editor[dataGrid.SelectedIndex];
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

        private void ListGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.SelectedIndex >= 0)
            {
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
                DataGridCell RowColumn = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                int CellValue = Int32.Parse(((TextBlock)RowColumn.Content).Text);
                int selectedIndex = LoadedOborud2.FindIndex(a => a.InvNum_OsnovnSredstva.Value == CellValue);

                DataSet dataSetData = new DataSet();
                var a22 = LoadedOborud2[selectedIndex];

                Oborudovanie eEngine = LoadedOborud2[selectedIndex];
                var listOfFields1 = eEngine.GetType().GetProperties().ToList();
                IList<IAttribute> attList = new List<IAttribute>();
                foreach (var a in listOfFields1)
                {
                    var propvalue = a.GetValue(eEngine, null) as IAttribute;
                    attList.Add(propvalue);
                }
                dtGrid_dataOutput1.ItemsSource = attList;
            }
        }

        private void ListGrid01_Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListGrid01.Visibility = Visibility.Collapsed;
            ListGrid1.Visibility = Visibility.Visible;
            BackButton.IsEnabled = true;
            DataGridRow row = sender as DataGridRow;
            KeyValuePair<int, string> item = (KeyValuePair<int, string>)row.Item;

            var oborud = oborudovanies_editor[item.Key - 1];
            Console.WriteLine(oborud.GetType().Name.ToString());

            string connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("BN");
            var collection = database.GetCollection<Oborudovanie>("BNCol");
            var builder = Builders<Oborudovanie>.Filter;
            var filter = builder.Eq("_t", oborud.GetType().Name.ToString());
            if (item.Value == "Оборудование")
            {
                filter = builder.Not(builder.Exists("_t"));
            }
            LoadedOborud2 = collection.Find(filter).ToList();
            UpdateOborList2();
            e.Handled = true;
        }

        #endregion

        #region buttons
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
        private void btn_GetDataWindow_Click(object sender, RoutedEventArgs e)
        {
            GetWebDataWindow subWindow = new GetWebDataWindow()
            {
                Owner = System.Windows.Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };
            subWindow.Show();
            subWindow.Closed += subWindowClosed;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ListGrid1.Visibility = Visibility.Collapsed;
            ListGrid01.Visibility = Visibility.Visible;
            BackButton.IsEnabled = false;
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
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var accentBrush = TryFindResource("AccentColorBrush") as SolidColorBrush;
            if (accentBrush != null) accentBrush.Color.CreateAccentColors();
        }
        private void UpdateOborList2()
        {
            OborList = new Dictionary<int, string>();
            for (int i = 0; i < LoadedOborud2.Count; i++)
            {
                OborList.Add(LoadedOborud2[i].InvNum_OsnovnSredstva.Value, LoadedOborud2[i].Name_OsnovnSredstva.Value.ToString());
            }
            ListGrid1.ItemsSource = OborList;
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
        private string GetSelectedValue(DataGrid grid)
        {
            DataGridCellInfo cellInfo = grid.SelectedCells[0];
            if (cellInfo == null) return null;

            DataGridBoundColumn column = cellInfo.Column as DataGridBoundColumn;
            if (column == null) return null;

            FrameworkElement element = new FrameworkElement() { DataContext = cellInfo.Item };
            BindingOperations.SetBinding(element, TagProperty, column.Binding);

            return element.Tag.ToString();
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
        public void subWindowClosed(object sender, System.EventArgs e)
        {
            string connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("BN");
            var collection = database.GetCollection<Oborudovanie>("BNCol");
            var filter = new BsonDocument();
            LoadedOborud = collection.Find(filter).ToList();
            UpdateOborList();
        }
    }
}

