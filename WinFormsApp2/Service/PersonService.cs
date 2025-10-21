using WinFormsApp2.Domain;
using WinFormsApp2.Entity;

namespace WinFormsApp2.Service;

public class PersonService
{
    private PostgresRepository _repository;

    public PersonService()
    {
        InitializeDatabase();
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

    public async Task CreatePerson(string name, DateTime birthdate)
    {
        var person = new Person(name, birthdate);
        await _repository.Add(person);
    }

    public async Task DeletePerson(int id) =>
        await _repository.Delete(id);

    public async Task UpdatePerson(int id, string name, DateTime birthdate)
    {
        var person = new Person(name, birthdate);
        await _repository.Update(id, person);
    }

    public async Task<List<Person>> GetAllPersons() => 
        await _repository.GetAll();
    
}