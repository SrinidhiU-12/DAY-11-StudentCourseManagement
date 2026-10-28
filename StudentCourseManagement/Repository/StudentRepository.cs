using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;
using System.Data;

namespace StudentCourseManagement.Repositories; // or StudentCourseManagement.Repository depending on your namespace

public class StudentRepository
{
    public async Task<int> AddAsync(Student student)
    {
        const string sql = """
            INSERT INTO Students (Name, Email, Age, DepartmentId)
            VALUES (@Name, @Email, @Age, @DepartmentId);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using SqlConnection connection = DatabaseHelper.CreateConnection();
        await using SqlCommand command = new(sql, connection);

        command.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = student.Name;
        command.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = student.Email;
        command.Parameters.Add("@Age", SqlDbType.Int).Value = student.Age;
        command.Parameters.Add("@DepartmentId", SqlDbType.Int).Value = student.DepartmentId;

        await connection.OpenAsync();
        object? result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }
}