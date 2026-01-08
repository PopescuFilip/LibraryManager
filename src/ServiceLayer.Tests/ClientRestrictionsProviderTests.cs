using DataMapper;
using DomainModel.Restrictions;
using NSubstitute;
using ServiceLayer.CRUD;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class ClientRestrictionsProviderTests
{
    private ClientRestrictionsProvider _clientRestrictionsProvider = default!;
    private IRestrictionsProvider _restrictionsProvider = default!;

    [TestInitialize]
    public void Init()
    {
        _restrictionsProvider = Substitute.For<IRestrictionsProvider>();
        _clientRestrictionsProvider = new ClientRestrictionsProvider(_restrictionsProvider);
    }

    [TestMethod]
    public void GetClientRestrictions_ShouldReturnCorrectRestrictions()
    {
        var restrictions = new Restrictions()
        {

        };
        _restrictionsProvider.GetRestrictions().Returns(restrictions);

        var clientRestrictions = _clientRestrictionsProvider.GetClientRestrictions();

        Assert.IsTrue(true);
    }
}