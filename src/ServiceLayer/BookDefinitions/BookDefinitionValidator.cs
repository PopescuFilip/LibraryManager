using DomainModel;
using FluentValidation;
using ServiceLayer.CRUD;

namespace ServiceLayer.BookDefinitions;

public class BookDefinitionValidator : AbstractValidator<BookDefinition>
{
    public BookDefinitionValidator(IDomainQueryService _domainQueryService,
        IBookRestrictionsProvider _bookRestrictionsProvider)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Authors.Count).GreaterThan(0);
        RuleFor(x => x.Domains.Count).GreaterThan(0);
        RuleFor(x => x.Domains.Count).LessThanOrEqualTo(_bookRestrictionsProvider.Get().MaxDomains);
        RuleFor(x => x.Domains).Must(domains =>
        {
            var implicitDomainNames = domains
            .Select(d => d.Id)
            .SelectMany(_domainQueryService.GetImplicitDomainNames)
            .Distinct()
            .ToList();

            return !domains.Select(d => d.Name).Any(implicitDomainNames.Contains);
        });
    }
}