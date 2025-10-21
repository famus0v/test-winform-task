using System.ComponentModel;
using WinFormsApp2.Entity;
using WinFormsApp2.Service;

namespace WinFormsApp2;
public partial class Form1 : Form
{
    private PersonService _service;
    private BindingList<Person> _presons;
    private Person _currentPerson;

    public Form1()
    {
        InitializeComponent();
        _service = new PersonService();
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await InitializeData();
    }

    private async Task InitializeData()
    {
        try
        {
            var peopleList = await _service.GetAllPersons();
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
        dataGridView.ClearSelection();
        txtId.Text = "0";
        txtName.Text = string.Empty;
        dtpBirthdate.Value = DateTime.Now;
        CalculateAndDisplayAge(DateTime.Now);
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
            var name = txtName.Text.Trim();
            var birthdate = dtpBirthdate.Value;

            await _service.CreatePerson(name, birthdate);
            await RefreshData();

            ClearForm();
            MessageBox.Show("Запись успешно добавлена!", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            var name = txtName.Text.Trim();
            var birthdate = dtpBirthdate.Value;

            await _service.UpdatePerson(_currentPerson.Id, name, birthdate);
            await RefreshData();

            ClearForm();
            MessageBox.Show("Запись успешно обновлена!", "Успех",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            await _service.DeletePerson(_currentPerson.Id);
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

    private void dtpBirthdate_ValueChanged(object sender, EventArgs e)
    {
        CalculateAndDisplayAge(dtpBirthdate.Value);
    }

    private void CalculateAndDisplayAge(DateTime birthdate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthdate.Year;

        if (birthdate.Date > today.AddYears(-age))
            age--;

        txtAge.Text = age.ToString();
    }

    private async Task RefreshData()
    {
        var peopleList = await _service.GetAllPersons();
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
