namespace LzLeague.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
