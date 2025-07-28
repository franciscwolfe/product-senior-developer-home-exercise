using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Mappers;

public class PersonMapper : IPersonMapper
{
    public PersonViewModel ToViewModel(Person person)
    {
        return new PersonViewModel
        {
            Id = person.Id,
            FirstName = person.FirstName,
            LastName = person.LastName,
            DateOfBirth = person.DateOfBirth,
            DepartmentId = person.DepartmentId,
            DepartmentName = person.Department?.Name ?? string.Empty
        };
    }

    public Person ToEntity(PersonViewModel viewModel)
    {
        return new Person
        {
            Id = viewModel.Id,
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            DateOfBirth = viewModel.DateOfBirth,
            DepartmentId = viewModel.DepartmentId
        };
    }
} 