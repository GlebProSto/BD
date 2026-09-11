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

        // Конструктор принимает DataSet, флаг (новая запись или редактирование) и ID записи
        public Form2(DataSet1 ds, bool isNew, int id = 0)
        {
            InitializeComponent();
            this.dataSet1 = ds;
            this.isNewVisit = isNew;
            this.currentVisitId = id;

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
                // Создаем новую строку в таблице Visit
                DataRow newRow = dataSet1.Visit.NewRow();
                newRow["DateTimeIn"] = dateTimePicker1.Value;
                newRow["DateTimeOff"] = dateTimePicker2.Value;
                newRow["ZoneVisit"] = textBox1.Text;
                // Примечание: поле FK_C (ID клиента) пока оставляем пустым или 0, 
                // если нужно привязать к клиенту, это делается через Form1

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
    }
}