using DomainModel;
using Microsoft.EntityFrameworkCore;

namespace DataMapper;

public class LibraryDbContext(DbContextOptions<LibraryDbContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Domain> Domains { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookEdition> BookEditions { get; set; }
    public DbSet<BookRecord> BookRecords { get; set; }
    public DbSet<BorrowRecord> BorrowRecords { get; set; }
}