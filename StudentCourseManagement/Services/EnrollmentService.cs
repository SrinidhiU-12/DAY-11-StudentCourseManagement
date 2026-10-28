using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class EnrollmentService
{
    private readonly EnrollmentRepository _repository;

    public EnrollmentService(EnrollmentRepository repository)
    {
        _repository = repository;
    }

    public async Task EnrollStudentAsync(int studentId, int courseId)
    {
        if (studentId <= 0 || courseId <= 0)
            throw new ArgumentException("Invalid student or course ID.");

        await _repository.EnrollStudentAsync(studentId, courseId);
    }
}