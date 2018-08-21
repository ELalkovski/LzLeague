namespace LzLeague.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Article
    {
        public Article()
        {
            this.Comments = new List<Comment>();
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
