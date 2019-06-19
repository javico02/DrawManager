using DrawManager.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DrawManager.Api.Infrastructure
{
    public class DrawManagerDbContext : DbContext
    {
        private static readonly Func<DrawManagerDbContext, int, IEnumerable<DrawEntry>> _getEntriesByDrawExcludingPreviousWinnersAndLosers = EF.CompileQuery((DrawManagerDbContext context, int drawId) => context
                .DrawEntries
                .AsNoTracking()
                .Where(de => de.DrawId == drawId)
                .Select(de => new DrawEntry { Id = de.Id, EntrantId = de.EntrantId })
                );

        public DrawManagerDbContext()
        { }

        public DrawManagerDbContext(DbContextOptions<DrawManagerDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Draw> Draws { get; set; }
        public DbSet<Prize> Prizes { get; set; }
        public DbSet<Entrant> Entrants { get; set; }
        public DbSet<DrawEntry> DrawEntries { get; set; }
        public DbSet<PrizeSelectionStep> PrizeSelectionSteps { get; set; }

        public IEnumerable<DrawEntry> GetEntriesByDrawExcludingPreviousWinnersAndLosers(int drawId)
        {
            return _getEntriesByDrawExcludingPreviousWinnersAndLosers(this, drawId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prize>(prize =>
            {
                prize
                    .HasOne(p => p.Draw)
                    .WithMany(d => d.Prizes)
                    .HasForeignKey(p => p.DrawId);
            });

            modelBuilder.Entity<Entrant>(entrant =>
            {
                entrant.
                    HasIndex(e => e.Id)
                    .IsUnique();
            });

            modelBuilder.Entity<DrawEntry>(drawEntry =>
            {
                drawEntry
                    .HasIndex(de => de.Id)
                    .IsUnique();

                drawEntry
                    .HasOne(de => de.Entrant)
                    .WithMany(e => e.Entries)
                    .HasForeignKey(de => de.EntrantId);

                drawEntry
                    .HasOne(de => de.Draw)
                    .WithMany(d => d.Entries)
                    .HasForeignKey(de => de.DrawId);
            });

            modelBuilder.Entity<PrizeSelectionStep>(prizeSelectionStep =>
            {
                prizeSelectionStep
                    .HasKey(pss => new { pss.PrizeId, pss.EntrantId, pss.DrawEntryId });

                prizeSelectionStep
                    .HasOne(pss => pss.Prize)
                    .WithMany(p => p.SelectionSteps)
                    .HasForeignKey(pss => pss.PrizeId);

                prizeSelectionStep
                    .HasOne(pss => pss.Entrant)
                    .WithMany(e => e.SelectionSteps)
                    .HasForeignKey(pss => pss.EntrantId);

                prizeSelectionStep
                   .HasOne(pss => pss.DrawEntry)
                   .WithMany(de => de.SelectionSteps)
                   .HasForeignKey(pss => pss.DrawEntryId);
            });
        }
    }
}
