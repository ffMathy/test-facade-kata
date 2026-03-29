namespace BookStore.Data.Tests.Builders;

/// <summary>
/// Provides unique non-zero integer IDs for test builders.
/// Each call produces a positive value derived from a new <see cref="Guid"/>,
/// guaranteeing uniqueness within a test run without needing a real database.
/// </summary>
internal static class TestId
{
    /// <summary>Returns a new unique positive non-zero <see cref="int"/> ID.</summary>
    public static int New()
    {
        // Take the first 4 bytes of a new GUID, mask the sign bit to keep the value
        // positive, then OR 1 to guarantee it is never zero.
        var bytes = Guid.NewGuid().ToByteArray();
        return (BitConverter.ToInt32(bytes, 0) & int.MaxValue) | 1;
    }
}
