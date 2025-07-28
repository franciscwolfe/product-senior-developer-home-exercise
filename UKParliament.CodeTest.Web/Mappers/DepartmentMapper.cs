using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Mappers;

public class DepartmentMapper : IDepartmentMapper
{
    public DepartmentViewModel ToViewModel(Department department)
    {
        return new DepartmentViewModel
        {
            Id = department.Id,
            Name = department.Name
        };
    }
} 