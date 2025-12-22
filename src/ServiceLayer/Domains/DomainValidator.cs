using DomainModel;
using FluentValidation;

namespace ServiceLayer.Domains;

public class DomainValidator : AbstractValidator<Domain>
{
    public DomainValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.ParentDomain).Must((domain, parentDomain) => domain != parentDomain);
        RuleFor(x => x.SubDomains).Must((domain, subDomains) => !subDomains.Contains(domain));
        RuleFor(x => x).Must(d =>
        {
            if (d.ParentDomain is null)
                return true;

            return !d.SubDomains.Contains(d.ParentDomain);
        });
    }
}