using CarStore.Domain;
using CarStore.Infrastracture;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CarStore.UserInterface
{
    public partial class AddCarForm : Form
    {
        public AddCarForm()
        {
            InitializeComponent();
            PrepairFields();
        }

        private void PrepairFields()
        {
            //var properties = typeof(Car).GetProperties();
            //int y = 0;
            //foreach (var e in properties)
            //{
            //    var attribute = e.GetCustomAttributes(true).OfType<LabelAttribute>().First();
            //    var label = new Label { Top = y, Left = 0, Width = ClientSize.Width / 2, Height = 16 };
            //    label.Text = attribute.LabelText;

            //    if(e.PropertyType.Name == "String")
            //    {
            //        var comboBox = new ComboBox() { Name = e.Name.ToLower(), Top = y, Left = label.Width, Width = label.Width, Height = 16, Enabled = false };
            //        Controls.Add(comboBox);
            //    }
            //    else
            //    {
            //        var numeric = new NumericUpDown() { Name = e.Name.ToLower(), Top = y, Left = label.Width, Width = label.Width, Height = 16, Enabled = false };
            //        Controls.Add(numeric);
            //    }
            //    Controls.Add(label);

            //    y += label.Height + 5;
            //}

            //comboBox1.DataSource = CarsRepository.SelectField("Brand", "brands");
            
        }

        private void BrandChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                comboBox2.SelectedIndex = -1;
                comboBox2.Enabled = false;
                return;
            }
            comboBox2.SelectedIndex = -1;
            comboBox2.Enabled = true;
            var selectedState = (int)comboBox1.SelectedValue;
            string connectionString = Properties.Settings.Default.CarStoreDbConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sqlDataAdapter = new SqlDataAdapter($"select Id, Name from models where BrandCode = {selectedState}", connection);
            //    var bindingSource = new BindingSource();
                var dataTable = new DataTable();

                connection.Open();
                sqlDataAdapter.Fill(dataTable);
                //    bindingSource.DataSource = dataTable;
                if (dataTable.Rows.Count != 0)
                    comboBox2.DataSource = dataTable;
                else
                    comboBox2.Enabled = false;
                connection.Close();
            }
            
            //
            //comboBox2.DataSource = CarsRepository.SelectField("ModelName", "models", $"BrandID = (select Id from brands where Brand = '{selectedState}')");
            
        }

        private void ModelChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem == null)
            {
                comboBox3.SelectedIndex = -1;
                comboBox3.Enabled = false;
                return;
            }
            var selectedState = comboBox2.SelectedValue;
            comboBox3.SelectedIndex = -1;
            //comboBox3.DataSource = CarsRepository.SelectField("GenerationName", "generations", $"ModelID = (select Id from models where ModelName = '{selectedState}')");
            comboBox3.Enabled = true;

            string connectionString = Properties.Settings.Default.CarStoreDbConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sqlDataAdapter = new SqlDataAdapter($"select Id, Name from generations where ModelCode = {selectedState}", connection);
               // var bindingSource = new BindingSource();
                var dataTable = new DataTable();

                connection.Open();
                sqlDataAdapter.Fill(dataTable);
                // bindingSource.DataSource = dataTable;
                if (dataTable.Rows.Count != 0)
                    comboBox3.DataSource = dataTable;
                else
                    comboBox3.Enabled = false;
                connection.Close();
            }
        }

        private void GenerationChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedItem == null)
            {
                numericUpDown1.Enabled = false;
                comboBox4.SelectedIndex = -1;
                comboBox4.Enabled = false;
                //comboBox5.SelectedIndex = -1;
                //comboBox5.Enabled = false;
                return;
            }
            
            var selectedModel = comboBox2.SelectedValue;
            var selectedGenName = comboBox3.SelectedText;
            var selectedGenId = comboBox3.SelectedValue;
            string connectionString = Properties.Settings.Default.CarStoreDbConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sqlDataAdapter1 = new SqlDataAdapter($"select StartManufacture, EndManufacture from generations where Name = '{selectedGenName}' and ModelCode = {selectedModel}", connection);
                var sqlDataAdapter2 = new SqlDataAdapter($"select Id, Name from modifications where GenerationCode = {selectedGenId}", connection);
                var dataTable1 = new DataTable();
                var dataTable2 = new DataTable();

                connection.Open();
                sqlDataAdapter1.Fill(dataTable1);
                sqlDataAdapter2.Fill(dataTable2);
                if (dataTable1.Rows.Count != 0)
                {
                    numericUpDown1.Minimum = (int)dataTable1.Rows[0][0];
                    numericUpDown1.Maximum = (int)dataTable1.Rows[0][1];
                    numericUpDown1.Enabled = true;
                }
                else
                    numericUpDown1.Enabled = false;
                if (dataTable2.Rows.Count != 0)
                {
                    comboBox4.Enabled = true;
                    comboBox4.DataSource = dataTable2;
                }
                else
                    comboBox4.Enabled = false;
                connection.Close();
            }
            // string selectedGen = comboBox3.SelectedItem.ToString();
            //string selectedModel = comboBox2.SelectedItem.ToString();
            /*var result = CarsRepository.SelectField("EndOfManufacture", "generations", $"GenerationName = '{selectedGen}' and ModelID = (select Id from models where ModelName = '{selectedModel}')")[0];
            if (result == "")
                numericUpDown1.Maximum = DateTime.Now.Year;
            else
                numericUpDown1.Maximum = int.Parse(result);
            result = CarsRepository.SelectField("StartOfManufacture", "generations", $"GenerationName = '{selectedGen}' and ModelID = (select Id from models where ModelName = '{selectedModel}')")[0];
            numericUpDown1.Minimum = int.Parse(result);
            numericUpDown1.Enabled = true;

            comboBox4.SelectedIndex = -1;
            var res1 = new SqlRequest()
                .Select("EnginePower")
                .From("characteristics")
                .InnerJoin("generations")
                .On("characteristics.GenerationID = generations.Id")
                .InnerJoin("models")
                .On("generations.ModelID = models.Id")
                .Where($"generations.GenerationName = '{selectedGen}' and models.ModelName = '{selectedModel}'")
                .GroupBy("EnginePower")
                .Execute();
            var dataSource1 = new List<string>();
            foreach (var val in res1)
                dataSource1.Add(val["EnginePower"].ToString());
            comboBox4.DataSource = dataSource1;//CarsRepository.SelectField("EnginePower", "characteristics", new SqlRequest().Select/*$"GenerationID = (select Id from generations where GenerationName = '{selectedGen}') and ModelID = (select Id from models where ModelName = '{selectedModel}') group by EnginePower");*/
            

            //comboBox5.SelectedIndex = -1;
            /*var res2 = new SqlRequest()
                .Select("TransmissionType")
                .From("characteristics")
                .InnerJoin("generations")
                .On("characteristics.GenerationID = generations.Id")
                .InnerJoin("models")
                .On("generations.ModelID = models.Id")
                .Where($"generations.GenerationName = '{selectedGen}' and models.ModelName = '{selectedModel}'")
                .GroupBy("TransmissionType")
                .Execute();
            var dataSource2 = new List<string>();
            foreach (var val in res2)
                dataSource2.Add(val["TransmissionType"].ToString());
            comboBox5.DataSource = dataSource2;
            //comboBox5.DataSource = CarsRepository.SelectField("TransmissionType", "characteristics", $"GenerationID = (select Id from generations where GenerationName = '{selectedGen}') group by TransmissionType");
            comboBox5.Enabled = true;*/
        }

        private void AddCarButton(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null ||
                comboBox2.SelectedValue == null ||
                comboBox3.SelectedValue == null ||
                comboBox4.SelectedValue == null)
            {
                MessageBox.Show("Заполнены не все поля");
                return;
            }
            if (numericUpDown3.Value == 0)
            {
                MessageBox.Show("Цена не может быть равной 0");
                return;
            }

            var brandCode = comboBox1.SelectedValue;
            var modelCode = comboBox2.SelectedValue;
            var genCode = comboBox3.SelectedValue;
            var year = numericUpDown1.Value;
            var modificationCode = comboBox4.SelectedValue;
            var condition = comboBox6.SelectedText;
            var mileage = numericUpDown2.Value;
            var price = numericUpDown3.Value;
            var status = "Выставлена на продажу";
            string connectionString = Properties.Settings.Default.CarStoreDbConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand($"insert into cars (BrandCode, ModelCode, GenerationCode, ModificationCode, YearOfManufacture, Condition, Mileage, Price, Status) values({brandCode}, {modelCode}, {genCode}, {modificationCode}, {year}, '{condition}', {mileage}, {price}, '{status}')", connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            /*var res = new SqlRequest()
                .Select("characteristics.Id")
                .From("characteristics")
                .InnerJoin("generations")
                .On("characteristics.GenerationID = generations.Id")
                .InnerJoin("models")
                .On("models.Id = generations.ModelID")
                .InnerJoin("brands")
                .On("models.BrandID = brands.Id")
                .Where($"brands.Brand = '{(string)comboBox1.SelectedValue}' " +
                $"and models.ModelName = '{(string)comboBox2.SelectedValue}' " +
                $"and generations.GenerationName = '{(string)comboBox3.SelectedValue}' " +
                $"and characteristics.EnginePower = {int.Parse((string)comboBox4.SelectedValue)} " +
                $"and characteristics.TransmissionType = '{(string)comboBox5.SelectedValue}'")
                .Execute();
            var inputResult = new SqlRequest()
                .InsertInto("cars")
                .Values(res[0]["Id"], $"{(string)comboBox6.SelectedValue}", numericUpDown3.Value, numericUpDown1.Value, numericUpDown2.Value)
                .Execute();
            Close();*/
        }

        private void AddCarForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "carStoreDbDataSet.Modifications". При необходимости она может быть перемещена или удалена.
            this.modificationsTableAdapter.Fill(this.carStoreDbDataSet.Modifications);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "carStoreDbDataSet.Generations". При необходимости она может быть перемещена или удалена.
            this.generationsTableAdapter.Fill(this.carStoreDbDataSet.Generations);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "carStoreDbDataSet.Models". При необходимости она может быть перемещена или удалена.
            this.modelsTableAdapter.Fill(this.carStoreDbDataSet.Models);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "carStoreDbDataSet.Brands". При необходимости она может быть перемещена или удалена.
            this.brandsTableAdapter.Fill(this.carStoreDbDataSet.Brands);
            comboBox6.DataSource = new string[] { "Новый", "С пробегом" };
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
        }
    }
}
