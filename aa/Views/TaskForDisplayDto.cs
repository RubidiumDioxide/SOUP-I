using aa.Models;

namespace aa.Views
{
    public class TaskForDisplayDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; } 
        public string ProjectName { get; set; } 
        public int CreatorId { get; set; }
        public string CreatorName { get; set; }
        public int AssigneeId { get; set; }
        public string AssigneeName { get; set; }    
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsComplete { get; set; }

        public TaskForDisplayDto(aa.Models.Task t, string creatorName, string assigneeName, string projectName) 
        {
            Id = t.Id;
            ProjectId = t.ProjectId;
            ProjectName = projectName; 
            CreatorId = t.CreatorId;
            CreatorName = creatorName;
            AssigneeId = t.AssigneeId;
            AssigneeName = assigneeName;
            Name = t.Name;
            Description = t.Description;
            IsComplete = t.IsComplete;
        }

        public TaskForDisplayDto(int id, int projectId, string projectName, int creatorId, string creatorName, int assigneeId, string assigneeName, string name, string description, bool isComplete)
        {
            Id = id;
            ProjectId = projectId; 
            ProjectName = projectName; 
            CreatorId = creatorId;
            CreatorName = creatorName; 
            AssigneeId = assigneeId; 
            AssigneeName = assigneeName; 
            Name = name;
            Description = description;
            IsComplete = isComplete;
        }

        public TaskForDisplayDto() { }
    }
}
