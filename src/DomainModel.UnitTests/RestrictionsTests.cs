using DomainModel.Restrictions;
using System.Diagnostics.CodeAnalysis;

namespace DomainModel.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class RestrictionsTests
{
    [TestMethod]
    public void ExtensionDaysLimitMonthCount_ShouldHaveDefaultValueThree()
    {
        var restrictions = new RawRestrictions();

        Assert.AreEqual(3, restrictions.ExtensionDaysLimitMonthCount);
    }
}