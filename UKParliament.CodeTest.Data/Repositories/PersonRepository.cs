using Microsoft.EntityFrameworkCore;

namespace UKParliament.CodeTest.Data.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly PersonManagerContext _context;

    public PersonRepository(PersonManagerContext context)
    {
        _context = context;
    }

    public async Task<List<Person>> GetAllAsync()
    {
        return await _context.People.Include(p => p.Department).ToListAsync();
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        return await _context.People.Include(p => p.Department).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Person> AddAsync(Person person)
    {
        _context.People.Add(person);
        await _context.SaveChangesAsync();
        return person;
    }

    public async Task<Person> UpdateAsync(Person person)
    {
        // Find the existing tracked entity
        var existingEntity = _context.People.Local.FirstOrDefault(p => p.Id == person.Id);
        
        if (existingEntity != null)
        {
            // Update the existing tracked entity
            _context.Entry(existingEntity).CurrentValues.SetValues(person);
        }
        else
        {
            // No existing tracked entity, check if it exists in the database
            var dbEntity = await _context.People.FindAsync(person.Id);
            if (dbEntity != null)
            {
                // Update the database entity
                _context.Entry(dbEntity).CurrentValues.SetValues(person);
            }
            else
            {
                // Entity doesn't exist, this shouldn't happen for updates
                throw new InvalidOperationException($"Person with ID {person.Id} not found.");
            }
        }
        
        await _context.SaveChangesAsync();
        return person;
    }

    public async Task DeleteAsync(int id)
    {
        var person = await _context.People.FindAsync(id);
        if (person != null)
        {
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
        }
    }
} 