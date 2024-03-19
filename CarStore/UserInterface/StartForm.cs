using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace CarStore.UserInterface
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "carStoreDbDataSet.Cars". При необходимости она может быть перемещена или удалена.
            //this.carsTableAdapter.Fill(this.carStoreDbDataSet.Cars);
            TableLoad();
        }

        private void TableLoad()
        {
            string connectionString = Properties.Settings.Default.CarStoreDbConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                var sqlDataAdapter = new SqlDataAdapter("select * from models", connection);
                var bindingSource = new BindingSource();
                var dataTable = new DataTable();

                connection.Open();
                sqlDataAdapter.Fill(dataTable);
                bindingSource.DataSource = dataTable;
                dataGridView1.DataSource = bindingSource;
                bindingNavigator1.BindingSource = bindingSource;
                connection.Close();
            }
        }
    }
}
