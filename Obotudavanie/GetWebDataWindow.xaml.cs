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

namespace Obotudavanie
{
    /// <summary>
    /// Логика взаимодействия для GetWebDataWindow.xaml
    /// </summary>
    public partial class GetWebDataWindow : System.Windows.Window
    {
        int indexObor = 0;
        List<Oborudovanie> LoadedOborud = new List<Oborudovanie>();
        List<Oborudovanie> oborudovanies = new List<Oborudovanie>();
        Dictionary<int, string> namesList = new Dictionary<int, string>();
        public KeyValuePair<Type, string> keyItem { get; set; }
        public Dictionary<Type, string> TypeNameList { get; set; }
        Dictionary<long, string> OborList = new Dictionary<long, string>();

        public GetWebDataWindow()
        {
            InitializeComponent();

            oborudovanies.AddRange(new List<Oborudovanie>()
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

            TypeNameList = new Dictionary<Type, string>();
            for (int i = 0; i < oborudovanies.Count; i++)
            {
                namesList.Add(i, oborudovanies[i].Name_OsnovnSredstva.Value);
                TypeNameList.Add(oborudovanies[i].GetType(), oborudovanies[i].Name_OsnovnSredstva.Value);
            }
            typeNameListCombobox.ItemsSource = TypeNameList;
            typeNameListCombobox.SelectedIndex = 0;
        }

        private void typeNameListCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (typeNameListCombobox.SelectedItem != null)
            {
                keyItem = (KeyValuePair<Type, string>)typeNameListCombobox.SelectedItem;
                Type selKey = keyItem.Key;
                string selvalue = keyItem.Value;
            }
        }

        private void btn_LoadButton_Click(object sender, RoutedEventArgs e)
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
            //string connectionString = "mongodb://localhost:27017";
            //MongoClient client = new MongoClient(connectionString);
            //IMongoDatabase database = client.GetDatabase("BN");
            //var collection = database.GetCollection<Oborudovanie>("BNCol");
            //var cust1 = new BsonDocument();
            //collection.InsertMany(LoadedOborud);
            //string json = JsonConvert.SerializeObject(LoadedOborud, Formatting.Indented);
            UpdateOborList();
        }

        private void UpdateOborList()
        {
            OborList = new Dictionary<long, string>();
            for (int i = 0; i < LoadedOborud.Count; i++)
            {
                OborList.Add(LoadedOborud[i].InvNum_OsnovnSredstva.Value, LoadedOborud[i].Name_OsnovnSredstva.Value.ToString());
            }
            ListGrid.ItemsSource = OborList;
        }

        private void ListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;
            if (dataGrid.SelectedIndex >= 0)
            {
                DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
                DataGridCell RowColumn = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                int CellValue = Int32.Parse(((TextBlock)RowColumn.Content).Text);
                int selectedIndex = LoadedOborud.FindIndex(a => a.InvNum_OsnovnSredstva.Value == CellValue);
                indexObor = selectedIndex;

                DataSet dataSetData = new DataSet();
                var a22 = LoadedOborud[selectedIndex];

                var item = TypeNameList.Single(a => a.Key == a22.GetType());
                TypeLable.Content = item.Value;

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

        private void btn_TypeChangeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (indexObor >= 0 && LoadedOborud.Count>0)
                {
                    var a = LoadedOborud[indexObor];
                    Console.WriteLine(LoadedOborud[indexObor].GetType().ToString());
                    LoadedOborud[indexObor] = (Oborudovanie)Activator.CreateInstance(keyItem.Key);

                    LoadedOborud[indexObor].InvNum_OsnovnSredstva.Value = a.InvNum_OsnovnSredstva.Value;
                    LoadedOborud[indexObor].Name_OsnovnSredstva.Value = a.Name_OsnovnSredstva.Value;
                    LoadedOborud[indexObor].ShifrByCalssificator_OsnovnSredstva.Value = a.ShifrByCalssificator_OsnovnSredstva.Value;
                    LoadedOborud[indexObor].RUP_PartName.Value = a.RUP_PartName.Value;
                    LoadedOborud[indexObor].ORG_PartName.Value = a.ORG_PartName.Value;
                    LoadedOborud[indexObor].Year_OsnovnSredstva.Value = a.Year_OsnovnSredstva.Value;
                    LoadedOborud[indexObor].Vvod_v_Expl_Date.Value = a.Vvod_v_Expl_Date.Value;
                    LoadedOborud[indexObor].MatOtv_Person.Value = a.MatOtv_Person.Value;
                    LoadedOborud[indexObor].Dislocation_OsnovnSredstva.Value = a.Dislocation_OsnovnSredstva.Value;

                    Console.WriteLine(LoadedOborud[indexObor].GetType().ToString());

                    var item = TypeNameList.Single(c => c.Key == LoadedOborud[indexObor].GetType());
                    TypeLable.Content = item.Value;

                    Oborudovanie eEngine = LoadedOborud[indexObor];
                    var listOfFields1 = eEngine.GetType().GetProperties().ToList();
                    IList<IAttribute> attList = new List<IAttribute>();
                    foreach (var b in listOfFields1)
                    {
                        var propvalue = b.GetValue(eEngine, null) as IAttribute;
                        attList.Add(propvalue);
                    }
                    dtGrid_dataOutput.ItemsSource = attList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btn_SaveAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = "mongodb://localhost:27017";
                MongoClient client = new MongoClient(connectionString);
                IMongoDatabase database = client.GetDatabase("BN");
                var collection = database.GetCollection<Oborudovanie>("BNCol");
                var cust1 = new BsonDocument();
                var builder = Builders<Oborudovanie>.Filter;
                

                foreach (var obor in LoadedOborud)
                {
                   var filter = builder.Eq("_id", obor.InvNum_OsnovnSredstva.ToBsonDocument());
                   var result = collection.ReplaceOne(filter, obor, new UpdateOptions { IsUpsert = true });
                   //collection.InsertOne(obor);
                }
                string json = JsonConvert.SerializeObject(LoadedOborud, Formatting.Indented);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            MessageBox.Show("Данные сохранены");
            this.Close();
        }
    }
}

