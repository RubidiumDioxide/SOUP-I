using aa.Models;

namespace aa.Views
{
    public class TaskDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int CreatorId { get; set; }
        public int AssigneeId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsComplete { get; set; }

        public TaskDto(aa.Models.Task t)
        {
            Id = t.Id; 
            ProjectId = t.ProjectId; 
            CreatorId = t.CreatorId; 
            AssigneeId = t.AssigneeId; 
            Name = t.Name; 
            Description = t.Description; 
            IsComplete = t.IsComplete;
        }

        public TaskDto(int id, int projectId, int creatorId, int assigneeId, string name, string description, bool isComplete)
        {
            Id = id; 
            ProjectId = projectId; 
            CreatorId = creatorId; 
            AssigneeId = assigneeId; 
            Name = name; 
            Description = description; 
            IsComplete = isComplete; 
        }

        public TaskDto() { }
    }
}
