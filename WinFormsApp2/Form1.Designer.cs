namespace WinFormsApp2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView dataGridView;
        private TextBox txtId;
        private TextBox txtName;
        private TextBox txtAge;
        private DateTimePicker dtpBirthdate;
        private Button btnSave;
        private Button btnUpdate;
        private Button btnDelete;
        private Button btnClear;
        private Label lblId;
        private Label lblName;
        private Label lblAge;
        private Label lblBirthdate;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 461);
            this.Text = "Test Task Application";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(700, 500);

            InitializeControls();
        }

        private void InitializeControls()
        {
            // DataGridView
            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Top;
            dataGridView.Height = 250;
            dataGridView.SelectionChanged += dataGridView_SelectionChanged;

            // Labels
            lblId = new Label { Text = "ID:", Location = new Point(20, 270), Width = 80 };
            lblName = new Label { Text = "Имя:", Location = new Point(20, 300), Width = 80 };
            lblAge = new Label { Text = "Возраст:", Location = new Point(20, 330), Width = 80 };
            lblBirthdate = new Label { Text = "Дата рождения:", Location = new Point(20, 360), Width = 80 };

            // TextBoxes
            txtId = new TextBox { Location = new Point(100, 267), Width = 100, ReadOnly = true, Text = "0" };
            txtName = new TextBox { Location = new Point(100, 297), Width = 180 };
            txtAge = new TextBox { Location = new Point(100, 327), Width = 100, ReadOnly = true };
            txtAge.KeyPress += txtAge_KeyPress;

            // DateTimePicker
            dtpBirthdate = new DateTimePicker { Location = new Point(100, 357), Width = 180 };
            dtpBirthdate.ValueChanged += dtpBirthdate_ValueChanged;

            // Buttons
            btnSave = new Button { Text = "Добавить", Location = new Point(350, 295), Width = 100 };
            btnSave.Click += btnSave_Click;

            btnUpdate = new Button { Text = "Обновить", Location = new Point(350, 325), Width = 100 };
            btnUpdate.Click += btnUpdate_Click;
            btnUpdate.Enabled = false;

            btnDelete = new Button { Text = "Удалить", Location = new Point(350, 355), Width = 100 };
            btnDelete.Click += btnDelete_Click;
            btnDelete.Enabled = false;

            btnClear = new Button { Text = "Очистить", Location = new Point(470, 295), Width = 100 };
            btnClear.Click += btnClear_Click;

            // Add controls to form
            Controls.AddRange(new Control[] {
            dataGridView, lblId, lblName, lblAge, lblBirthdate,
            txtId, txtName, txtAge, dtpBirthdate,
            btnSave, btnUpdate, btnDelete, btnClear
            });
        }
    }
}

