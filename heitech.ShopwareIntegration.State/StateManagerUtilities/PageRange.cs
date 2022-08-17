namespace heitech.ShopwareIntegration.State.StateManagerUtilities;

/// <summary>
/// Describes the available Range for a page request
/// </summary>
internal readonly struct PageRange
{
    public int Start { get; }
    public int End { get; }

    public PageRange(ushort start, ushort end)
    {
        Start = start;
        End = end;
    }

    public bool IsValid()
    {
        return Start < End;
    }

    public IEnumerable<int> AsRange()
    {
        var count = Start;
        while (count <= End)
        {
            yield return count;
            count++;
        }
    }
}