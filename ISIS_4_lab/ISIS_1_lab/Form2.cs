using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using System.Collections.Specialized;
using Flurl.Http;



namespace ISIS_1_lab
{
    public partial class Form2 : Form
    {
        SqlConnection sqlConnection;
        public void Sqlcon()
        {
            string ConnectionString = @"Data Source = DESKTOP-TP9M2L6\SQLEXPRESS; Initial Catalog = ISISDB; Integrated Security = True";
            sqlConnection = new SqlConnection(ConnectionString);
            sqlConnection.Open();
        }
        public Form2()
        {
            InitializeComponent();
        }
        string MyFName = "";
        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "iSISDBDataSet.Texts". При необходимости она может быть перемещена или удалена.
            this.textsTableAdapter.Fill(this.iSISDBDataSet.Texts);
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnCount = 3;
            //dataGridView1.RowCount = 10;
            dataGridView1.Columns[0].HeaderText = "Длина слова";
            dataGridView1.Columns[1].HeaderText = "Кол-во слов";
            dataGridView1.Columns[2].HeaderText = "Частота встречи, %";
            dataGridView1.Columns[2].DefaultCellStyle.Format = "n2";
            Authorization aut = new Authorization();
            //aut.Sqlcon();
            Sqlcon();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT name FROM [dbo].[Texts]", sqlConnection);
            string str = "";
            try
            {
                sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    str += (Convert.ToString(sqlReader["name"]) + ";");
                }
                string[] names = str.Split(new char[] { ';' });
                foreach (string name in names)
                    comboBox1.Items.Add(name);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            Sqlcon();
            SqlDataReader sqlReader1 = null;
            SqlCommand command1 = new SqlCommand("SELECT name FROM [dbo].[Ini_Files]", sqlConnection);
            string str1 = "";
            try
            {
                sqlReader1 = command1.ExecuteReader();
                while (sqlReader1.Read())
                {
                    str1 += (Convert.ToString(sqlReader1["name"]) + ";");
                }
                string[] names = str1.Split(new char[] { ';' });
                foreach (string name in names)
                    comboBox2.Items.Add(name);
            }
            finally
            {
                if (sqlReader1 != null)
                    sqlReader1.Close();
            }
        }
        public string prov()
        {
            if (richTextBox1.Text == "")
            {
                return null;
            }
            else
            {
                return "";
            }
            
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            string a = prov();
            if (a == null)
            {
                //richTextBox1.SaveFile(MyFName);
                // Form2 aAuthor = new Form2();
                //aAuthor.ShowDialog();
                string message = "Загрузите файл.";
                string caption = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result;

                // Displays the MessageBox.

                result = MessageBox.Show(message, caption, buttons);
            }
            else
            {
                for (int x = 0; x < 10; x++)
                {
                    dataGridView1.Rows.Add();
                }

                // string text = richTextBox1.Text;// "я веселный молочник\n кто сказал, что я один?";
                for (int x = 0, y = 1; x < 10; x++)
                {
                    dataGridView1.Rows[x].Cells[0].Value = y;
                    y++;
                }
                for (int x = 0, y = 1; y <= 10;)
                {
                    var count = GetCountWordsByLength(richTextBox1.Text, y);
                    dataGridView1.Rows[x].Cells[1].Value = count;
                    y++;
                    x++;
                    //count == 0;
                }

                int arrSize = 0;
                double textSize = 0;
                for (int x = 0; x < 10;)
                {
                    if (Convert.ToInt32(dataGridView1.Rows[x].Cells[1].Value) != 0)
                    {
                        arrSize++;
                        textSize += Convert.ToDouble(dataGridView1.Rows[x].Cells[1].Value);
                        x++;
                    }
                    else
                    {
                        x++;
                    }
                }

                for (int x = 0; x < 10; x++)
                {
                    dataGridView1.Rows[x].Cells[2].Value = ((Convert.ToDouble(dataGridView1.Rows[x].Cells[1].Value) * 100) / textSize);
                    // y++;
                }


                int[] arrSourse = new int[arrSize];
                string[] yValues = new string[arrSize];
                for (int x = 0, y = 0; x < 10 && y < arrSize;)
                {
                    if (Convert.ToInt32(dataGridView1.Rows[x].Cells[1].Value) != 0)
                    {
                        arrSourse[y] = Convert.ToInt32(dataGridView1.Rows[x].Cells[1].Value);
                        yValues[y] = Convert.ToString(dataGridView1.Rows[x].Cells[0].Value);//+" - " + Convert.ToString(Convert.ToInt32(dataGridView1.Rows[x].Cells[2].Value))+"%";
                        x++;
                        y++;
                    }
                    else
                    {
                        x++;
                    }
                }
                chart1.Series[0].Points.DataBindXY(yValues, arrSourse);
                //chart1.Series[0].Label = "#PERCENT{P}"; //Отображать значения Y в процентах
                //chart1.Legends["Legend1"].CellColumns.Add(new LegendCellColumn("Name", LegendCellColumnType.Text, "#LEGENDTEXT"));
                //chart1.Legends[Legend1] = "#VALX"; //В легенде отображать значения по X

                button1.Enabled = false;
                /*
                int[] arrNumbers = new int[10];
                for (int x=0; x<10;x++)
                {
                    arrNumbers[x] = Convert.ToInt32(dataGridView1.Rows[x].Cells[1].Value);
                }

                string[] xValues = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
                chart1.Series[0].Points.DataBindXY(xValues, arrNumbers);
                //var count = GetCountWordsByLength(richTextBox1.Text, 3); // количество слов длинной в 3 символа
                //dataGridView1.Rows[0].Cells[1].Value = count;
                //textBox1.AppendText ( Convert.ToString(count));
                */
            }
        }

