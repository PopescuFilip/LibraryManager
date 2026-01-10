using DomainModel;
using FluentValidation;
using ServiceLayer.CRUD;

namespace ServiceLayer.BookDefinitions;

public class BookDefinitionValidator : AbstractValidator<BookDefinition>
{
    public BookDefinitionValidator(
        IDomainQueryService _domainQueryService,
        IBookRestrictionsProvider _bookRestrictionsProvider)
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Authors.Count).GreaterThan(0);
        RuleFor(x => x.Domains.Count).GreaterThan(0);
        RuleFor(x => x.Domains.Count)
            .Must(count =>
            {
                var result = _bookRestrictionsProvider.Get();
                if (!result.IsValid)
                    return false;

                var restrictions = result.Get();
                return count <= restrictions.MaxDomains;
            });
        RuleFor(x => x.Domains)
            .Must(domains =>
            {
                var ids = domains.Select(domain => domain.Id);
                var implicitDomainNames = _domainQueryService.GetImplicitDomainNames(ids)
                .Distinct()
                .ToList();

                return !domains.Select(d => d.Name).Any(implicitDomainNames.Contains);
            })
            .When(x => x.Domains.Count > 1);
    }
}