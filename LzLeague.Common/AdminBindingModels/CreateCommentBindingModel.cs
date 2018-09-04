namespace LzLeague.Common.AdminBindingModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Models;

    public class CreateCommentBindingModel
    {
        [Required]
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public DateTime PublicationDate { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
