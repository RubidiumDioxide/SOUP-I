using aa.Models;

namespace aa.Views
{
    public class ActionForDisplayDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; } 
        public string ProjectName { get; set; } 
        public int ActorId { get; set; }
        public string ActorName { get; set; } 
        public int TaskId { get; set; }
        public string TaskName { get; set; } 
        public string Description { get; set; } = null!;
        public string? Commit { get; set; }
        public DateTime Date { get; set; }

        public ActionForDisplayDto(aa.Models.Action a, string projectName, string actorName, string taskName)
        {
            Id = a.Id;
            ProjectId = a.ProjectId;
            ProjectName = projectName;
            ActorId = a.ActorId;
            ActorName = actorName;
            TaskId = a.TaskId;
            TaskName = taskName;
            Description = a.Description;
            Commit = a.Commit; 
            Date = a.Date;
        }

        public ActionForDisplayDto(int id, int projectId, string projectName, int actorId, string actorName, int taskId, string taskName, string description, string commit, DateTime date)
        {
            Id = id;
            ProjectId = projectId;
            ProjectName = projectName; 
            ActorId = actorId;
            ActorName = actorName;
            TaskId = taskId;
            TaskName = taskName;
            Description = description;
            Commit = commit; 
            Date = date;
        }

        public ActionForDisplayDto() { }
    }
}
