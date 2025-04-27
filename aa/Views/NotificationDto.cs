using aa.Models;

namespace aa.Views
{
    public partial class NotificationDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Role { get; set; } = null!;
        public string Type { get; set; } = null!; 
    
        public NotificationDto(Notification n)
        {
            Id = n.Id; 
            ProjectId = n.ProjectId; 
            SenderId = n.SenderId;
            ReceiverId = n.ReceiverId; 
            Role = n.Role; 
            Type = n.Type; 
        } 

        public NotificationDto(int id, int projectId, int senderId, int receiverId, string role, string type)
        {
            Id = id;
            ProjectId = projectId;
            SenderId = senderId;
            ReceiverId = receiverId;
            Role = role;
            Type = type;
        }

        public NotificationDto() { }
    }
}
