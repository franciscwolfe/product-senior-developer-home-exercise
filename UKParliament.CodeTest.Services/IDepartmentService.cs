using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services;

public interface IDepartmentService
{
    Task<List<Department>> GetAllAsync();
    Task<Department?> GetByIdAsync(int id);
} 