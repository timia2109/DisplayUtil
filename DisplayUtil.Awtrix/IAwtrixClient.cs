namespace DisplayUtil.Awtrix;

public interface IAwtrixClient
{
    /// <summary>
    ///     Set the content for an app
    /// </summary>
    /// <param name="appId">ID of app</param>
    /// <param name="app">App content</param>
    /// <returns>Task</returns>
    Task SetAppAsync(string appId, AwtirxCustomApp app);

    /// <summary>
    ///     Sends a notification to Awtrix
    /// </summary>
    /// <param name="notification">Notification content</param>
    /// <returns>Task</returns>
    Task SendNotificationAsync(AwtirxNotification notification);

    /// <summary>
    ///     Set the matrix state (on or off)
    /// </summary>
    /// <param name="enabled">Should be on</param>
    /// <returns>Task</returns>
    Task SetMatrixStateAsync(bool enabled);

    /// <summary>
    ///     Set the Settings for Awtrix
    /// </summary>
    /// <param name="settings">Affected Settings</param>
    /// <returns>Task</returns>
    Task SetSettingsAsync(AwtirxSettings settings);

    /// <summary>
    ///     Toggle the matrix on or off
    /// </summary>
    /// <param name="power">True to turn on, false to turn off</param>
    /// <returns>Task</returns>
    Task SetPowerAsync(bool power);

    /// <summary>
    ///     Send the board into deep sleep mode (turns off the matrix as well)
    /// </summary>
    /// <param name="seconds">Number of seconds to sleep</param>
    /// <returns>Task</returns>
    Task SetSleepAsync(int seconds);

    /// <summary>
    ///     Play a RTTTL sound from the MELODIES folder or a 4-digit MP3 number for DFplayer
    /// </summary>
    /// <param name="sound">Sound filename (without extension) or 4-digit MP3 number</param>
    /// <returns>Task</returns>
    Task PlaySoundAsync(string sound);

    /// <summary>
    ///     Play a RTTTL sound from a given RTTTL string
    /// </summary>
    /// <param name="rtttl">RTTTL string</param>
    /// <returns>Task</returns>
    Task PlayRtttlAsync(string rtttl);

    /// <summary>
    ///     Set the moodlight color or temperature for the entire matrix
    /// </summary>
    /// <param name="moodlight">Moodlight configuration</param>
    /// <returns>Task</returns>
    Task SetMoodlightAsync(AwtrixMoodlight moodlight);

    /// <summary>
    ///     Clear/disable the moodlight
    /// </summary>
    /// <returns>Task</returns>
    Task ClearMoodlightAsync();

    /// <summary>
    ///     Set a colored indicator (1 = upper right, 2 = right side, 3 = lower right)
    /// </summary>
    /// <param name="indicatorNumber">Indicator number (1-3)</param>
    /// <param name="indicator">Indicator configuration</param>
    /// <returns>Task</returns>
    Task SetIndicatorAsync(int indicatorNumber, AwtrixIndicator indicator);

    /// <summary>
    ///     Clear/hide an indicator
    /// </summary>
    /// <param name="indicatorNumber">Indicator number (1-3)</param>
    /// <returns>Task</returns>
    Task ClearIndicatorAsync(int indicatorNumber);

    /// <summary>
    ///     Dismiss a notification that was configured with "hold": true
    /// </summary>
    /// <returns>Task</returns>
    Task DismissNotificationAsync();

    /// <summary>
    ///     Navigate to the next app
    /// </summary>
    /// <returns>Task</returns>
    Task NextAppAsync();

    /// <summary>
    ///     Navigate to the previous app
    /// </summary>
    /// <returns>Task</returns>
    Task PreviousAppAsync();

    /// <summary>
    ///     Switch to a specific app by name
    /// </summary>
    /// <param name="appName">Name of the app (e.g., "Time", "Date")</param>
    /// <returns>Task</returns>
    Task SwitchToAppAsync(string appName);

    /// <summary>
    ///     Reboot the Awtrix device
    /// </summary>
    /// <returns>Task</returns>
    Task RebootAsync();

    /// <summary>
    ///     Initiate firmware update
    /// </summary>
    /// <returns>Task</returns>
    Task DoUpdateAsync();
}