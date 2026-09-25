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

            // Центрирование диалога над родителем
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            // Ограничение ввода в поле телефона: только цифры и '+' в начале
            try
            {
                this.textBox2.KeyPress += TextBoxPhone_KeyPress;
                this.textBox1.KeyPress += TextBoxFIO_KeyPress; // <-- Подключаем проверку ФИО
                this.textBox1.ContextMenuStrip = CreateBasicContextMenu();
                this.textBox2.ContextMenuStrip = CreateBasicContextMenu();
            }
            catch { }

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

            if (!System.Text.RegularExpressions.Regex.IsMatch(textBox2.Text.Trim(), "^(8|\\+7)\\d{10}$"))
            {
                FillingErrors += "Телефон должен начинаться с 8 или +7 и содержать далее 10 цифр.\n";
            }

            if (FillingErrors != "")
            {
                MessageBox.Show(this, FillingErrors, "Ошибка", MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
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

        private void TextBoxPhone_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // Разрешаем цифры и управляющие символы
            if (char.IsControl(e.KeyChar))
                return;

            TextBox tb = sender as TextBox;
            if (tb == null)
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == '+')
            {
                // '+' только в начале
                if (tb.SelectionStart != 0 || tb.Text.Contains("+"))
                    e.Handled = true;
                return;
            }

            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void TextBoxFIO_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            // Разрешаем управляющие символы (Backspace, Delete и т.д.)
            if (char.IsControl(e.KeyChar))
                return;

            // Блокируем цифры
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            // Разрешаем только буквы, пробелы, дефис и апостроф
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != ' ' && e.KeyChar != '-' && e.KeyChar != '\'')
            {
                e.Handled = true;
            }
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