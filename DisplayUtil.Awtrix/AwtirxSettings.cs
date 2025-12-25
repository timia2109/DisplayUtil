using System.Text.Json;
using System.Text.Json.Serialization;

namespace DisplayUtil.Awtrix;

/// <summary>
///     AWTRIX global settings payload (System.Text.Json).
///     Property names map 1:1 to the short AWTRIX keys (e.g. ATIME, TEFF, ...).
/// </summary>
public record AwtirxSettings
{
    /// <summary>
    ///     Duration an app is displayed in seconds.
    ///     Positive integer.
    ///     Default: 7
    /// </summary>
    [JsonPropertyName("ATIME")]
    public int Atime { get; init; } = 7;

    /// <summary>
    ///     Choose between app transition effects.
    ///     Range: 0–10
    ///     Default: 1
    /// </summary>
    [JsonPropertyName("TEFF")]
    public AwtirxTransitionEffect TransitionEffect { get; init; } = AwtirxTransitionEffect.Effect1;

    /// <summary>
    ///     Time taken for the transition to the next app in milliseconds.
    ///     Positive integer.
    ///     Default: 500
    /// </summary>
    [JsonPropertyName("TSPEED")]
    public int TransitionSpeedMs { get; init; } = 500;

    /// <summary>
    ///     Global text color.
    ///     RGB array or hex color string.
    /// </summary>
    [JsonPropertyName("TCOL")]
    public Color? GlobalTextColor { get; init; }

    /// <summary>
    ///     Changes the time app style.
    ///     Range: 0–6
    ///     Default: 1
    /// </summary>
    [JsonPropertyName("TMODE")]
    public AwtirxTimeMode TimeMode { get; init; } = AwtirxTimeMode.Mode1;

    /// <summary>
    ///     Calendar header color of the time app.
    ///     RGB array or hex color string.
    ///     Default: #FF0000
    /// </summary>
    [JsonPropertyName("CHCOL")]
    public Color? CalendarHeaderColor { get; init; }

    /// <summary>
    ///     Calendar body color of the time app.
    ///     RGB array or hex color string.
    ///     Default: #FFFFFF
    /// </summary>
    [JsonPropertyName("CBCOL")]
    public Color? CalendarBodyColor { get; init; }

    /// <summary>
    ///     Calendar text color in the time app.
    ///     RGB array or hex color string.
    ///     Default: #000000
    /// </summary>
    [JsonPropertyName("CTCOL")]
    public Color? CalendarTextColor { get; init; }

    /// <summary>
    ///     Enable or disable the weekday display.
    ///     Default: true
    /// </summary>
    [JsonPropertyName("WD")]
    public bool WeekdayDisplay { get; init; } = true;

    /// <summary>
    ///     Active weekday color.
    ///     RGB array or hex color string.
    /// </summary>
    [JsonPropertyName("WDCA")]
    public Color? WeekdayColorActive { get; init; }

    /// <summary>
    ///     Inactive weekday color.
    ///     RGB array or hex color string.
    /// </summary>
    [JsonPropertyName("WDCI")]
    public Color? WeekdayColorInactive { get; init; }

    /// <summary>
    ///     Matrix brightness.
    ///     Range: 0–255
    /// </summary>
    [JsonPropertyName("BRI")]
    public int? Brightness { get; init; }

    /// <summary>
    ///     Automatic brightness control.
    /// </summary>
    [JsonPropertyName("ABRI")]
    public bool? AutoBrightness { get; init; }

    /// <summary>
    ///     Automatic switching to the next app.
    /// </summary>
    [JsonPropertyName("ATRANS")]
    public bool? AutoTransitionToNextApp { get; init; }

    /// <summary>
    ///     Color correction for the matrix.
    ///     RGB array.
    /// </summary>
    [JsonPropertyName("CCORRECTION")]
    public int[]? ColorCorrection { get; init; }

    /// <summary>
    ///     Color temperature for the matrix.
    ///     RGB array.
    /// </summary>
    [JsonPropertyName("CTEMP")]
    public int[]? ColorTemperature { get; init; }

    /// <summary>
    ///     Time format for the TimeApp.
    ///     Varies.
    /// </summary>
    [JsonPropertyName("TFORMAT")]
    public string? TimeFormat { get; init; }

