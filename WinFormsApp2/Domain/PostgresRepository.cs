using Microsoft.EntityFrameworkCore;
using WinFormsApp2.Entity;

namespace WinFormsApp2.Domain;

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

    public async Task Add(Person person)
    {
        await _context.People.AddAsync(person);
        await _context.SaveChangesAsync();
    }

    public async Task Update(int id, Person person)
    {
        var existingPerson = await _context.People.FindAsync(id);
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
