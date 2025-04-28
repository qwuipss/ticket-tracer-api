using Microsoft.EntityFrameworkCore;
using TicketTracer.Data.Entities;
using TicketTracer.Data.Entities.Abstract;

namespace TicketTracer.Data;

public class TicketTracerDbContext(DbContextOptions<TicketTracerDbContext> options) : DbContext(options)
{
    public DbSet<ProjectEntity> Projects { get; init; }

    public DbSet<BoardEntity> Boards { get; init; }

    public DbSet<TicketEntity> Tickets { get; init; }

    public DbSet<UserEntity> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var baseEntityType = typeof(BaseEntity);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (baseEntityType.IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder
                    .Entity(entityType.ClrType)
                    .Property<DateTime>(nameof(BaseEntity.CreatedAt))
                    .HasColumnType("timestamp without time zone")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .ValueGeneratedOnAdd();
            }
        }

        base.OnModelCreating(modelBuilder);
    }
}