    /// <summary>
    ///     Date format for the DateApp.
    ///     Varies.
    /// </summary>
    [JsonPropertyName("DFORMAT")]
    public string? DateFormat { get; init; }

    /// <summary>
    ///     Start the week on Monday.
    ///     Default: true
    /// </summary>
    [JsonPropertyName("SOM")]
    public bool StartOnMonday { get; init; } = true;

    /// <summary>
    ///     Shows the temperature in Celsius (Fahrenheit when false).
    ///     Default: true
    /// </summary>
    [JsonPropertyName("CEL")]
    public bool ShowCelsius { get; init; } = true;

    /// <summary>
    ///     Block physical navigation keys (still sends input to MQTT).
    ///     Default: false
    /// </summary>
    [JsonPropertyName("BLOCKN")]
    public bool BlockNavigationKeys { get; init; } = false;

    /// <summary>
    ///     Display text in uppercase.
    ///     Default: true
    /// </summary>
    [JsonPropertyName("UPPERCASE")]
    public bool Uppercase { get; init; } = true;

    /// <summary>
    ///     Text color of the time app. Use 0 for global text color.
    ///     RGB array or hex color string.
    /// </summary>
    [JsonPropertyName("TIME_COL")]
    public Color? TimeAppTextColor { get; init; }

    /// <summary>
    ///     Text color of the date app. Use 0 for global text color.
    ///     RGB array or hex color string.
    /// </summary>
    [JsonPropertyName("DATE_COL")]
    public Color? DateAppTextColor { get; init; }

    /// <summary>
    ///     Text color of the temperature app. Use 0 for global text color.
    ///     RGB array or hex color string.
    /// </summary>
    [JsonPropertyName("TEMP_COL")]
    public Color? TemperatureAppTextColor { get; init; }

    /// <summary>
    ///     Text color of the humidity app. Use 0 for global text color.
    ///     RGB array or hex color string.
    /// </summary>
    [JsonPropertyName("HUM_COL")]
    public Color? HumidityAppTextColor { get; init; }

    /// <summary>
    ///     Text color of the battery app. Use 0 for global text color.
    ///     RGB array or hex color string.
    /// </summary>
    [JsonPropertyName("BAT_COL")]
    public Color? BatteryAppTextColor { get; init; }

    /// <summary>
    ///     Scroll speed modification.
    ///     Percentage of original scroll speed.
    ///     Default: 100
    /// </summary>
    [JsonPropertyName("SSPEED")]
    public int ScrollSpeed { get; init; } = 100;

    /// <summary>
    ///     Enable or disable the native time app (requires reboot).
    ///     Default: true
    /// </summary>
    [JsonPropertyName("TIM")]
    public bool TimeAppEnabled { get; init; } = true;

    /// <summary>
    ///     Enable or disable the native date app (requires reboot).
    ///     Default: true
    /// </summary>
    [JsonPropertyName("DAT")]
    public bool DateAppEnabled { get; init; } = true;

    /// <summary>
    ///     Enable or disable the native humidity app (requires reboot).
    ///     Default: true
    /// </summary>
    [JsonPropertyName("HUM")]
    public bool HumidityAppEnabled { get; init; } = true;

    /// <summary>
    ///     Enable or disable the native temperature app (requires reboot).
    ///     Default: true
    /// </summary>
    [JsonPropertyName("TEMP")]
    public bool TemperatureAppEnabled { get; init; } = true;

    /// <summary>
    ///     Enable or disable the native battery app (requires reboot).
    ///     Default: true
    /// </summary>
    [JsonPropertyName("BAT")]
    public bool BatteryAppEnabled { get; init; } = true;

    /// <summary>
    ///     Enable or disable the matrix.
    ///     Similar to power endpoint but without the animation.
    ///     Default: true
    /// </summary>
    [JsonPropertyName("MATP")]
    public bool MatrixEnabled { get; init; } = true;

    /// <summary>
    ///     Allows to set the volume of the buzzer and DFplayer.
    ///     Range: 0–30
    /// </summary>
    [JsonPropertyName("VOL")]
    public int? Volume { get; init; }

    /// <summary>
    ///     Sets a global effect overlay (cannot be used with app specific overlays).
    ///     Varies.
    /// </summary>
    [JsonPropertyName("OVERLAY")]
    public string? Overlay { get; init; }
}