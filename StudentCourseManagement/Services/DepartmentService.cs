using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class DepartmentService
{
    private readonly DepartmentRepository _repository;

    public DepartmentService(DepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> AddDepartmentAsync(Department department)
    {
        if (string.IsNullOrWhiteSpace(department.DepartmentName))
            throw new ArgumentException("Department name is required.");

        return await _repository.AddAsync(department);
    }

    public Task<List<Department>> GetAllDepartmentsAsync()
    {
        return _repository.GetAllAsync();
    }
}