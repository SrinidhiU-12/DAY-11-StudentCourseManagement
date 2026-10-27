using StudentCourseManagement.Models;
using StudentCourseManagement.Repository;

namespace StudentCourseManagement.Services;

public class StudentService
{
    private readonly StudentRepository _repository;

    public StudentService(StudentRepository repository)
    {
        _repository = repository;
    }

    public async Task AddStudentAsync(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.Name))
            throw new ArgumentException("Student name is required.");

        if (string.IsNullOrWhiteSpace(student.Email))
            throw new ArgumentException("Student email is required.");

        if (student.Age < 17 || student.Age > 60)
            throw new ArgumentException("Age must be between 17 and 60.");

        await _repository.AddStudentAsync(student);
    }

    public Task<List<Student>> GetAllStudentsAsync()
    {
        return _repository.GetAllStudentsAsync();
    }

    public async Task<Student?> GetStudentByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Student ID must be greater than zero.");

        return await _repository.GetStudentByIdAsync(id);
    }
}
