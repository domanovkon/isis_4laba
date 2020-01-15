using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ISIS_1_lab
{
    public partial class Authorization : Form
    {
        public Authorization()
        {
            InitializeComponent();
        }
        SqlConnection sqlConnection;
        public void Sqlcon()
        {
            string ConnectionString = @"Data Source = DESKTOP-TP9M2L6\SQLEXPRESS; Initial Catalog = ISISDB; Integrated Security = True";
            sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Sqlcon();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Users] WHERE username = @username and password = @password", sqlConnection);
            command.Parameters.AddWithValue("username", textBox1.Text);
            command.Parameters.AddWithValue("password", textBox2.Text);
            string str = "";
            try
            {
                sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    str += (Convert.ToString(sqlReader["id"]) + " " + Convert.ToString(sqlReader["username"]) + " "
                        + Convert.ToString(sqlReader["password"]) + ";");
                }
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            if (str != "")
            {
                Authorization aut = new Authorization();
                Form1 fm1 = new Form1();
                this.Hide();
                fm1.Show();
            }
            else
            {
                label1.Visible = true;
            }
        }
    }
}
