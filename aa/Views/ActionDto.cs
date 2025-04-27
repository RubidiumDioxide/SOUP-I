using aa.Models; 

namespace aa.Views
{
    public class ActionDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int ActorId { get; set; }
        public int TaskId { get; set; }
        public string Description { get; set; } = null!;
        public string? Commit { get; set; }
        public DateTime Date { get; set; }

        public ActionDto(aa.Models.Action a)
        {
            Id = a.Id;
            ProjectId = a.ProjectId;
            ActorId = a.ActorId;
            TaskId = a.TaskId;
            Description = a.Description; 
            Commit = a.Commit; 
            Date = a.Date;
        }

        public ActionDto(int id, int projectId, int actorId, int taskId, string description, string commit, DateTime date)
        {
            Id = id;
            ProjectId = projectId;
            ActorId = actorId;
            TaskId = taskId;
            Description = description;
            Commit = commit; 
            Date = date;
        }

        public ActionDto() { }
    }
}
