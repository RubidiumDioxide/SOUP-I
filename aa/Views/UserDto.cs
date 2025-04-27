using aa.Models;

namespace aa.Views
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password {  get; set; }

        public UserDto(User u)
        {
            Id = u.Id; 
            Name = u.Name;
            Password = u.Password; 
        }

        public UserDto(int id, string name, string password)
        {
            Id = id;
            Name = name;
            Password = password; 
        }

        public UserDto() { }
    }  
}
