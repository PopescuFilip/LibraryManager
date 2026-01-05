using DomainModel;
using FluentValidation.TestHelper;
using ServiceLayer.Domains;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class DomainValidatorTests
{
    public DomainValidator domainValidator = default!;

    [TestInitialize]
    public void Init()
    {
        domainValidator = new DomainValidator();
    }

    [TestMethod]
    public void DomainValidator_ShouldReturnInvalid_WhenNameIsEmpty()
    {
        var domain = new Domain("");

        var result = domainValidator.TestValidate(domain);

        result.ShouldHaveValidationErrorFor(d => d.Name);
    }

    [TestMethod]
    public void DomainValidator_ShouldReturnInvalid_WhenParentIdIsCurrentId()
    {
        var id = 1234;
        var domain = new Domain("Name", id) { Id = id };

        var result = domainValidator.TestValidate(domain);

        result.ShouldHaveValidationErrorFor(d => d.ParentDomainId);
    }

    [TestMethod]
    public void DomainValidator_ShouldReturnInvalid_WhenSubDomainsContainCurrent()
    {
        var id = 1234;
        var name = "Name here";
        var domain = new Domain(name) { Id = id };
        domain.SubDomains.Add(domain);

        var result = domainValidator.TestValidate(domain);

        result.ShouldHaveValidationErrorFor(d => d.SubDomains);
    }

    [TestMethod]
    public void DomainValidator_ShouldReturnInvalid_WhenParentDomainHasTheSameName()
    {
        var id = 1234;
        var name = "Name here";
        var parentDomain = new Domain(name) { Id = 321 };
        var domain = new Domain(name, parentDomain.Id) { Id = id, ParentDomain = parentDomain };

        var result = domainValidator.TestValidate(domain);

        result.ShouldHaveValidationErrorFor(d => d.ParentDomain);
    }

    [TestMethod]
    public void DomainValidator_ShouldReturnValid_WhenDomainIsInCorrectState()
    {
        var id = 1234;
        var parentId = 23;
        var name = "Some Name";
        var domain = new Domain(name, parentId) { Id = id };

        var result = domainValidator.TestValidate(domain);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
