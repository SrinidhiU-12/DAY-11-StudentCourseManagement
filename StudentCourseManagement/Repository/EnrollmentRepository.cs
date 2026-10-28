using Microsoft.Data.SqlClient;
using StudentCourseManagement.Helpers;
using System.Data;

namespace StudentCourseManagement.Repositories;

public class EnrollmentRepository
{
    public async Task EnrollStudentAsync(int studentId, int courseId)
    {
        await using SqlConnection connection = DatabaseHelper.CreateConnection();
        await connection.OpenAsync();

        await using SqlTransaction transaction =
            (SqlTransaction)await connection.BeginTransactionAsync();

        try
        {
            const string enrollmentSql = """
                INSERT INTO Enrollments (StudentId, CourseId, Status)
                VALUES (@StudentId, @CourseId, N'Enrolled');
                SELECT CAST(SCOPE_IDENTITY() AS INT);
                """;

            int enrollmentId;
            await using (SqlCommand command = new(enrollmentSql, connection, transaction))
            {
                command.Parameters.Add("@StudentId", SqlDbType.Int).Value = studentId;
                command.Parameters.Add("@CourseId", SqlDbType.Int).Value = courseId;
                enrollmentId = Convert.ToInt32(await command.ExecuteScalarAsync());
            }

            const string auditSql = """
                INSERT INTO AuditLogs (EntityName, EntityId, Action, Details)
                VALUES (@EntityName, @EntityId, @Action, @Details);
                """;

            await using (SqlCommand command = new(auditSql, connection, transaction))
            {
                command.Parameters.Add("@EntityName", SqlDbType.NVarChar, 50).Value = "Enrollment";
                command.Parameters.Add("@EntityId", SqlDbType.Int).Value = enrollmentId;
                command.Parameters.Add("@Action", SqlDbType.NVarChar, 20).Value = "INSERT";
                command.Parameters.Add("@Details", SqlDbType.NVarChar, 500).Value =
                    $"Student {studentId} enrolled in course {courseId}.";
                await command.ExecuteNonQueryAsync();
            }

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}