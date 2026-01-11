using System.Collections;

namespace ServiceLayer;

public class IdCollection(IEnumerable<int> ids) : IReadOnlyCollection<int>
{
    private readonly int[] _ids = [.. ids];

    public int Count => ((IReadOnlyCollection<int>)_ids).Count;

    public IEnumerator<int> GetEnumerator() => ((IEnumerable<int>)_ids).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _ids.GetEnumerator();
}