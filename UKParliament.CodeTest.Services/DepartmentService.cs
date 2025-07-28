using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Data.Repositories;

namespace UKParliament.CodeTest.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public Task<List<Department>> GetAllAsync() => _departmentRepository.GetAllAsync();

    public Task<Department?> GetByIdAsync(int id) => _departmentRepository.GetByIdAsync(id);
} 