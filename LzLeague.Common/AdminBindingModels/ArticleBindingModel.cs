namespace LzLeague.Common.AdminBindingModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using LzLeague.Models;

    public class ArticleBindingModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string CoverUrl { get; set; }

        public CreateCommentBindingModel CreateCommentModel { get; set; }

        public ICollection<CommentBindingModel> Comments { get; set; }
    }
}
