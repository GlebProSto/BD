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

        // Конструктор принимает DataSet, флаг (новая запись или редактирование) и ID записи
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

            // При добавлении нового посещения параметр id может содержать ID клиента
            if (isNew && id > 0)
            {
                this.currentClientId = id;
            }

            if (!isNewVisit)
            {
                // Если это РЕДАКТИРОВАНИЕ, загружаем данные в поля
                this.Text = "Редактирование посещения";
                DataRow[] rows = dataSet1.Visit.Select($"ID_V = {currentVisitId}");
                if (rows.Length > 0)
                {
                    DataRow row = rows[0];
                    dateTimePicker1.Value = Convert.ToDateTime(row["DateTimeIn"]);
                    dateTimePicker2.Value = Convert.ToDateTime(row["DateTimeOff"]);
                    textBox1.Text = row["ZoneVisit"].ToString(); // Используем textBox1 для Зоны
                }
            }
            else
            {
                // Если это ДОБАВЛЕНИЕ
                this.Text = "Добавление посещения";
            }
        }

        private void button1_Click(object sender, EventArgs e) // Кнопка ОК
        {
            if (isNewVisit)
            {
                if (currentClientId <= 0)
                {
                    MessageBox.Show("Нельзя добавить посещение без выбора клиента.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Создаем новую строку в таблице Visit
                DataRow newRow = dataSet1.Visit.NewRow();
                // Приведение значений к нужным типам (DataSet ожидает DateTime)
                newRow["DateTimeIn"] = dateTimePicker1.Value;
                newRow["DateTimeOff"] = dateTimePicker2.Value;
                newRow["ZoneVisit"] = textBox1.Text;
                // Если передан ID клиента — устанавливаем внешний ключ
                if (currentClientId > 0)
                {
                    newRow["FK_C"] = currentClientId;
                }

                dataSet1.Visit.Rows.Add(newRow);
                dataSet1.Visit.AcceptChanges();
            }
            else
            {
                // Редактируем существующую строку
                DataRow[] rows = dataSet1.Visit.Select($"ID_V = {currentVisitId}");
                if (rows.Length > 0)
                {
                    rows[0]["DateTimeIn"] = dateTimePicker1.Value;
                    rows[0]["DateTimeOff"] = dateTimePicker2.Value;
                    rows[0]["ZoneVisit"] = textBox1.Text;
                    dataSet1.Visit.AcceptChanges();
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e) // Кнопка Отмена
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private System.Windows.Forms.ContextMenuStrip CreateBasicContextMenu()
        {
            var cms = new System.Windows.Forms.ContextMenuStrip();
            cms.Items.Add("Отменить", null, (s, e) => { SendKeys.Send("^z"); }).Enabled = true;
            cms.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            cms.Items.Add("Вырезать", null, (s, e) => { SendKeys.Send("^x"); });
            cms.Items.Add("Копировать", null, (s, e) => { SendKeys.Send("^c"); });
            cms.Items.Add("Вставить", null, (s, e) => { SendKeys.Send("^v"); });
            cms.Items.Add(new System.Windows.Forms.ToolStripSeparator());
            cms.Items.Add("Выделить всё", null, (s, e) => { SendKeys.Send("^a"); });
            return cms;
        }
    }
}