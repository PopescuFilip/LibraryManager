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
    public DbSet<BookDefinition> BookDefinitions { get; set; }
    public DbSet<BookEdition> BookEditions { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BorrowRecord> BorrowRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BorrowRecord>()
            .HasOne(x => x.Lender)
            .WithMany(x => x.BorrowRecords)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BorrowRecord>()
            .HasOne(x => x.Borrower)
            .WithMany(x => x.BorrowRecords)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BorrowRecord>()
            .HasMany(x => x.BorrowedBooks)
            .WithMany();

        modelBuilder.Entity<BookDefinition>()
            .HasMany(x => x.Domains)
            .WithMany();

        modelBuilder.Entity<Domain>()
            .HasMany(x => x.SubDomains)
            .WithOne(x => x.ParentDomain);

        modelBuilder.Entity<Client>()
            .HasOne(x => x.Account)
            .WithOne();

        modelBuilder.Entity<Employee>()
            .HasOne(x => x.Account)
            .WithOne();

        base.OnModelCreating(modelBuilder);
    }
}