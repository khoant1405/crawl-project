using Demo.CoreData.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo.CoreData.Entities;

public partial class DemoDbContext : DbContext
{
    public DemoDbContext()
    {
    }

    public DemoDbContext(DbContextOptions<DemoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<ArticleContent> ArticleContents { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https: //go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(
            "Server=localhost,1433;User ID=sa;Password=1405;TrustServerCertificate=True;Initial Catalog=CoreData;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Pk_Article_Id");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<ArticleContent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Pk_ArticleContent_Id");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Article).WithOne(p => p.ArticleContent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_ArticleContent_ArticleId");
        });

        modelBuilder.Entity<Category>(entity => { entity.HasKey(e => e.Id).HasName("Pk_Category_Id"); });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Pk_User_Id");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}