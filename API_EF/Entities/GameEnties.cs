using System.ComponentModel.DataAnnotations;

namespace API_EF.Entities

{
    public class GameEnties
    {
        [Key]
        public int GameId { get; set; }
        public required string Name { get; set; } 
        public string Genres { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }
}
