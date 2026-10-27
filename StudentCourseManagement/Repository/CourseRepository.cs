using Microsoft.Data.SqlClient;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repository;

public class CourseRepository
{
    private readonly string _connectionString;

    public CourseRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task AddCourseAsync(Course course)
    {
        const string query = """
            INSERT INTO Courses (CourseName, DurationMonths, Fee)
            VALUES (@CourseName, @DurationMonths, @Fee)
            """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@CourseName", course.CourseName);
        command.Parameters.AddWithValue("@DurationMonths", course.DurationMonths);
        command.Parameters.AddWithValue("@Fee", course.Fee);

        await connection.OpenAsync();
        await command.ExecuteNonQueryAsync();
    }

    public async Task<List<Course>> GetAllCoursesAsync()
    {
        const string query = """
            SELECT CourseId, CourseName, DurationMonths, Fee
            FROM Courses
            ORDER BY CourseId
            """;

        var courses = new List<Course>();

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            courses.Add(new Course
            {
                CourseId = reader.GetInt32(0),
                CourseName = reader.GetString(1),
                DurationMonths = reader.GetInt32(2),
                Fee = reader.GetDecimal(3)
            });
        }

        return courses;
    }

    public async Task<Course?> GetCourseByIdAsync(int id)
    {
        const string query = """
            SELECT CourseId, CourseName, DurationMonths, Fee
            FROM Courses
            WHERE CourseId = @CourseId
            """;

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@CourseId", id);

        await connection.OpenAsync();
        await using var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Course
            {
                CourseId = reader.GetInt32(0),
                CourseName = reader.GetString(1),
                DurationMonths = reader.GetInt32(2),
                Fee = reader.GetDecimal(3)
            };
        }

        return null;
    }
}
