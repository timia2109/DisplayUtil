using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace DisplayUtil.Providers;

/// <summary>
/// Responsible to download from Google Fonts
/// </summary>
internal partial class GFontDownloader(
    IOptions<ProviderSettings> options,
    ILogger<GFontDownloader> logger,
    IHttpClientFactory httpClientFactory
)
{
    private readonly ILogger _logger = logger;
    private readonly HttpClient _httpClient
        = httpClientFactory.CreateClient(nameof(GFontDownloader));

    private static readonly Dictionary<string, int> _fontWeights = new()
    {
        {"thin", 100},
        {"extra-light", 200},
        {"light", 300},
        {"regular", 400},
        {"medium", 500},
        {"semi-bold", 600},
        {"bold", 700},
        {"extra-bold", 800},
        {"black", 900},
    };

    public bool IsGFont(string fontPath)
    {
        return GetGFontRegex().IsMatch(fontPath);
    }

    private DownloadData ParseExpression(string fontPath)
    {
        var matches = GetGFontRegex().Match(fontPath);

        var download = new DownloadData(
            matches.Groups[1].Value,
            "regular"
        );

        if (matches.Groups.Count >= 3)
            download = download with
            {
                Weight = matches.Groups[2].Value[1..]
            };

        return download;
    }

    private string GeneratePath(DownloadData downloadData)
    {
        var basePath = options.Value.GoogleFontsCachePath;
        return Path.Combine(basePath, downloadData.FileName);
    }

    private static Uri BuildUri(DownloadData downloadData)
    {
        var name = $"{downloadData.FontFamily}:wght@{downloadData.Weight}";
        var uri = $"https://fonts.googleapis.com/css2?family={name}";

        return new Uri(uri);
    }

    private async Task HandleDownloadAsync(DownloadData downloadData, string path)
    {
        var uri = BuildUri(downloadData);
        LogStartDownload(downloadData, uri);

        var response = await _httpClient.GetAsync(uri);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error on downloading font {downloadData}");
        }
        var text = await response.Content.ReadAsStringAsync();

        var ttfUri = GetCssRegex().Match(text);
        if (ttfUri is null)
        {
            throw new Exception($"Could not parse GFont Response on font {downloadData}");
        }

        var fontResponse = await _httpClient.GetAsync(ttfUri.Groups[1].Value);
        await fontResponse.Content.CopyToAsync(File.OpenWrite(path));
    }

    public async Task<string> DownloadFontAsync(string gfontFormat)
    {
        var data = ParseExpression(gfontFormat);

        // Check if weight is number
        if (!GetNumberRegex().IsMatch(data.Weight))
        {
            data = data with
            {
                Weight = _fontWeights[data.Weight].ToString()
            };
        }

        var path = GeneratePath(data);

        // Already downloaded. Skip
        if (File.Exists(path))
        {
            LogExists(data);
            return path;
        }

        await HandleDownloadAsync(data, path);
        LogFinishDownload(data);
        return path;
    }

    [GeneratedRegex("^gfonts://([^@]+)(@.+)?$")]
    private partial Regex GetGFontRegex();

    [GeneratedRegex(@"src:\s+url\((.+)\)\s+format\('truetype'\);")]
    private partial Regex GetCssRegex();

    [GeneratedRegex(@"^\d+$")]
    private partial Regex GetNumberRegex();

    [LoggerMessage(LogLevel.Information, "Font {data} already exists. Skip.")]
    private partial void LogExists(DownloadData data);

    [LoggerMessage(LogLevel.Information, "Start download font {data} from {uri}")]
    private partial void LogStartDownload(DownloadData data, Uri uri);

    [LoggerMessage(LogLevel.Information, "Finished download font {data}")]
    private partial void LogFinishDownload(DownloadData data);

    private record DownloadData(
        string FontFamily,
        string Weight
    )
    {
        public string FileName => $"gfont_{FontFamily}_{Weight}_v1.ttf";
    }
}