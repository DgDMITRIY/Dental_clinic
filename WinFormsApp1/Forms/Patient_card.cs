using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using WinFormsApp1.Classes;

namespace WinFormsApp1
{
    public partial class Patient_card : Form
    {
        private Sqliteclass mydb = null;
        private string sPath = @"C:\Users\user\source\repos\WinFormsApp1\WinFormsApp1\mybd.db"; // изменяйте путь к переменной, если путь к базе данных отличается (сама база данных находится в папке WinFormsApp1 и называется mybd )
        private string sSql = string.Empty;

        SQLiteDataAdapter dataadapter = null;
        DataSet dataset = new DataSet();
        SQLiteConnection con = new SQLiteConnection();

        Patient patient = new Patient();
        Classes.Procedures proceduress = new Classes.Procedures();
        Classes.Doctors doctor = new Classes.Doctors();
        Classes.Medicines medicines = new Classes.Medicines();

        public Patient_card()
        {
            InitializeComponent();
        }
        private void Patient_card_Load(object sender, EventArgs e)
        {
            Text = "Карточка пациента";
            //Заполнение таблицы
            mydb = new Sqliteclass();
            sSql = "SELECT fio,male,birth_date,receipt,doctor FROM card";
            con.ConnectionString = @"Data Source=" + sPath + ";Version=3;New=False;";
            dataadapter = new SQLiteDataAdapter(sSql, con);
            dataadapter.Fill(dataset); //Заполняем dataset
            dataGridView1.DataSource = dataset.Tables[0];//привязываем dataGridView к DataSet
            DataRow[] datarows = mydb.drExecute(sPath, sSql);
            if (datarows == null)
            {
                Text = "Ошибка чтения!";
                mydb = null;
                return;
            }
            int i = -1;
            foreach (DataRow dr in datarows)
            {
                i++;
                dataGridView1.ClearSelection(); //Отменяет выделение ячеек, выбранных в данный момент.
                dataGridView1.Rows[i].Cells[0].Value = dr["fio"].ToString().Trim(); // заполнение данными из бд в datagriedview (Функция Trim без параметров обрезает начальные и конечные пробелы и возвращает обрезанную строку)
                dataGridView1.Rows[i].Cells[1].Value = dr["male"].ToString().Trim();
                dataGridView1.Rows[i].Cells[2].Value = dr["birth_date"].ToString();
                dataGridView1.Rows[i].Cells[3].Value = dr["receipt"].ToString().Trim();
                dataGridView1.Rows[i].Cells[4].Value = dr["doctor"].ToString().Trim();
            }
            //Инициализация формы добавления(доктор)
            mydb = new Sqliteclass();
            sSql = "SELECT * FROM doctor";
            DataRow[] datarows2 = mydb.drExecute(sPath, sSql);
            if (datarows2 == null)
            {
                Text = "Ошибка чтения!";
                mydb = null;
                return;
            }
            foreach (DataRow dr in datarows2)
            {
                comboBox1.Items.Add(dr["name"].ToString());
            }
            //Инициализация формы добавления(процедуры)
            mydb = new Sqliteclass();
            sSql = "SELECT title FROM proc";
            DataRow[] datarows3 = mydb.drExecute(sPath, sSql);
            if (datarows3 == null)
            {
                Text = "Ошибка чтения!";
                mydb = null;
                return;
            }
            foreach (DataRow dr in datarows3)
            {
                comboBox2.Items.Add(dr["title"].ToString());
            }
            //Инициализация формы добавления(лекарства)
            mydb = new Sqliteclass();
            sSql = "SELECT * FROM medicine";
            DataRow[] datarows4 = mydb.drExecute(sPath, sSql);
            if (datarows4 == null)
            {
                Text = "Ошибка чтения!";
                mydb = null;
                return;
            }
            foreach (DataRow dr in datarows4)
            {
                listBox1.Items.Add(dr["name"].ToString());
            }
        }
        private void Patient_card_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form ifrm = Application.OpenForms[0];// вызываем главную форму, которая открыла текущую, главная форма всегда = 0 ([0])
            ifrm.StartPosition = FormStartPosition.Manual;// меняем параметр StartPosition у формы Main_menu, иначе она будет использовать тот, который у неё прописан в настройках и всегда будет открываться по центру экрана
            ifrm.Left = this.Left; // задаём открываемой форме позицию слева равную позиции текущей формы
            ifrm.Top = this.Top; // задаём открываемой форме позицию сверху равную позиции текущей формы
            ifrm.Show();// отображаем форму Main_menu
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            patient.Name = Convert.ToString(textBox1.Text).Trim();
            if (radioButton1.Checked) { patient.Male = "Мужской"; } else if (radioButton2.Checked) { patient.Male = "Женский"; }
            patient.Birth_date = Convert.ToString(this.dateTimePicker1.Text);
            patient.Receipt = Convert.ToString(this.dateTimePicker2.Text);
            doctor.Name = Convert.ToString(comboBox1.Text).Trim();
            for (int k = 0; k < listBox1.SelectedItems.Count; k++)
            {
                medicines.MedicinesName = $"{medicines.MedicinesName}{listBox1.SelectedItems[k].ToString()}\n";
            }
            patient.Disease = Convert.ToString(richTextBox1.Text).Trim();
            proceduress.Title = Convert.ToString(comboBox2.Text).Trim();
            if (patient.Name == "") { MessageBox.Show("Заполните поле Ф.И.О.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (patient.Male == "" || patient.Male == null) { MessageBox.Show("Выберите пол", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (doctor.Name == "") { MessageBox.Show("Выберите лечащего врача", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            if (patient.Disease == "") { MessageBox.Show("Опишите заболевание", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            mydb = new Sqliteclass();
            sSql = @"INSERT INTO card (fio,male,birth_date,receipt,medicine,doctor,disease,proc) VALUES ('" + patient.Name + "','" + patient.Male + "','" + patient.Birth_date + "','" + patient.Receipt + "','" + medicines.MedicinesName + "','" + doctor.Name + "','" + patient.Disease + "','" + proceduress.Title + "');";
            //Проверка работы
            if (mydb.iExecuteNonQuery(sPath, sSql,1) == 0) //Третий параметр функции говорит, что БД создается вновь
            {
                Text = "Ошибка записи!";
                mydb = null;
                return;
            }
            else
            {
                textBox1.Text = "";
                richTextBox1.Text = "";
                comboBox1.Text = "";
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                comboBox2.Text = "";
                listBox1.ClearSelected();
                int i = dataGridView1.RowCount - 1 ;
                //Добавляем в таблицу новую запись при успешном добавлении в базу
                Text = "Запись пациента №" + (dataGridView1.RowCount) + " добавлена!";
                dataGridView1.Rows[i].Cells[0].Value = patient.Name.ToString().Trim();
                dataGridView1.Rows[i].Cells[1].Value = patient.Male.ToString().Trim();
                dataGridView1.Rows[i].Cells[2].Value = patient.Birth_date.ToString();
                dataGridView1.Rows[i].Cells[3].Value = patient.Receipt.ToString().Trim();
                dataGridView1.Rows[i].Cells[4].Value = doctor.Name.ToString().Trim();
            }
            dataGridView1.ClearSelection();
            dataset.Reset(); 
            dataadapter.Fill(dataset);
            dataGridView1.DataSource = dataset.Tables[0];
            mydb = null;
        }
        private void button2_Click(object sender, EventArgs e)//Удалить пациента
        {
            try
            {
                string it = (string)dataGridView1.CurrentRow.Cells[0].Value;
                mydb = new Sqliteclass();
                sSql = "DELETE FROM card WHERE fio = '" + it + "'";
                if (mydb.iExecuteNonQuery(sPath, sSql,1) == 0)
                {
                    Text = "Ошибка удаления записи!";
                    mydb = null;
                    return;
                }
                mydb = null;
                Text = "Запись удалена из БД!";
                int ind = dataGridView1.SelectedCells[0].RowIndex;
                dataGridView1.Rows.RemoveAt(ind); 
            }
            catch
            {
                MessageBox.Show("Вы не выбрали строку для удаления", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            textBox2.Text = "";
            label14.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            richTextBox3.Text = "";
            richTextBox4.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string FullName = (string)dataGridView1.CurrentRow.Cells[0].Value;
            mydb = new Sqliteclass();
            sSql = "SELECT * FROM card WHERE fio LIKE '%" + FullName + "%'"; //С помощью команды "LIKE" можно искать подстроку в столбце. Чтобы сделать это поставьте знак процента "%" с той стороны подстроки, с которой могут находиться другие символы. 
            DataRow[] datarows = mydb.drExecute(sPath, sSql);
            if (datarows == null)
            {
                Text = "Ошибка чтения!";
                mydb = null;
                return;
            }
            foreach (DataRow dr in datarows)
            {
                textBox2.Text = dr["fio"].ToString();
                label14.Text = dr["male"].ToString();
                textBox3.Text = dr["birth_date"].ToString();
                textBox4.Text = dr["receipt"].ToString();
                textBox5.Text = dr["doctor"].ToString();
                richTextBox3.Text = dr["disease"].ToString();
                richTextBox4.Text = dr["medicine"].ToString();
                textBox6.Text = dr["proc"].ToString();
            }
        }
    }
}
