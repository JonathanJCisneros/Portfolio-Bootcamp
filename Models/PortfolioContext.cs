#pragma warning disable CS8618

using Microsoft.EntityFrameworkCore;
namespace Portfolio.Models;

public class PortfolioContext : DbContext 
{ 
    public PortfolioContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; } 
    public DbSet<Inquiry> Inquiries { get; set; } 

}