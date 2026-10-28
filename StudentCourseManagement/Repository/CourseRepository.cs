using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;
using System.Data;

namespace StudentCourseManagement.Repository; // Make sure this matches your project namespace (Repositories or Repository)

public class CourseRepository
{
    public async Task AddCourseAsync(Course course)
    {
        const string sql = """
            INSERT INTO Courses (CourseName, DurationMonths, Fee, DepartmentId)
            VALUES (@CourseName, @DurationMonths, @Fee, @DepartmentId);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using SqlConnection connection = DatabaseHelper.CreateConnection();
        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@CourseName", SqlDbType.NVarChar, 100).Value = course.CourseName;
        command.Parameters.Add("@DurationMonths", SqlDbType.Int).Value = course.DurationMonths;
        command.Parameters.Add("@Fee", SqlDbType.Decimal).Value = course.Fee;
        command.Parameters.Add("@DepartmentId", SqlDbType.Int).Value = course.DepartmentId;

        await connection.OpenAsync();
        await command.ExecuteScalarAsync();
    }

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        const string sql = "SELECT CourseId, CourseName, DurationMonths, Fee, DepartmentId, IsActive FROM Courses";
        var courses = new List<Course>();

        await using SqlConnection connection = DatabaseHelper.CreateConnection();
        await using SqlCommand command = new(sql, connection);

        await connection.OpenAsync();
        await using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            courses.Add(new Course
            {
                CourseId = reader.GetInt32(0),
                CourseName = reader.GetString(1),
                DurationMonths = reader.GetInt32(2),
                Fee = reader.GetDecimal(3),
                DepartmentId = reader.GetInt32(4),
                IsActive = reader.GetBoolean(5)
            });
        }

        return courses;
    }

    public async Task<Course?> GetCourseByIdAsync(int id)
    {
        const string sql = "SELECT CourseId, CourseName, DurationMonths, Fee, DepartmentId, IsActive FROM Courses WHERE CourseId = @CourseId";

        await using SqlConnection connection = DatabaseHelper.CreateConnection();
        await using SqlCommand command = new(sql, connection);
        command.Parameters.Add("@CourseId", SqlDbType.Int).Value = id;

        await connection.OpenAsync();
        await using SqlDataReader reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Course
            {
                CourseId = reader.GetInt32(0),
                CourseName = reader.GetString(1),
                DurationMonths = reader.GetInt32(2),
                Fee = reader.GetDecimal(3),
                DepartmentId = reader.GetInt32(4),
                IsActive = reader.GetBoolean(5)
            };
        }

        return null;
    }
}