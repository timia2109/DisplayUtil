namespace DisplayUtil.Awtrix;

/// <summary>
///     Defines what happens when a custom app lifetime expires.
/// </summary>
public enum AwtirxLifetimeMode
{
    /// <summary>Delete the app.</summary>
    Delete = 0,

    /// <summary>Mark the app as stale with a red rectangle.</summary>
    Stale = 1
}