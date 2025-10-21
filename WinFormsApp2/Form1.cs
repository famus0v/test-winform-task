using System;
using System.ComponentModel;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private PostgresRepository _repository;
        private BindingList<Person> _presons;
        private Person _currentPerson;

        public Form1()
        {
            InitializeComponent();
            InitializeDatabase();
            InitializeData().Wait();
        }

        private void InitializeDatabase()
        {
            try
            {
                using (var context = new AppDbContext())
                    context.Database.EnsureCreated();
                _repository = new PostgresRepository();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}",
                    "Ошибка подключения",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
        }

        private async Task InitializeData()
        {
            try
            {
                var peopleList = await _repository.GetAll();
                _presons = new BindingList<Person>(peopleList);
                dataGridView.DataSource = _presons;
                ConfigureDataGridView();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigureDataGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.ReadOnly = true;

            dataGridView.Columns.Clear();

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Id",
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 50
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Name",
                DataPropertyName = "Name",
                HeaderText = "Имя",
                Width = 150
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Age",
                DataPropertyName = "Age",
                HeaderText = "Возраст",
                Width = 70
            });

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Birthdate",
                DataPropertyName = "Birthdate",
                HeaderText = "Дата рождения",
                Width = 120
            });
        }

        private void ClearForm()
        {
            txtId.Text = "0";
            txtName.Text = string.Empty;
            txtAge.Text = string.Empty;
            dtpBirthdate.Value = DateTime.Now;
            _currentPerson = null;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                var person = new Person
                {
                    Name = txtName.Text.Trim(),
                    Age = int.Parse(txtAge.Text),
                    Birthdate = dtpBirthdate.Value
                };

                await _repository.Add(person);
                await RefreshData();
                ClearForm();
                MessageBox.Show("Запись успешно добавлена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                dataGridView.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            if (_currentPerson == null || !ValidateForm())
                return;

            try
            {
                _currentPerson.Name = txtName.Text.Trim();
                _currentPerson.Age = int.Parse(txtAge.Text);
                _currentPerson.Birthdate = dtpBirthdate.Value;

                await _repository.Update(_currentPerson);
                RefreshData();
                ClearForm();
                MessageBox.Show("Запись успешно обновлена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                dataGridView.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при обновлении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (_currentPerson == null)
                return;

            var result = MessageBox.Show("Вы уверены, что хотите удалить эту запись?",
                "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                await _repository.Delete(_currentPerson.Id);
                await RefreshData();
                ClearForm();
                MessageBox.Show("Запись успешно удалена!", "Успех",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e) => ClearForm();

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            ClearForm();

            if (dataGridView.SelectedRows.Count == 0)
                return;

            var selectedRow = dataGridView.SelectedRows[0];
            _currentPerson = selectedRow.DataBoundItem as Person;

            if (_currentPerson == null)
                return;

            txtId.Text = _currentPerson.Id.ToString();
            txtName.Text = _currentPerson.Name;
            txtAge.Text = _currentPerson.Age.ToString();
            dtpBirthdate.Value = _currentPerson.Birthdate;

            btnDelete.Enabled = true;
            btnUpdate.Enabled = true;
            btnSave.Enabled = false;
        }

        private async Task RefreshData()
        {
            var peopleList = await _repository.GetAll();
            _presons.Clear();
            foreach (var person in peopleList)
                _presons.Add(person);
        }

        private bool ValidateForm() 
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Пожалуйста, введите имя", "Ошибка валидации",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (!int.TryParse(txtAge.Text, out int age) || age < 0 || age > 150)
            {
                MessageBox.Show("Пожалуйста, введите корректный возраст (0-100)", "Ошибка валидации",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAge.Focus();
                return false;
            }

            if (dtpBirthdate.Value > DateTime.Now)
            {
                MessageBox.Show("Дата рождения не может быть в будущем", "Ошибка валидации",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpBirthdate.Focus();
                return false;
            }

            return true;
        }

        private void txtAge_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}
