namespace DisplayUtil.Awtrix;

/// <summary>
///     Controls how text casing is handled.
/// </summary>
public enum AwtirxTextCase
{
    /// <summary>Use global device setting.</summary>
    Global = 0,

    /// <summary>Force uppercase.</summary>
    Uppercase = 1,

    /// <summary>Show text exactly as sent.</summary>
    AsSent = 2
}