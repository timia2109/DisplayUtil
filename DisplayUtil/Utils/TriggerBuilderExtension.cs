using Quartz;

namespace DisplayUtil.Utils;

public static class TriggerBuilderExtension
{

    private static TimeSpan _securityTimeout = TimeSpan.FromSeconds(30);

    public static TriggerBuilder WithSecurityTimeout(this TriggerBuilder builder)
    {
        return builder.StartAt(
            DateTimeOffset.Now + _securityTimeout
        );
    }

}