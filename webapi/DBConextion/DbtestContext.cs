
using Microsoft.EntityFrameworkCore;
using webapi.Models;

namespace webapi.DBConextion;

public partial class DbtestContext : DbContext
{
    public DbtestContext()
    {
    }

    public DbtestContext(DbContextOptions<DbtestContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Launch> Launches { get; set; }
    public virtual DbSet<Rocket> Rockets { get; set; }
    public virtual DbSet<Mission> Missions { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { 
       
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Launch>(entity =>
        {
            entity.ToTable("launch");
            entity.Property(e => e.ID).HasColumnType("int(11)");
            //entity.Property(e => e.DateCached).HasColumnType("datetime");
            entity.Property(e => e.DateLunch).HasColumnType("datetime").IsRequired();
            entity.Property(e => e.FirstRocketlaunch).HasDefaultValue(false);
            ///entity.Property(e => e.Cached).HasDefaultValue(false);
            entity.Property(e => e.MissionID).HasColumnType("int(11)").IsRequired();
            entity.Property(e => e.RocketID).HasColumnType("int(11)").IsRequired();
        });
        modelBuilder.Entity<Mission>(entity =>
        {
            entity.ToTable("mission");
            entity.Property(e => e.ID).HasColumnType("int(11)");
             entity.Property(e => e.MissionName).HasMaxLength(45).IsRequired();
        });
        modelBuilder.Entity<Rocket>(entity =>
        {
            entity.ToTable("rocket");
            entity.Property(e => e.ID).HasColumnType("int(11)");
            entity.Property(e => e.RocketName).HasMaxLength(45).IsRequired();
        });
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
