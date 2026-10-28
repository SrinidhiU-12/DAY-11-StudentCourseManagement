using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;
using StudentCourseManagement.Repository;

namespace StudentCourseManagement.Services;

public class StudentService
{
    private readonly StudentRepository _repository;

    public StudentService(StudentRepository repository)
    {
        _repository = repository;
    }

    public Task<int> RegisterAsync(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.Name))
            throw new ArgumentException("Student name is required.");

        if (string.IsNullOrWhiteSpace(student.Email))
            throw new ArgumentException("Student email is required.");

        if (student.Age is < 17 or > 60)
            throw new ArgumentOutOfRangeException(
                nameof(student.Age), "Age must be between 17 and 60.");

        if (student.DepartmentId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(student.DepartmentId), "Select a valid department.");

        return _repository.AddAsync(student);
    }
}