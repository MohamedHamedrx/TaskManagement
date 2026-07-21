using TaskManagement.Domain.Common;
using TaskManagement.Domain.Entities.TaskItems;


namespace TaskManagement.Domain.Entities.Projects;

public class Project : BaseEntity
{
    public string Name { get; set; }
    public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();

}