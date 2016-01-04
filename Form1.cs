using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SafeToXML
{
    public partial class Form1 : Form
    {

        //Директория выполнения приложения
        private string sCurDir = string.Empty;
        //Класс XML документа
        private XmlDocument xmldoc = null;
        //Классы для работв с XML документом как с объектом базы данных
        DataTable MyDatatable = null;
        DataSet MyDataSet = null;
        public Form1()
        {
            InitializeComponent();


            sCurDir = Directory.GetCurrentDirectory();
            using (StreamReader streamreader = new StreamReader(sCurDir + @"\my.xml",
                           System.Text.Encoding.UTF8))
            {
                MyDataSet = new DataSet();
                MyDataSet.ReadXml(streamreader, XmlReadMode.Auto);
                MyDatatable = MyDataSet.Tables[0];
            }
            dataGridView1.DataSource = MyDataSet.Tables[0];
            dataGridView1.Columns[0].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "") return;
            if (textBox2.Text == "") return;
            if (textBox3.Text == "") return;
            if (textBox4.Text == "") return;

            DataRow[] datarows = null;
            //Ищем максимальное ID в DataSet (в Datatable)
            string s = string.Empty;
            try
            {
                datarows = MyDatatable.Select("id=max(id)");
                s = datarows[0]["id"].ToString();
            }
            catch (Exception)
            {

            }
            if (s == "" || s == string.Empty)
            {
                s = "0";
            }
            int i = int.Parse(s) + 1;

            //Создаем новую строку для MyDataSet 
            DataRow datarow = MyDataSet.Tables[0].NewRow();
            datarow[0] = Convert.ToString(i);
            datarow[1] = textBox1.Text.Trim();
            datarow[2] = textBox2.Text.Trim();
            datarow[3] = textBox3.Text.Trim();
            datarow[4] = textBox4.Text.Trim();

            MyDataSet.Tables[0].Rows.Add(datarow);

            if (i == 1)     //для удаления первых строк, если был загружен пустой XML
            {
                MyDataSet.Tables[0].DefaultView.AllowDelete = true;
                MyDataSet.Tables[0].DefaultView.Delete(0);
            }
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

            MyDataSet.WriteXml(sCurDir + @"\my.xml", XmlWriteMode.WriteSchema);
            MyDataSet = new DataSet();
            //Вновь загружаем сохраненные данные
            MyDataSet.ReadXml(sCurDir + @"\my.xml", XmlReadMode.Auto);
            MyDatatable = MyDataSet.Tables[0];

            dataGridView1.DataSource = MyDataSet.Tables[0];
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataRow current = ((DataRowView)dataGridView1.CurrentRow.DataBoundItem).Row;
            current.Delete();

            MyDataSet.WriteXml(sCurDir + @"\my.xml", XmlWriteMode.WriteSchema);
            MyDataSet = new DataSet();
            //Вновь загружаем сохраненные данные
            MyDataSet.ReadXml(sCurDir + @"\my.xml", XmlReadMode.Auto);
            MyDatatable = MyDataSet.Tables[0];

            dataGridView1.DataSource = MyDataSet.Tables[0];
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
