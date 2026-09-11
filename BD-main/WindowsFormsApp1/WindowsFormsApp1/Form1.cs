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
            // коммент
            InitializeComponent();
            button2.Enabled = false;
            button3.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;
        }

        private void visitDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3(dataSet1, true);
            f3.ShowDialog();
            clientBindingSource.ResetBindings(false);

            if (clientBindingSource.Count > 0)
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }

            // После закрытия формы проверяем - есть ли клиенты
            if (clientBindingSource.Count > 0)
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void clientDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (clientBindingSource.Current != null)
            {
                int editId = Convert.ToInt32(clientDataGridView.CurrentRow.Cells[0].Value);
                Form3 f3 = new Form3(dataSet1, false, editId);
                f3.ShowDialog();
                clientBindingSource.ResetBindings(false);
            }
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (clientBindingSource.Current != null)
            {
                if (MessageBox.Show("Удалить клиента?", "Вопрос", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    clientBindingSource.RemoveCurrent();

                    // Если клиентов не осталось - делаем кнопки серыми
                    if (clientBindingSource.Count == 0)
                    {
                        button2.Enabled = false;
                        button3.Enabled = false;
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2(dataSet1, true);
            f2.ShowDialog();
            visitBindingSource.ResetBindings(false);

            if (visitBindingSource.Count > 0)
            {
                button5.Enabled = true;
                button6.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (visitBindingSource.Current != null)
            {
                int editId = Convert.ToInt32(visitDataGridView.CurrentRow.Cells[0].Value);
                Form2 f2 = new Form2(dataSet1, false, editId);
                f2.ShowDialog();
                visitBindingSource.ResetBindings(false);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (visitBindingSource.Current != null)
            {
                if (MessageBox.Show("Удалить посещение?", "Вопрос", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    visitBindingSource.RemoveCurrent();

                    if (visitBindingSource.Count == 0)
                    {
                        button5.Enabled = false;
                        button6.Enabled = false;
                    }
                }
            }
        }
    }
}
