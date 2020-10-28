using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace HangmanAPI.Models
{
    public partial class HangmanAPIDbContext : DbContext
    {
        public HangmanAPIDbContext()
        {
        }

        public HangmanAPIDbContext(DbContextOptions<HangmanAPIDbContext> options, IConfiguration configuration)
            : base(options)
        {
            configuration = Configuration;
        }
        private IConfiguration Configuration { get; set; }
        public virtual DbSet<Words> Words { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Words>(entity =>
            {
                entity.HasKey(e => e.WordId)
                    .HasName("PK__Words__2C20F066943E9657");

                entity.Property(e => e.Word).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
