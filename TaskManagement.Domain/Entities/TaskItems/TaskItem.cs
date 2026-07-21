using TaskManagement.Domain.Common;
using TaskManagement.Domain.Entities.Projects;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities.TaskItems
{
    public class TaskItem : BaseEntity
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public TaskItemStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public Project Project { get; set; } = null!;

    }
}
