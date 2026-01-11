using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests;

[TestClass]
[ExcludeFromCodeCoverage]
public class IdCollectionTests
{
    [TestMethod]
    public void Constructor_ShouldCopyContentsOfOriginialIEnumerable()
    {
        var ids = new List<int>() { 1, 2, 3, 4 };

        var collection = new IdCollection(ids);

        Assert.AreEqual(ids.Count, collection.Count);
        Assert.IsTrue(ids.SequenceEqual(collection));
    }

    [TestMethod]
    public void Constructor_ShouldConstructADifferentBackingCollection()
    {
        var ids = new List<int>() { 1, 2, 3, 4 };
        var originalIdsCount = ids.Count;
        var originalIds = ids.ToList();

        var collection = new IdCollection(ids);
        ids.Add(1);
        ids.AddRange([1, 2, 3]);

        Assert.AreEqual(originalIdsCount, collection.Count);
        Assert.IsTrue(originalIds.SequenceEqual(collection));
    }
}