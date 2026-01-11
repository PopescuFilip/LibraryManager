using DataMapper;
using DomainModel.Restrictions;
using NSubstitute;
using ServiceLayer.CRUD;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class BookRestrictionsProviderTests
{
    private BookRestrictionsProvider _bookRestrictionsProvider = default!;
    private IRestrictionsProvider _restrictionProvider = default!;

    [TestInitialize]
    public void Init()
    {
        _restrictionProvider = Substitute.For<IRestrictionsProvider>();
        _bookRestrictionsProvider = new BookRestrictionsProvider(_restrictionProvider);
    }

    [TestMethod]
    public void Get_ShouldReturnInvalid_WhenRestrictionsAreNotFound()
    {
        _restrictionProvider.GetRestrictions().Returns((RawRestrictions?)null);

        var result = _bookRestrictionsProvider.Get();

        Assert.IsFalse(result.IsValid);
    }

    [TestMethod]
    public void Get_ShouldReturnCorrectBookRestrictions()
    {
        var restrictions = new RawRestrictions()
        {
            MaxDomains = 10,
        };
        _restrictionProvider.GetRestrictions().Returns(restrictions);

        var result = _bookRestrictionsProvider.Get();

        Assert.IsTrue(result.IsValid);
        var bookRestrictions = result.Get();
        Assert.AreEqual(restrictions.MaxDomains, bookRestrictions.MaxDomains);
    }

    [TestMethod]
    public void Get_ShouldReturnValid_WhenRestrictionsAreFound()
    {
        var restrictions = new RawRestrictions()
        {
            MaxDomains = 10,
        };
        _restrictionProvider.GetRestrictions().Returns(restrictions);

        var result = _bookRestrictionsProvider.Get();

        Assert.IsTrue(result.IsValid);
    }
}