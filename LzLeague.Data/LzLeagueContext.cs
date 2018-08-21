namespace LzLeague.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class LzLeagueContext : IdentityDbContext<ApplicationUser>
    {
        public LzLeagueContext(DbContextOptions<LzLeagueContext> options)
            : base(options)
        {
        }

        public DbSet<Match> Matches { get; set; }
        public DbSet<Prediction> Predictions { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupWinnerPrediction> GroupsWinnersPredictions { get; set; }
        public DbSet<MatchResultPrediction> MatchesResultsPredictions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Prediction>(entity =>
            {
                entity.HasOne(p => p.Owner)
                    .WithOne(au => au.Prediction)
                    .HasForeignKey<Prediction>(p => p.OwnerId);
            });

            builder.Entity<GroupWinnerPrediction>(entity =>
            {
                entity.HasKey(gwp => new { gwp.GroupId, gwp.PredictionId });

                entity.HasOne(gwp => gwp.Group)
                    .WithMany(g => g.GroupWinnerPredictions)
                    .HasForeignKey(gwp => gwp.GroupId);

                entity.HasOne(gwp => gwp.Prediction)
                    .WithMany(p => p.GroupsWinners)
                    .HasForeignKey(gwp => gwp.PredictionId);

            });

            builder.Entity<MatchResultPrediction>(entity =>
            {
                entity.HasKey(mrp => new {mrp.PredictionId, mrp.MatchId});

                entity.HasOne(mrp => mrp.Match)
                    .WithMany(m => m.MatchResultPredictions)
                    .HasForeignKey(mrp => mrp.MatchId);

                entity.HasOne(mrp => mrp.Prediction)
                    .WithMany(p => p.MatchResultsPredictions)
                    .HasForeignKey(mrp => mrp.PredictionId);
            });
        }
    }
}
