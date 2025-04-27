using aa.Models;

namespace aa.Views
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description {  get; set; } 
        public int Creator {  get; set; }
        public bool IsComplete { get; set; }
        public string DateBegan { get; set; }
        public string? DateFinished { get; set; }
        public string? DateDeadline { get; set; }
        public bool IsPrivate { get; set; }

        public ProjectDto(Project p)
        {
            Id = p.Id; 
            Name = p.Name;
            Description = p.Description; 
            Creator = p.Creator; 
            IsComplete = p.IsComplete; 
            DateBegan = p.DateBegan.ToString(); 
            DateFinished = p.DateFinished.ToString(); 
            DateDeadline = p.DateDeadline.ToString();  
            IsPrivate = p.IsPrivate; 
        }

        public ProjectDto(int id, string name, string description, int creator, bool isComplete, string dateBegan, string dateFinished, string dateDeadline, bool isPrivate)
        {
            Id = id;
            Name = name;
            Description = description; 
            Creator = creator; 
            IsComplete = isComplete; 
            DateBegan = dateBegan; 
            DateFinished = dateFinished;
            DateDeadline = dateDeadline; 
            IsPrivate = isPrivate; 
        }

        public ProjectDto() { }
    }  
}
