using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Mappers;

public interface IDepartmentMapper
{
    DepartmentViewModel ToViewModel(Department department);
} 