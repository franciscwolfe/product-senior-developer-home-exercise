using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Repositories;
using UKParliament.CodeTest.Services;

namespace UKParliament.CodeTest.Tests;

public class PersonServiceTests
{
    private readonly Mock<IPersonRepository> _personRepositoryMock;
    private readonly PersonService _personService;

    public PersonServiceTests()
    {
        _personRepositoryMock = new Mock<IPersonRepository>();
        _personService = new PersonService(_personRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsyncReturnsPeople()
    {
        var persons = new List<Person>
        {
            new Person { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = new DateTime(2000,01,01), DepartmentId = 1 }
        };
        _personRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(persons);

        var result = await _personService.GetAllAsync();

        Assert.Equal(persons, result);
        _personRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsyncEmptyList()
    {
        var emptyList = new List<Person>();
        _personRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(emptyList);

        var result = await _personService.GetAllAsync();

        Assert.Empty(result);
        _personRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync()
    {
        var person = new Person { Id = 1, FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(2000,01,01), DepartmentId = 2 };
        _personRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(person);

        var result = await _personService.GetByIdAsync(1);

        Assert.Equal(person, result);
        _personRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsyncPersonNotFound()
    {
        _personRepositoryMock.Setup(repo => repo.GetByIdAsync(999)).ReturnsAsync((Person?)null);

        var result = await _personService.GetByIdAsync(999);

        Assert.Null(result);
        _personRepositoryMock.Verify(repo => repo.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task CreateAsyncOK()
    {
        var person = new Person { FirstName = "John", LastName = "Smith", DateOfBirth = new DateTime(1990,05,15), DepartmentId = 1 };
        var createdPerson = new Person { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = new DateTime(1990,05,15), DepartmentId = 1 };
        
        _personRepositoryMock.Setup(repo => repo.AddAsync(person)).ReturnsAsync(createdPerson);

        var result = await _personService.CreateAsync(person);

        Assert.Equal(createdPerson, result);
        _personRepositoryMock.Verify(repo => repo.AddAsync(person), Times.Once);
    }

    [Fact]
    public async Task UpdateAsyncOK()
    {
        var person = new Person { Id = 1, FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(1985,03,20), DepartmentId = 2 };
        var updatedPerson = new Person { Id = 1, FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(1985,03,20), DepartmentId = 2 };
        
        _personRepositoryMock.Setup(repo => repo.UpdateAsync(person)).ReturnsAsync(updatedPerson);

        var result = await _personService.UpdateAsync(person);

        Assert.Equal(updatedPerson, result);
        _personRepositoryMock.Verify(repo => repo.UpdateAsync(person), Times.Once);
    }

    [Fact]
    public async Task DeleteAsyncOK()
    {
        const int personId = 1;
        _personRepositoryMock.Setup(repo => repo.DeleteAsync(personId)).Returns(Task.CompletedTask);

        await _personService.DeleteAsync(personId);

        _personRepositoryMock.Verify(repo => repo.DeleteAsync(personId), Times.Once);
    }
}
