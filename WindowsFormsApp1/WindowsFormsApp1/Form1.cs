using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void visitDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void clientDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Загрузите данные
            // ... ваш код загрузки ...

            // Потом переименуйте столбцы:
            clientDataGridView.Columns["FCs"].HeaderText = "Фамилия";
            clientDataGridView.Columns["Phone"].HeaderText = "Телефон";
            clientDataGridView.Columns["Имя Клиента"].HeaderText = "Имя клиента";
        }
    }
}
