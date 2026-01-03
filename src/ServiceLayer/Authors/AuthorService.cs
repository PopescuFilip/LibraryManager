using DomainModel;
using FluentValidation;
using ServiceLayer.CRUD;

namespace ServiceLayer.Authors;

public interface IAuthorService
{
    Result<Author> Create(string name);
}

public class AuthorService(
    IEntityService<Author> _entityService,
    IValidator<Author> _validator)
    : IAuthorService
{
    public Result<Author> Create(string name)
    {
        return _entityService.Insert(new Author(name), _validator);
    }
}