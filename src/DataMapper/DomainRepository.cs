using DomainModel;
using Microsoft.EntityFrameworkCore;

namespace DataMapper;

public interface IDomainRepository: IRepository<int, Domain> {}

public sealed class DomainRepository(IDbContextFactory<LibraryDbContext> dbContextFactory)
    : Repository<int, Domain>(dbContextFactory), IDomainRepository
{
    public override void Insert(Domain entity)
    {
        using var context = _dbContextFactory.CreateDbContext();
        if (entity.ParentDomain is not null)
        {
            context.Attach(entity.ParentDomain);
        }
        context.Domains.Add(entity);

        context.SaveChanges();
    }
}