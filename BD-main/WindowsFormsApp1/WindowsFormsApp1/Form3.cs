using System;
using System.Data;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        Int64 editId;
        private DataSet1 dataSet1;
        public Int64 NewId;
        bool newClient;
        bool saveb = false;

        public Form3(DataSet1 dataSet1, bool newClient, Int64 editId = 0)
        {
            InitializeComponent();
            this.editId = editId;
            this.dataSet1 = dataSet1;
            this.newClient = newClient;

            if (!newClient)
            {
                this.Text = "Редактирование клиента";
                DataView dataView = dataSet1.Client.AsDataView();
                dataView.RowFilter = $"ID_C = {editId}";

                if (dataView.Count > 0)
                {
                    DataRowView row = dataView[0];
                    textBox1.Text = row["FCs"].ToString();
                    textBox2.Text = row["Phone"].ToString();
                }
            }
            else
            {
                this.Text = "Добавление клиента";
            }
        }

        private void SaveClient()
        {
            if (newClient)
            {
                DataRow newRow = dataSet1.Client.NewRow();
                newRow["FCs"] = textBox1.Text;
                newRow["Phone"] = textBox2.Text;
                dataSet1.Client.Rows.Add(newRow);
                dataSet1.Client.AcceptChanges();
                NewId = Convert.ToInt64(newRow["ID_C"].ToString());
            }
            else
            {
                DataRow[] rows = dataSet1.Client.Select($"ID_C = {editId}");
                if (rows.Length > 0)
                {
                    rows[0]["FCs"] = textBox1.Text;
                    rows[0]["Phone"] = textBox2.Text;
                    dataSet1.Client.AcceptChanges();
                    NewId = editId;
                }
            }
        }

        private void ClientValidating()
        {
            saveb = false;
            string FillingErrors = "";

            if (textBox1.Text == "")
                FillingErrors = "ФИО не заполнено.\n";
            if (textBox2.Text == "")
                FillingErrors = FillingErrors + "Телефон не заполнен.\n";

            if (FillingErrors != "")
            {
                MessageBox.Show(FillingErrors, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);
                saveb = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClientValidating();
            if (!saveb)
            {
                SaveClient();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = saveb;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}