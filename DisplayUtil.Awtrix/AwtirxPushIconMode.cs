namespace DisplayUtil.Awtrix;

/// <summary>
///     Controls icon scrolling behavior.
/// </summary>
public enum AwtirxPushIconMode
{
    /// <summary>Icon does not move.</summary>
    Static = 0,

    /// <summary>Icon moves with text and disappears.</summary>
    MoveOnce = 1,

    /// <summary>Icon moves with text and reappears when scrolling restarts.</summary>
    MoveAndRepeat = 2
}