        public int GetCountWordsByLength(string text, int length)
        {
            char[] delimiters = new char[] { ' ', '\r', '\n', ',', '?', '-', '.', ':' }; // разделители
            var words = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries); // все слова
            return words.Where(x => x.Length == length).ToList().Count; // количество слов по условию 
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Текстовые файлы (*.rtf; *.txt; *.dat) | *.rtf; *.txt; *.dat";
            openFileDialog1.Title = "Выбирите файл";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MyFName = openFileDialog1.FileName;
                try
                {

                    richTextBox1.LoadFile(MyFName);
                }
                catch { richTextBox1.Text = File.ReadAllText(MyFName); }
            }
            
        }
        
        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamReader sr = new StreamReader(MyFName, Encoding.GetEncoding(1251)))
                {

                    String line = sr.ReadToEnd();
                    richTextBox1.Text = line;
                }
            }
            catch { };
        }
        
        private void Button4_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet(); // создаем пока что пустой кэш данных
                DataTable dt = new DataTable(); // создаем пока что пустую таблицу данных
                dt.TableName = "Статистика"; // название таблицы
                dt.Columns.Add("Длина слова"); // название колонок
                dt.Columns.Add("Количество слов");
                dt.Columns.Add("Частота встречи");
                ds.Tables.Add(dt); //в ds создается таблица, с названием и колонками, созданными выше

                foreach (DataGridViewRow r in dataGridView1.Rows) // пока в dataGridView1 есть строки
                {
                    DataRow row = ds.Tables["Статистика"].NewRow(); // создаем новую строку в таблице, занесенной в ds
                    row["Длина слова"] = r.Cells[0].Value;  //в столбец этой строки заносим данные из первого столбца dataGridView1
                    row["Количество слов"] = r.Cells[1].Value; // то же самое со вторыми столбцами
                    row["Частота встречи"] = r.Cells[2].Value; //то же самое с третьими столбцами
                    ds.Tables["Статистика"].Rows.Add(row); //добавление всей этой строки в таблицу ds.
                }
                ds.WriteXml("E:\\Data.xml");
                MessageBox.Show("XML файл успешно сохранен.", "Выполнено.");
            }
            catch
            {
                MessageBox.Show("Невозможно сохранить XML файл.", "Ошибка.");
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveINI = new SaveFileDialog();
            saveINI.Filter = "INI File |*.ini";
            if (saveINI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                IniFiles ini = new IniFiles(saveINI.FileName);
                ini.Write("App", "Value_button1", button1.Text);
                ini.Write("App", "Value_text", richTextBox1.Text);
                ini.Write("App", "Value_button2", button2.Text);
                ini.Write("App", "Value_button1_color", button1.BackColor.ToString());
                ini.Write("App", "Value_button2_color", button2.BackColor.ToString());
            }
        }
        public int zagr()
        {
            int a1 = 0;
            return a1;
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog o1 = new OpenFileDialog();
            o1.Filter = "INI File |*.ini";
            if (o1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string a = ini_read(o1.FileName);
            }
        }
        public string ini_read(string FileName)
        {
                IniFiles ini = new IniFiles(FileName);
                string ini1 = ini.ReadINI("App", "Value_button1");
                string ini2 = ini.ReadINI("App", "Value_text");
                string ini3 = ini.ReadINI("App", "Value_button2");
                string ini4 = ini.ReadINI("App", "Value_button1_color");
                string ini5 = ini.ReadINI("App", "Value_button2_color");
                button1.Text = ini1;
                richTextBox1.Text += ini2;
                button2.Text = ini3;
                Color color2 = Color.FromName(ini5);
                Color color1 = Color.FromName(ini4);
                button1.BackColor = color1;
                button2.BackColor = color2;
            return ini3;
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            int a = ochist();
            if (a != -1)
            {
                MessageBox.Show("Произошла ошибка очистки");
            }
            //button1.Enabled = true;
        }
        public int ochist()
        {
            try
            {
                richTextBox1.Clear();
                dataGridView1.Rows.Clear();
                chart1.Series.Clear();
                return -1;
            }
            catch
            {
                return 0;
            }
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Button8_Click(object sender, EventArgs e)
        {
            Sqlcon();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT text FROM [dbo].[Texts] WHERE name = @name", sqlConnection);
            command.Parameters.AddWithValue("name", comboBox1.Text);
            string str = "";
            try
            {
                sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    str = (Convert.ToString(sqlReader["text"]));
                    
                }
                if (str != "")
                {
                    richTextBox1.Text = str;
                }
                //else
                //{
                  //  richTextBox1.Text = "123";
                //}
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
            
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                label2.Visible = false;
                Sqlcon();
                SqlDataReader sqlReader = null;
                SqlCommand command = new SqlCommand("SELECT COUNT(name) FROM [dbo].[Ini_Files]", sqlConnection);
                string strid = "";
                int intid;
                try
                {
                    sqlReader = command.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        strid = (Convert.ToString(sqlReader[""]));

                    }
                    intid = Convert.ToInt32(strid);
                }
                finally
                {
                    if (sqlReader != null)
                        sqlReader.Close();
                }
                Sqlcon();
                SqlCommand command1 = new SqlCommand("INSERT INTO [dbo].[Ini_Files] VALUES(@id, @name, @value_text, @value_button1, @value_button2, @value_button1_color, @value_button2_color)", sqlConnection);
                command1.Parameters.AddWithValue("id", intid);
                command1.Parameters.AddWithValue("name", textBox1.Text);
                command1.Parameters.AddWithValue("value_text", richTextBox1.Text);
                command1.Parameters.AddWithValue("value_button1", button1.Text);
                command1.Parameters.AddWithValue("value_button2", button2.Text);
                command1.Parameters.AddWithValue("value_button1_color", button1.BackColor.ToString());
                command1.Parameters.AddWithValue("value_button2_color", button2.BackColor.ToString());
                command1.ExecuteNonQuery();
            }
            else
            {
                label2.Visible = true;
            }
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            Sqlcon();
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [dbo].[Ini_Files] WHERE name = @name", sqlConnection);
            command.Parameters.AddWithValue("name", comboBox2.Text);
            string str = "";
            try
            {
                sqlReader = command.ExecuteReader();
                while (sqlReader.Read())
                {
                    richTextBox1.Text = (Convert.ToString(sqlReader["value_text"]));
                    button1.Text = (Convert.ToString(sqlReader["value_button1"]));
                    button2.Text = (Convert.ToString(sqlReader["value_button2"]));
                    Color color1 = Color.FromName(Convert.ToString(sqlReader["value_button1_color"]));
                    Color color2 = Color.FromName(Convert.ToString(sqlReader["value_button2_color"]));
                    button1.BackColor = color1;
                    button2.BackColor = color2;
                }
                if (str != "")
                {
                    richTextBox1.Text = str;
                }
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            label3.Text = "";
            Settings settings = new Settings();
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync("http://localhost:2223/api/ini");
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    
                    var result = response.Result.Content.ReadAsStringAsync();
                    label3.Text = result.Result;
                }
            }
            //RunAsync().Wait();
        }

        static async Task<Settings> RunAsync(string a)
        {
            using (HttpClient client = new HttpClient())
            {
                Settings settings = null;
                HttpResponseMessage response = await client.GetAsync("http://localhost:2223/api/ini");
                if (response.IsSuccessStatusCode)
                {
                    settings = await response.Content.ReadAsAsync<Settings>();
                }

               
                return settings;
            }
            
        }

        private void button12_Click(object sender, EventArgs e)
        {
            label3.Text = "";
            Settings settings = new Settings();
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync("http://localhost:2223/api/ini/2");
                if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = response.Result.Content.ReadAsStringAsync();
                    IList<Settings> settingsList = (IList<Settings>)Newtonsoft.Json.JsonConvert.DeserializeObject(result.Result, typeof(IList<Settings>));

                    label3.Text = settingsList.ToString();
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                label5.Visible = false;
                label3.Text = "";
                using (HttpClient client = new HttpClient())
                {
                    var response = client.GetAsync("http://localhost:2223/api/ini" + "/" + textBox2.Text);
                        var result = response.Result.Content.ReadAsStringAsync();
                        Settings settingsList = JsonConvert.DeserializeObject<Settings>(result.Result);
                    if (settingsList.Value_Button1 == null && settingsList.Value_Button2 == null &&
                    settingsList.Value_Button1_Color == null && settingsList.Value_Button2_Color == null &&
                    settingsList.Value_Text == null)
                    {
                        goto M;
                    }
                        richTextBox1.Text = settingsList.Value_Text;
                        button1.Text = settingsList.Value_Button1;
                        button2.Text = settingsList.Value_Button2;
                        Color color1 = Color.FromName(settingsList.Value_Button1_Color);
                        Color color2 = Color.FromName(settingsList.Value_Button2_Color);
                        button1.BackColor = color1;
                        button2.BackColor = color2;
                        label3.Text = "Вы загрузили " + settingsList.Name;
                M: int q = 0;
                }
            }
            catch
            {
                label5.Text = "";
            }
        }
        static async Task RunAsync1()
        {
            var responseString = await "http://localhost:2223/api/ini"
                .PostJsonAsync(new {Name = "ghjf", Value_Text = "qwe", Value_Button1 = "asd", Value_Button2 = "fgh", Value_Button1_Color = "Color[Control]", Value_Button2_Color = "Color[Control]" })
                .ReceiveJson<Settings>();
            
        }

        private void button14_Click(object sender, EventArgs e)
        {
            label6.Visible = false;
            label3.Text = "";
            if (textBox3.Text == "")
            {
                label6.Visible = true;
            }
            else
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://localhost:2223/api/ini");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"Name\":\"" + textBox3.Text + "\", \"Value_Text\":\"" + richTextBox1.Text + "\",\"Value_Button1\":\"" + button1.Text + "\",\"Value_Button2\":\"" + button2.Text + "\",\"Value_Button1_Color\":\"" + button1.BackColor.ToString() + "\",\"Value_Button2_Color\":\"" + button2.BackColor.ToString() + "\"}";
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    label3.Text = result.ToString();
                }
            }
        }
    }
}
