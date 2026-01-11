using System.Collections;

namespace ServiceLayer;

public class UniqueCollection<T>(IEnumerable<T> values) : IReadOnlyCollection<T>
{
    private readonly T[] _internalArray = [.. values.Distinct()];

    public int Count => ((IReadOnlyCollection<T>)_internalArray).Count;

    public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_internalArray).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _internalArray.GetEnumerator();
}

public sealed class BookDomainIds(IEnumerable<int> values) : UniqueCollection<int>(values)
{}

public sealed class BookParentDomainIds(IEnumerable<int> values) : UniqueCollection<int>(values)
{}