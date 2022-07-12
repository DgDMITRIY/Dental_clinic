using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WinFormsApp1
{
    public partial class Main_menu : Form
    {     
        public Main_menu()
        {
            InitializeComponent();
        }
        private void Main_menu_Load(object sender, EventArgs e) => Text = "Стоматологическая поликлинника";
        private void button3_Click(object sender, EventArgs e)//Класс (форма) врачей
        {
            Doctors f = new Doctors(); // создаем обьект данного класса (формы)
            f.Show();// показываем класс (форму) "Врачи"
            f.Left = this.Left;// задаём открываемой форме позицию слева равную позиции текущей формы
            f.Top = this.Top;// задаём открываемой форме позицию сверху равную позиции текущей формы
            this.Hide(); //скрываем форму (this - текущая форма)
        }
        private void button4_Click(object sender, EventArgs e)//Класс (форма) процедур
        {
            Procedures f = new Procedures();
            f.Show();
            f.Left = this.Left;
            f.Top = this.Top;
            this.Hide();
        }
        private void button1_Click(object sender, EventArgs e)//Класс (форма) карточки больного
        {
            Patient_card f = new Patient_card(); 
            f.Show(); 
            f.Left = this.Left;
            f.Top = this.Top;
            this.Hide();
        }
        private void button2_Click(object sender, EventArgs e) //Класс (форма) лекарств
        {
            Medicines f = new Medicines();
            f.Show(); 
            f.Left = this.Left;
            f.Top = this.Top;
            this.Hide();
        }
    }
}
