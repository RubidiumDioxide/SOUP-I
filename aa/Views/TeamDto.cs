using aa.Models;

namespace aa.Views
{
    public class TeamDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public string Role { get; set; } = null!;
        public int Level { get; set; }  

        public TeamDto(Team t)
        {
            Id = t.Id;
            UserId = t.UserId; 
            ProjectId = t.ProjectId; 
            Role = t.Role;
            Level = t.Level; 
        }

        public TeamDto(int id, int userId, int projectId, string role, int level)
        {
            Id = id;
            UserId = userId; 
            ProjectId = projectId; 
            Role = role; 
            Level = level; 
        }

        public TeamDto() { }
    }
}
