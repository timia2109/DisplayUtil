using DisplayUtil.Building;

namespace DisplayUtil.Infrastructure.Providers;

public sealed class DefaultDefinitionProvider(
    DefaultDefinition definition
)
{
    public DefaultDefinition Definition => definition;
}