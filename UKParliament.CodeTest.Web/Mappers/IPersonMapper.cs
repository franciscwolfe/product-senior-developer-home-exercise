using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Mappers;

public interface IPersonMapper
{
    PersonViewModel ToViewModel(Person person);
    Person ToEntity(PersonViewModel viewModel);
} 