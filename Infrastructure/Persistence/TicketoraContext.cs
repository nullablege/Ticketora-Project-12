using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class TicketoraContext:IdentityDbContext<AppUser, IdentityRole<int> , int>
{
    public TicketoraContext(DbContextOptions<TicketoraContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Event>()
            .Property(e => e.Price)
            .HasPrecision(18, 2);

        builder.Entity<EventRegistration>()
            .Property(e => e.TotalPrice)
            .HasPrecision(18, 2);
    }
    
    
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<EventRegistration> EventRegistrations => Set<EventRegistration>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    
}
