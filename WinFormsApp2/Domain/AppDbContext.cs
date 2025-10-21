using Microsoft.EntityFrameworkCore;
using WinFormsApp2.Entity;

namespace WinFormsApp2.Domain;

public class AppDbContext : DbContext
{
    public DbSet<Person> People { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "Host=localhost;Port=5432;Database=persons_db;Username=postgres;Password=admin";
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.ToTable("people");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Birthdate)
                .HasColumnName("birthdate")
                .HasColumnType("timestamp without time zone")
                .IsRequired();

            entity.Ignore(e => e.Age);
        });
    }
}
