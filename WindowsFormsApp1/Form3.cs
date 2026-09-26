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

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            try
            {
                this.textBox1.KeyPress += TextBoxFIO_KeyPress;
                this.textBox1.KeyDown += TextBoxFIO_KeyDown;
                this.textBox1.AcceptsReturn = false;
                this.textBox1.Multiline = false;
                this.textBox1.ContextMenuStrip = CreateBasicContextMenu();
                this.maskedTextBox2.ContextMenuStrip = CreateBasicContextMenu();

                // При клике на поле телефона — курсор после скобки
                this.maskedTextBox2.Click += MaskedTextBox2_Click;
                // При входе в поле телефона (Tab) — курсор после скобки
                this.maskedTextBox2.Enter += MaskedTextBox2_Enter;
                // При нажатии Enter в поле телефона — как кнопка ОК
                this.maskedTextBox2.KeyDown += MaskedTextBox2_KeyDown;

                this.textBox1.KeyDown += TextBoxFIO_KeyDown_Enter;
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
                    maskedTextBox2.Text = row["Phone"].ToString();
                }
            }
            else
            {
                this.Text = "Добавление клиента";
            }

            // При открытии формы — фокус на ФИО
            this.Shown += (s, e) =>
            {
                textBox1.Focus();
            };
        }

        // При клике на поле телефона — курсор после скобки (позиция 4)
        private void MaskedTextBox2_Click(object sender, EventArgs e)
        {
            maskedTextBox2.SelectionStart = 4;
            maskedTextBox2.SelectionLength = 0;
        }

        // При входе в поле телефона — курсор после скобки
        private void MaskedTextBox2_Enter(object sender, EventArgs e)
        {
            maskedTextBox2.SelectionStart = 4;
            maskedTextBox2.SelectionLength = 0;
        }

        // При нажатии Enter в поле телефона — как кнопка ОК
        private void MaskedTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                // Вызываем ту же логику, что и кнопка ОК
                button1_Click(sender, e);
            }
        }

        private void TextBoxFIO_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void TextBoxFIO_KeyDown_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                maskedTextBox2.Focus(); // Переход на телефон
            }
        }

        private void SaveClient()
        {
            if (newClient)
            {
                DataRow newRow = dataSet1.Client.NewRow();
                newRow["FCs"] = textBox1.Text;
                string phone = maskedTextBox2.Text.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("+", "");
                newRow["Phone"] = phone;
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
                    string phone = maskedTextBox2.Text.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("+", "");
                    rows[0]["Phone"] = phone;
                    dataSet1.Client.AcceptChanges();
                    NewId = editId;
                }
            }
        }

        private void ClientValidating()
        {
            saveb = false;
            string FillingErrors = "";

            if (string.IsNullOrWhiteSpace(textBox1.Text))
                FillingErrors = "ФИО не заполнено.\n";

            if (!maskedTextBox2.MaskCompleted)
            {
                FillingErrors += "Телефон заполнен не полностью. Введите номер целиком.\n";
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

        private void TextBoxFIO_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
                return;
            if (char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
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