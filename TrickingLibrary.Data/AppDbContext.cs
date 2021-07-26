using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Models;

namespace TrickingLibrary.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base((options)) {}
        
        public DbSet<Trick> Tricks { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<TrickRelationship> TrickRelationships { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TrickCategory> TrickCategories { get; set; }
        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TrickCategory>()
                .HasKey(tr => new {tr.CategoryId, tr.TrickId});

            modelBuilder.Entity<TrickRelationship>()
                .HasKey(tr => new {tr.PrerequisiteId, tr.ProgressionId});
            
            // specifying the relationship between TrickRelations
            // easy -> hard
            modelBuilder.Entity<TrickRelationship>()
                .HasOne(tr => tr.Progression)
                .WithMany(tr => tr.Prerequisites)
                .HasForeignKey(tr => tr.ProgressionId);
            
            // hard -> easy
            modelBuilder.Entity<TrickRelationship>()
                .HasOne(tr => tr.Prerequisite)
                .WithMany(tr => tr.Progressions)
                .HasForeignKey(tr => tr.PrerequisiteId);
        }
    }
}