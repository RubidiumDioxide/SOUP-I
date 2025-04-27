using aa.Models;

namespace aa.Views
{
    public class RepositoryDto
    {
        public int Id { get; set; }
        public string GithubName { get; set; } = null!; 
        public string GithubCreator { get; set; } = null!;
        
        public RepositoryDto(Repository r)
        {
            Id = r.Id; 
            GithubName = r.GithubName; 
            GithubCreator = r.GithubCreator; 
        }

        public RepositoryDto(int id, string githubName, string githubCreator)
        {
            Id = id;
            GithubName = githubName; 
            GithubCreator = githubCreator; 
        }

        public RepositoryDto() { }
    }
}
