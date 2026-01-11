using DomainModel;
using System.Diagnostics.CodeAnalysis;

namespace ServiceLayer.UnitTests.TestHelpers;

[ExcludeFromCodeCoverage]
public class FakeEntity : IEntity
{
    public int Id { get; set; }
}