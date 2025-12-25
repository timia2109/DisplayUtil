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
}