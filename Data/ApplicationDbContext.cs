using DotNet8Learning.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet8Learning.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
}