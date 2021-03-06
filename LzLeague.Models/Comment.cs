﻿namespace LzLeague.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        [Required]
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        [Required]
        public DateTime PublicationDate { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
