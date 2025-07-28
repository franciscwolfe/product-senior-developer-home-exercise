namespace UKParliament.CodeTest.Data.Repositories;

public interface IPersonRepository
{
    Task<List<Person>> GetAllAsync();
    Task<Person?> GetByIdAsync(int id);
    Task<Person> AddAsync(Person person);
    Task<Person> UpdateAsync(Person person);
    Task DeleteAsync(int id);
} 