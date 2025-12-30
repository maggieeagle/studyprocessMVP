using Application.DTO;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Text;

public class CourseAssignmentsExportService : ICourseAssignmentsExportService
{
    private readonly ICourseRepository _courseRepository;
    private readonly ILogger<CourseAssignmentsExportService> _logger;

    public CourseAssignmentsExportService(ICourseRepository courseRepository, ILogger<CourseAssignmentsExportService> logger)
    {
        _courseRepository = courseRepository;
        _logger = logger;
    }

    public async Task<List<CourseAssignmentsExportDTO>> GetAssignmentsForCourseAsync(int courseId)
    {
        var course = await _courseRepository.GetByIdWithAssignmentsAsync(courseId);

        if (course == null)
        {
            _logger.LogWarning("Export failed: course not found  for CourseId {CourseId})", courseId);

            throw new Exception("Course not found");
        }

        return course.Assignments.Select(a => new CourseAssignmentsExportDTO
        {
            CourseName = course.Name,
            CourseCode = course.Code,
            AssignmentName = a.Title,
            Type = a is HomeworkAssignment ? "Homework" : "Exam",
            DueDate = a.DueDate,
            Status = a.Status.ToString(),
        }).ToList();
    }

    public string ToCsv(List<CourseAssignmentsExportDTO> assignments)
    {
        try
        {
            var sb = new StringBuilder();
            sb.AppendLine("Course Name,Course Code,Assignment Name,Type,Due Date,Status,Number of Submissions");

            foreach (var a in assignments)
            {
                sb.AppendLine(
                    $"{a.CourseName},{a.CourseCode},{a.AssignmentName},{a.Type},{a.DueDate:yyyy-MM-dd},{a.Status}");
            }

            _logger.LogInformation("CSV generation completed successfully");

            return sb.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while generating CSV export");

            throw;
        }
    }
}
