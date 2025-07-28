using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using UKParliament.CodeTest.Web.Mappers;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Tests;

public class PersonControllerTests
{
    private readonly Mock<IPersonService> _personServiceMock;
    private readonly Mock<IPersonMapper> _personMapperMock;
    private readonly Mock<IDepartmentService> _departmentServiceMock;
    private readonly PersonController _controller;

    public PersonControllerTests()
    {
        _personServiceMock = new Mock<IPersonService>();
        _personMapperMock = new Mock<IPersonMapper>();
        _departmentServiceMock = new Mock<IDepartmentService>();
        _controller = new PersonController(_personServiceMock.Object, _personMapperMock.Object, _departmentServiceMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsAllPeople()
    {
        // Arrange
        var persons = new List<Person>
        {
            new Person { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = new DateTime(2000,01,01), DepartmentId = 1 },
            new Person { Id = 2, FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(1995,05,15), DepartmentId = 2 }
        };
        
        var viewModels = new List<PersonViewModel>
        {
            new PersonViewModel { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = new DateTime(2000,01,01), DepartmentId = 1 },
            new PersonViewModel { Id = 2, FirstName = "Jane", LastName = "Smith", DateOfBirth = new DateTime(1995,05,15), DepartmentId = 2 }
        };

        _personServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(persons);
        _personMapperMock.Setup(m => m.ToViewModel(persons[0])).Returns(viewModels[0]);
        _personMapperMock.Setup(m => m.ToViewModel(persons[1])).Returns(viewModels[1]);

        // Act
        var result = await _controller.GetAll();

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        var resultData = okResult.Value as IEnumerable<PersonViewModel>;
        Assert.NotNull(resultData);
        Assert.Equal(2, resultData.Count());
        _personServiceMock.Verify(s => s.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetById_PersonExists_ReturnOkWithPerson()
    {
        // Arrange
        var person = new Person { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = new DateTime(2000,01,01), DepartmentId = 1 };
        var viewModel = new PersonViewModel { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = new DateTime(2000,01,01), DepartmentId = 1 };

        _personServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(person);
        _personMapperMock.Setup(m => m.ToViewModel(person)).Returns(viewModel);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.Equal(viewModel, okResult!.Value);
        _personServiceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetById_PersonNotFound_ReturnNotFound()
    {
        // Arrange
        _personServiceMock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Person?)null);

        // Act
        var result = await _controller.GetById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        _personServiceMock.Verify(s => s.GetByIdAsync(999), Times.Once);
    }

    [Fact]
    public async Task CreateOK()
    {
        // Arrange
        var viewModel = new PersonViewModel
        {
            FirstName = "John",
            LastName = "Smith",
            DateOfBirth = new DateTime(2000,01,01),
            DepartmentId = 1
        };
        
        var department = new Department { Id = 1, Name = "Sales" };
        var person = new Person { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = new DateTime(2000,01,01), DepartmentId = 1 };
        
        _departmentServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(department);
        _personMapperMock.Setup(m => m.ToEntity(viewModel)).Returns(person);
        _personServiceMock.Setup(s => s.CreateAsync(person)).ReturnsAsync(person);
        _personMapperMock.Setup(m => m.ToViewModel(person)).Returns(viewModel);

        // Act
        var result = await _controller.Create(viewModel);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result.Result);
        _departmentServiceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
        _personServiceMock.Verify(s => s.CreateAsync(person), Times.Once);
    }

    [Fact]
    public async Task CreateInvalidDepartment()
    {
        // Arrange
        var viewModel = new PersonViewModel
        {
            FirstName = "John",
            LastName = "Smith", 
            DateOfBirth = new DateTime(2000,01,01),
            DepartmentId = -1
        };
        
        _departmentServiceMock.Setup(s => s.GetByIdAsync(-1)).ReturnsAsync((Department?)null);

        // Act
        var result = await _controller.Create(viewModel);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
        _departmentServiceMock.Verify(s => s.GetByIdAsync(-1), Times.Once);
        _personServiceMock.Verify(s => s.CreateAsync(It.IsAny<Person>()), Times.Never);
    }

    [Fact]
    public async Task UpdateOK()
    {
        // Arrange
        var viewModel = new PersonViewModel
        {
            Id = 1,
            FirstName = "John",
            LastName = "Smith",
            DateOfBirth = new DateTime(2000,01,01),
            DepartmentId = 2
        };
        
        var department = new Department { Id = 2, Name = "Marketing" };
        var existingPerson = new Person { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = new DateTime(2000,01,01), DepartmentId = 1 };
        var updatedPerson = new Person { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = new DateTime(2000,01,01), DepartmentId = 2 };
        
        _departmentServiceMock.Setup(s => s.GetByIdAsync(2)).ReturnsAsync(department);
        _personServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(existingPerson);
        _personMapperMock.Setup(m => m.ToEntity(viewModel)).Returns(updatedPerson);
        _personServiceMock.Setup(s => s.UpdateAsync(updatedPerson)).ReturnsAsync(updatedPerson);
        _personMapperMock.Setup(m => m.ToViewModel(updatedPerson)).Returns(viewModel);

        // Act
        var result = await _controller.Update(1, viewModel);

        // Assert
        Assert.IsType<OkObjectResult>(result.Result);
        _departmentServiceMock.Verify(s => s.GetByIdAsync(2), Times.Once);
        _personServiceMock.Verify(s => s.UpdateAsync(updatedPerson), Times.Once);
    }

    [Fact]
    public async Task UpdateInvalidDepartment()
    {
        // Arrange
        var viewModel = new PersonViewModel
        {
            Id = 1,
            FirstName = "John",
            LastName = "Smith",
            DateOfBirth = new DateTime(2000,01,01),
            DepartmentId = -1
        };
        
        _departmentServiceMock.Setup(s => s.GetByIdAsync(-1)).ReturnsAsync((Department?)null);

        // Act
        var result = await _controller.Update(1, viewModel);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
        _departmentServiceMock.Verify(s => s.GetByIdAsync(-1), Times.Once);
        _personServiceMock.Verify(s => s.UpdateAsync(It.IsAny<Person>()), Times.Never);
    }

    [Fact]
    public async Task UpdateIdMismatch()
    {
        // Arrange
        var viewModel = new PersonViewModel
        {
            Id = 2,
            FirstName = "John",
            LastName = "Smith",
            DateOfBirth = new DateTime(2000,01,01),
            DepartmentId = 1
        };

        // Act
        var result = await _controller.Update(1, viewModel);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
        _personServiceMock.Verify(s => s.UpdateAsync(It.IsAny<Person>()), Times.Never);
    }

    [Fact]
    public async Task UpdatePersonNotFound()
    {
        // Arrange
        var viewModel = new PersonViewModel
        {
            Id = 999,
            FirstName = "John",
            LastName = "Smith",
            DateOfBirth = new DateTime(2000,01,01),
            DepartmentId = 1
        };
        
        var department = new Department { Id = 1, Name = "Sales" };
        
        _departmentServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(department);
        _personServiceMock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Person?)null);

        // Act
        var result = await _controller.Update(999, viewModel);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        _personServiceMock.Verify(s => s.GetByIdAsync(999), Times.Once);
        _personServiceMock.Verify(s => s.UpdateAsync(It.IsAny<Person>()), Times.Never);
    }

    [Fact]
    public async Task DeletePersonExists()
    {
        // Arrange
        var person = new Person { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = new DateTime(2000,01,01), DepartmentId = 1 };
        
        _personServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(person);
        _personServiceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _personServiceMock.Verify(s => s.GetByIdAsync(1), Times.Once);
        _personServiceMock.Verify(s => s.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeletePersonNotFound()
    {
        // Arrange
        _personServiceMock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Person?)null);

        // Act
        var result = await _controller.Delete(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        _personServiceMock.Verify(s => s.GetByIdAsync(999), Times.Once);
        _personServiceMock.Verify(s => s.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
} 