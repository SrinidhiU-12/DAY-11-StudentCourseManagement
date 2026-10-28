using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using StudentCourseManagement.Models;

namespace StudentCourseManagement.Repositories;

public class DepartmentRepository
{
    public async Task<int> AddAsync(Department department)
    {
        const string sql = """
            INSERT INTO Departments (DepartmentName, Location, IsActive)
            VALUES (@DepartmentName, @Location, 1);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        await using SqlConnection connection = DatabaseHelper.CreateConnection();
        await using SqlCommand command = new(sql, connection);

        command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
        command.Parameters.AddWithValue("@Location, object.Value", (object?)department.Location ?? DBNull.Value);

        await connection.OpenAsync();
        object? result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<List<Department>> GetAllAsync()
    {
        const string sql = "SELECT DepartmentId, DepartmentName, Location, IsActive FROM Departments";
        var departments = new List<Department>();

        await using SqlConnection connection = DatabaseHelper.CreateConnection();
        await using SqlCommand command = new(sql, connection);

        await connection.OpenAsync();
        await using SqlDataReader reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            departments.Add(new Department
            {
                DepartmentId = reader.GetInt32(0),
                DepartmentName = reader.GetString(1),
                Location = reader.IsDBNull(2) ? null : reader.GetString(2),
                IsActive = reader.GetBoolean(3)
            });
        }

        return departments;
    }
}