using aa.Models;

namespace aa.Views
{
    public class ProjectForDisplayDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description {  get; set; } 
        public int CreatorId {  get; set; }
        public string CreatorName { get; set; }
        public bool IsComplete { get; set; }
        public string DateBegan { get; set; }
        public string? DateFinished { get; set; }
        public string? DateDeadline { get; set; }
        public bool IsPrivate { get; set; }

        public ProjectForDisplayDto(Project p, string _creatorName)
        {
            Id = p.Id;
            Name = p.Name;
            Description = p.Description;
            CreatorId = p.Creator;
            CreatorName = _creatorName;
            IsComplete = p.IsComplete;
            DateBegan = p.DateBegan.ToString();
            DateFinished = p.DateFinished.ToString();
            DateDeadline = p.DateDeadline.ToString(); 
            IsPrivate = p.IsPrivate; 
        }

        public ProjectForDisplayDto(int id, string name, string description, int creator, string creatorName, bool isComplete, string dateBegan, string dateFinished, string dateDeadline, bool isPrivate)
        {
            Id = id;
            Name = name;
            Description = description; 
            CreatorId = creator; 
            CreatorName= creatorName;
            IsComplete = isComplete;
            DateBegan = dateBegan;
            DateFinished = dateFinished;
            DateDeadline = dateDeadline;
            IsPrivate = isPrivate; 
        }

        public ProjectForDisplayDto() { }
    }  
}
