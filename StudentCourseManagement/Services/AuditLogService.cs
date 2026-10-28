using StudentCourseManagement.Models;
using StudentCourseManagement.Repositories;

namespace StudentCourseManagement.Services;

public class AuditLogService
{
    private readonly AuditLogRepository _repository;

    public AuditLogService(AuditLogRepository repository)
    {
        _repository = repository;
    }

    public Task<List<AuditLog>> GetRecentAuditLogsAsync(int count = 10)
    {
        return _repository.GetRecentAuditLogsAsync(count);
    }
}