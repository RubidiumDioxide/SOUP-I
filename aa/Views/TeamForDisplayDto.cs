using aa.Models;

namespace aa.Views
{
    public class TeamForDisplayDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Role { get; set; } = null!;
        public int Level { get; set; } 

        public TeamForDisplayDto(Team t, string userName, string projectName)
        {
            Id = t.Id;
            UserId = t.UserId; 
            UserName = userName; 
            ProjectId = t.ProjectId; 
            ProjectName = projectName; 
            Role = t.Role; 
            Level = t.Level; 
        }

        public TeamForDisplayDto(int id, int userId,  string userName, int projectId, string projectName, string role, int level)
        { 
            Id = id;
            UserId = userId; 
            UserName = userName;
            ProjectId = projectId; 
            ProjectName = projectName;
            Role = role; 
            Level = level; 
        }

        public TeamForDisplayDto() { }
    }
}
