using DomainModel;
using Microsoft.EntityFrameworkCore;

namespace DataMapper;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    public DbSet<Domain> Domains { get; set; }
}