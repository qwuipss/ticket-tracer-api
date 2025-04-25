using Microsoft.EntityFrameworkCore;
using TicketTracer.Data.Models;

namespace TicketTracer.Data;

public class TicketTracerDbContext(DbContextOptions<TicketTracerDbContext> options) : DbContext(options)
{
    public DbSet<UserDbo> Users { get; init; }
}