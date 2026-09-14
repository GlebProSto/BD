using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string dataFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.xml");
        public Form1()
        {
            // коммент
            // gffsb
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;
            try
            {
                if (this.clientBindingNavigatorSaveItem != null)
                    this.clientBindingNavigatorSaveItem.Click += clientBindingNavigatorSaveItem_Click;
            }
            catch { }
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;

            try
            {
                this.clientBindingSource.CurrentChanged += ClientBindingSource_CurrentChanged;
            }
            catch { }
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
                button4.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (clientBindingSource.Current != null)
            {
                int editId = 0;
                try
                {
                    var drv = clientBindingSource.Current as DataRowView;
                    if (drv != null && drv.Row.Table.Columns.Contains("ID_C"))
                    {
                        var v = drv["ID_C"];
                        if (v != null && v != DBNull.Value)
                            int.TryParse(v.ToString(), out editId);
                    }
                }
                catch { editId = 0; }

                if (editId > 0)
                {
                    Form3 f3 = new Form3(dataSet1, false, editId);
                    f3.ShowDialog();
                    clientBindingSource.ResetBindings(false);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (visitBindingSource.Current != null)
            {
                int editId = 0;
                try
                {
                    var drv = visitBindingSource.Current as DataRowView;
                    if (drv != null && drv.Row.Table.Columns.Contains("ID_V"))
                    {
                        var v = drv["ID_V"];
                        if (v != null && v != DBNull.Value)
                            int.TryParse(v.ToString(), out editId);
                    }
                }
                catch { editId = 0; }

                if (editId > 0)
                {
                    Form2 f2 = new Form2(dataSet1, false, editId);
                    f2.ShowDialog();
                    visitBindingSource.ResetBindings(false);
                }
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
        private void Form1_Load(object sender, EventArgs e)
        {
            // Попытка загрузить данные и схему из XML
            try
            {
                if (System.IO.File.Exists(dataFilePath))
                {
                    dataSet1.ReadXml(dataFilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }

            // Переименовать столбцы при наличии
            try
            {
                if (clientDataGridView.Columns.Contains("FCs"))
                    clientDataGridView.Columns["FCs"].HeaderText = "ФИО";
                if (clientDataGridView.Columns.Contains("Phone"))
                    clientDataGridView.Columns["Phone"].HeaderText = "Телефон";
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Сохраняем схему и данные в XML
            try
            {
                DataStore.Save(dataSet1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении данных: " + ex.Message);
            }
        }

        private void clientBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            try
            {
                DataStore.Save(dataSet1);
                MessageBox.Show("Данные сохранены.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
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
                        button4.Enabled = false;
                    }
                }
            }
        }

        private void ClientBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            try
            {
                bool hasClients = clientBindingSource.Count > 0;
                button2.Enabled = hasClients;
                button3.Enabled = hasClients;
                // Добавление посещения доступно только если выбран текущий клиент
                button4.Enabled = hasClients && clientBindingSource.Current != null;
            }
            catch { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int selectedClientId = 0;
            // Получаем ID клиента из текущего элемента clientBindingSource (без опоры на видимые столбцы DataGridView)
            if (clientBindingSource.Current != null)
            {
                try
                {
                    var drv = clientBindingSource.Current as DataRowView;
                    if (drv != null && drv.Row.Table.Columns.Contains("ID_C"))
                    {
                        object idVal = drv["ID_C"];
                        if (idVal != null && idVal != DBNull.Value)
                            int.TryParse(idVal.ToString(), out selectedClientId);
                    }
                }
                catch { selectedClientId = 0; }
            }

            Form2 f2 = new Form2(dataSet1, true, selectedClientId);
            f2.ShowDialog();
            visitBindingSource.ResetBindings(false);

            if (visitBindingSource.Count > 0)
            {
                button5.Enabled = true;
                button6.Enabled = true;
            }
        }
    }
}
