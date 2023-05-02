using Microsoft.EntityFrameworkCore;

namespace TaskIt.API.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<ProjectNote> ProjectNotes => Set<ProjectNote>();
    public DbSet<TicketNote> TicketNotes => Set<TicketNote>();
    public DbSet<ProjectFile> ProjectFiles => Set<ProjectFile>();
    public DbSet<TicketFile> TicketFiles => Set<TicketFile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ticket>()
                    .HasOne(t => t.Project)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(k => k.ProjectId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProjectNote>()
                    .HasOne(n => n.Project)
                    .WithMany(p => p.Notes)
                    .HasForeignKey(k => k.ProjectId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ProjectFile>()
                    .HasOne(f => f.Project)
                    .WithMany(p => p.Files)
                    .HasForeignKey(k => k.ProjectId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TicketNote>()
                    .HasOne(n => n.Ticket)
                    .WithMany(t => t.Notes)
                    .HasForeignKey(k => k.TicketId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TicketFile>()
                    .HasOne(f => f.Ticket)
                    .WithMany(t => t.Files)
                    .HasForeignKey(k => k.TicketId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);
    }
}