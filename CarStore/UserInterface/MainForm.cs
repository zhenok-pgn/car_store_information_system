using CarStore.Domain;
using CarStore.UserInterface;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CarStore
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            PrepairTable();
            LoadCars();
        }

        private void PrepairTable()
        {
            var properties =typeof(Car).GetProperties();
            dataGridView1.ColumnCount = properties.Length;
            for(int i = 0; i < properties.Length; i++)
            {
                var attribute = properties[i].GetCustomAttributes(true).OfType<LabelAttribute>().First();
                dataGridView1.Columns[i].Name = attribute.LabelText;
            }
        }

        private void LoadCars(object sender = null, EventArgs e = null)
        {
            dataGridView1.Rows.Clear();
            var list = CarsRepository.SelectAll();
            var properties = Car.GetProperties();
            foreach(var element in list)
            {
                var data = new string[properties.Count];
                for(int i = 0; i < properties.Count; i++)
                {
                    data[i] = properties[i].GetValue(element).ToString();
                }
                dataGridView1.Rows.Add(data);
            }
        }

        private void LoadClients(object sender, EventArgs e)
        {

        }

        private void AddCarButtonClick(object sender, EventArgs e)
        {
            var addForm = new AddCarForm();
            addForm.ShowDialog();
            LoadCars();
        }

        private void AddClientButtonClick(object sender, EventArgs e)
        {

        }
    }
}
