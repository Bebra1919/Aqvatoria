using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aqvatoria
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ShamriloAS;Integrated Security=True");

        public MainWindow()
        {
            InitializeComponent();
            conn.Open();
            SqlCommand cmd = new SqlCommand("select Id from Orders;",conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                OrderList.Items.Add("#" + reader[0].ToString());
            }
            reader.Close();
        }

        private void OrderList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ordersId = OrderList.SelectedItem.ToString().Substring(1);
            ChecBlock.Text = "";
            ChecBlock.Text += GetInfoOrders(ordersId);
            ChecBlock.Text += GetInfoServices(ordersId);
        }
        private string GetInfoOrders(string idOrders)
        {
            string line = "";
            
            SqlCommand cmd = new SqlCommand("select " + "Orders.Id, " +
        "Orders.DataOrder, " +
        "Orders.NumberBox, " +
        "Users.FIO " +
        "from Orders " +
        "join Users ON Orders.ISWorker = Users.Id " +
        $"where Orders.Id= {idOrders};", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ChecBlock.Text = reader[0].ToString() + "\n" + reader[1].ToString() + "\n" + reader[2].ToString() + "\n" + reader[3].ToString();
            }
            reader.Close();
            return line;
        }
        
        private string GetInfoServices(string IDServices)
        {
            string line1 = "";

            SqlCommand cmd = new SqlCommand("select" +
"OrderServices.IDOrder " +
"sum(Services.Price) " +
"from OrderServices " +
"join Services On Services.Id = OrderServices.IDServices " +
"Group by OrderServices.IDOrder = {IDServices};", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                ChecBlock.Text = reader[0].ToString() + "\n" + reader[1].ToString() + "\n" + reader[2].ToString() + "\n" + reader[3].ToString();
            }
            reader.Close();
            return line1;
        }
    }
}
