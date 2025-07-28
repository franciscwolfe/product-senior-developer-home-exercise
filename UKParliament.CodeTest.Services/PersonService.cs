using System.Collections.Generic;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Repositories;

namespace UKParliament.CodeTest.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository _personRepository;

    public PersonService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public Task<List<Person>> GetAllAsync() => _personRepository.GetAllAsync();

    public Task<Person?> GetByIdAsync(int id) => _personRepository.GetByIdAsync(id);

    public Task<Person> CreateAsync(Person person) => _personRepository.AddAsync(person);

    public Task<Person> UpdateAsync(Person person) => _personRepository.UpdateAsync(person);

    public Task DeleteAsync(int id) => _personRepository.DeleteAsync(id);
}