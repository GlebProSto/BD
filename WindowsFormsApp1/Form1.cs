using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private string dataFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.xml");

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += Form1_FormClosing;

            try
            {
                if (this.clientBindingNavigatorSaveItem != null)
                    this.clientBindingNavigatorSaveItem.Click += clientBindingNavigatorSaveItem_Click;
            }
            catch { }

            // При запуске делаем все кнопки редактирования/удаления серыми
            MakeGray(button2);
            MakeGray(button3);
            MakeGray(button4);
            MakeGray(button5);
            MakeGray(button6);
        }

        // Метод делает кнопку серой и неактивной
        private void MakeGray(Button btn)
        {
            btn.Enabled = false;
            btn.BackColor = Color.LightGray;
            btn.ForeColor = Color.Gray;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.No;
        }

        // Метод делает кнопку белой и активной
        private void MakeWhite(Button btn)
        {
            btn.Enabled = true;
            btn.BackColor = Color.White;
            btn.ForeColor = Color.Black;
            btn.FlatStyle = FlatStyle.Standard;
            btn.Cursor = Cursors.Default;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3(dataSet1, true);
            f3.ShowDialog();
            clientBindingSource.ResetBindings(false);

            if (clientBindingSource.Count > 0)
            {
                MakeWhite(button2);
                MakeWhite(button3);
                MakeWhite(button4);
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (clientBindingSource.Current != null)
            {
                if (MessageBox.Show("Удалить клиента?", "Вопрос", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    clientBindingSource.RemoveCurrent();

                    if (clientBindingSource.Count == 0)
                    {
                        MakeGray(button2);
                        MakeGray(button3);
                        MakeGray(button4);
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int selectedClientId = 0;
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
                MakeWhite(button5);
                MakeWhite(button6);
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
                        MakeGray(button5);
                        MakeGray(button6);
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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

            try
            {
                // Настройка таблицы клиентов
                if (clientDataGridView.Columns.Contains("FCs"))
                {
                    clientDataGridView.Columns["FCs"].HeaderText = "ФИО";
                    clientDataGridView.Columns["FCs"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                if (clientDataGridView.Columns.Contains("Phone"))
                {
                    clientDataGridView.Columns["Phone"].HeaderText = "Телефон";
                    clientDataGridView.Columns["Phone"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                clientDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                clientDataGridView.ContextMenuStrip = null;

                // Настройка таблицы посещений
                if (visitDataGridView.Columns.Contains("DateTimeIn"))
                {
                    visitDataGridView.Columns["DateTimeIn"].HeaderText = "Дата Входа";
                    visitDataGridView.Columns["DateTimeIn"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                if (visitDataGridView.Columns.Contains("DateTimeOff"))
                {
                    visitDataGridView.Columns["DateTimeOff"].HeaderText = "Дата выхода";
                    visitDataGridView.Columns["DateTimeOff"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                if (visitDataGridView.Columns.Contains("ZoneVisit"))
                {
                    visitDataGridView.Columns["ZoneVisit"].HeaderText = "Зона посещения";
                    visitDataGridView.Columns["ZoneVisit"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
                visitDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                visitDataGridView.ContextMenuStrip = null;
            }
            catch { }

            // === ВАЖНО: В КОНЦЕ Form1_Load принудительно делаем ВСЕ кнопки серыми ===
            MakeGray(button2);
            MakeGray(button3);
            MakeGray(button4);
            MakeGray(button5);
            MakeGray(button6);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
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
    }
}