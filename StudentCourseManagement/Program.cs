using Microsoft.Data.SqlClient;
using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;
using StudentCourseManagement.Repository;
using StudentCourseManagement.Services;

// Initialize Repositories
var deptRepo = new DepartmentRepository();
var studentRepo = new StudentRepository();
var courseRepo = new CourseRepository();
var enrollmentRepo = new EnrollmentRepository();
var auditRepo = new AuditLogRepository();

// Initialize Services
var deptService = new DepartmentService(deptRepo);
var studentService = new StudentService(studentRepo);
var courseService = new CourseService(courseRepo);
var enrollmentService = new EnrollmentService(enrollmentRepo);
var auditService = new AuditLogService(auditRepo);

while (true)
{
    Console.WriteLine("\n========================================");
    Console.WriteLine("     STUDENT COURSE MANAGEMENT SYSTEM   ");
    Console.WriteLine("========================================");
    Console.WriteLine("1. Department Management");
    Console.WriteLine("2. Student Management");
    Console.WriteLine("3. Course Management");
    Console.WriteLine("4. Enrollment Management");
    Console.WriteLine("5. Reports");
    Console.WriteLine("6. View Audit Logs");
    Console.WriteLine("0. Exit");
    Console.WriteLine("========================================");
    Console.Write("Enter your choice: ");

    string? choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                {
                    Console.WriteLine("\n--- Department Management ---");
                    Console.WriteLine("1. List All Departments");
                    Console.WriteLine("2. Add New Department");
                    Console.Write("Choose an option: ");
                    string? dOpt = Console.ReadLine();

                    if (dOpt == "1")
                    {
                        var depts = await deptService.GetAllDepartmentsAsync();
                        foreach (var d in depts)
                            Console.WriteLine($"{d.DepartmentId} | {d.DepartmentName} | {d.Location} | Active: {d.IsActive}");
                    }
                    else if (dOpt == "2")
                    {
                        Console.Write("Department Name: ");
                        string name = Console.ReadLine() ?? "";
                        Console.Write("Location: ");
                        string loc = Console.ReadLine() ?? "";

                        await deptService.AddDepartmentAsync(new Department { DepartmentName = name, Location = loc });
                        Console.WriteLine("Department added successfully.");
                    }
                    break;
                }

            case "2":
                {
                    Console.WriteLine("\n--- Student Management ---");
                    Console.Write("Enter Name: ");
                    string name = Console.ReadLine() ?? "";
                    Console.Write("Enter Email: ");
                    string email = Console.ReadLine() ?? "";
                    Console.Write("Enter Age: ");
                    int.TryParse(Console.ReadLine(), out int age);
                    Console.Write("Enter Department ID: ");
                    int.TryParse(Console.ReadLine(), out int deptId);

                    await studentService.RegisterAsync(new Student
                    {
                        Name = name,
                        Email = email,
                        Age = age,
                        DepartmentId = deptId
                    });
                    Console.WriteLine("Student registered successfully.");
                    break;
                }

            case "4":
                {
                    Console.WriteLine("\n--- Enrollment Management ---");
                    Console.Write("Enter Student ID: ");
                    int.TryParse(Console.ReadLine(), out int studentId);
                    Console.Write("Enter Course ID: ");
                    int.TryParse(Console.ReadLine(), out int courseId);

                    await enrollmentService.EnrollStudentAsync(studentId, courseId);
                    Console.WriteLine("Student enrolled successfully! Transaction and Audit Log recorded.");
                    break;
                }

            case "6":
                {
                    Console.WriteLine("\n--- Recent Audit Logs ---");
                    var logs = await auditService.GetRecentAuditLogsAsync(10);
                    foreach (var log in logs)
                    {
                        Console.WriteLine($"[{log.CreatedAt}] {log.EntityName} (ID: {log.EntityId}) - {log.Action}: {log.Details}");
                    }
                    break;
                }

            case "0":
                Console.WriteLine("Application closed.");
                return;

            default:
                Console.WriteLine("Invalid choice or module under construction.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}