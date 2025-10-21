using Microsoft.EntityFrameworkCore;

namespace WinFormsApp2;

public class PostgresRepository : IDisposable
{
    private readonly AppDbContext _context;

    public PostgresRepository()
    {
        _context = new AppDbContext();
    }

    public async Task<List<Person>> GetAll() =>
        await _context.People
            .OrderBy(p => p.Id)
            .ToListAsync();

    public async Task<Person> GetById(int id) =>
        await _context.People
            .FirstOrDefaultAsync(p => p.Id == id);
    

    public async Task Add(Person person)
    {
        await _context.People.AddAsync(person);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Person person)
    {
        var existingPerson = await _context.People.FindAsync(person.Id);
        if (existingPerson == null)
            return;

        existingPerson.Name = person.Name;
        existingPerson.Age = person.Age;
        existingPerson.Birthdate = person.Birthdate;

        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var person = await _context.People.FindAsync(id);
        if (person == null)
            return;

        _context.People.Remove(person);
        _context.SaveChanges();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
