namespace LzLeague.Common.AdminBindingModels
{
    using System;
    using Models;

    public class CommentBindingModel
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public DateTime PublicationDate { get; set; }

        public string Content { get; set; }
    }
}
