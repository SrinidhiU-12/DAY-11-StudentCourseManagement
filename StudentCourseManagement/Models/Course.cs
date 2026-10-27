namespace StudentCourseManagement.Models;

public class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; } = string.Empty;
    public int DurationMonths { get; set; }
    public decimal Fee { get; set; }
}
