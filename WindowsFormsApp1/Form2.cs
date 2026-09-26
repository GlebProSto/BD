using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        private DataSet1 dataSet1;
        private bool isNewVisit;
        private int currentVisitId;
        private int currentClientId;

        public Form2(DataSet1 ds, bool isNew, int id = 0)
        {
            InitializeComponent();
            this.dataSet1 = ds;
            this.isNewVisit = isNew;
            this.currentVisitId = id;
            this.StartPosition = FormStartPosition.CenterParent;

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd.MM.yyyy HH:mm";
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd.MM.yyyy HH:mm";
            dateTimePicker2.ShowUpDown = true;

            if (isNew && id > 0)
            {
                this.currentClientId = id;
            }

            if (!isNewVisit)
            {
                this.Text = "Редактирование посещения";
                DataRow[] rows = dataSet1.Visit.Select($"ID_V = {currentVisitId}");
                if (rows.Length > 0)
                {
                    DataRow row = rows[0];
                    dateTimePicker1.Value = Convert.ToDateTime(row["DateTimeIn"]);
                    dateTimePicker2.Value = Convert.ToDateTime(row["DateTimeOff"]);

                    // Устанавливаем выбранную зону в ComboBox
                    string zone = row["ZoneVisit"].ToString();
                    int index = comboBox1.Items.IndexOf(zone);
                    if (index >= 0)
                        comboBox1.SelectedIndex = index;
                    else
                        comboBox1.SelectedIndex = 0;

                    // Получаем и показываем ФИО клиента в заголовке
                    if (row["FK_C"] != DBNull.Value)
                    {
                        int clientId = Convert.ToInt32(row["FK_C"]);
                        this.currentClientId = clientId;

                        DataRow[] clientRows = dataSet1.Client.Select($"ID_C = {clientId}");
                        if (clientRows.Length > 0)
                        {
                            string clientFio = clientRows[0]["FCs"].ToString();
                            this.Text = $"Посещение — {clientFio}";
                        }
                    }
                }
            }
            else
            {
                this.Text = "Добавление посещения";
                comboBox1.SelectedIndex = 0; // По умолчанию первый пункт

                if (currentClientId > 0)
                {
                    DataRow[] clientRows = dataSet1.Client.Select($"ID_C = {currentClientId}");
                    if (clientRows.Length > 0)
                    {
                        string clientFio = clientRows[0]["FCs"].ToString();
                        this.Text = $"Добавление посещения — {clientFio}";
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (currentClientId <= 0)
            {
                MessageBox.Show("Нельзя добавить посещение без выбора клиента.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (isNewVisit)
            {
                DataRow newRow = dataSet1.Visit.NewRow();
                newRow["DateTimeIn"] = dateTimePicker1.Value;
                newRow["DateTimeOff"] = dateTimePicker2.Value;
                newRow["ZoneVisit"] = comboBox1.Text; // Берём из ComboBox
                newRow["FK_C"] = currentClientId;

                dataSet1.Visit.Rows.Add(newRow);
                dataSet1.Visit.AcceptChanges();
            }
            else
            {
                DataRow[] rows = dataSet1.Visit.Select($"ID_V = {currentVisitId}");
                if (rows.Length > 0)
                {
                    rows[0]["DateTimeIn"] = dateTimePicker1.Value;
                    rows[0]["DateTimeOff"] = dateTimePicker2.Value;
                    rows[0]["ZoneVisit"] = comboBox1.Text; // Берём из ComboBox
                    rows[0]["FK_C"] = currentClientId;
                    dataSet1.Visit.AcceptChanges();
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}