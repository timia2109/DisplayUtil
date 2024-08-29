namespace DisplayUtil.Scripting.PropertyObjects.Mapping;

public sealed class MappingRegistry
{
    private readonly Dictionary<Type, IMapping> _mappings = [];

    public MappingFactory MappingFactory { get; }

    public MappingRegistry()
    {
        MappingFactory = new MappingFactory(this);
    }

    public IMapping RegisterType<TType>()
    {
        return RegisterType(typeof(TType));
    }

    public IMapping RegisterType(Type type)
    {
        if (_mappings.TryGetValue(type, out var mapping))
        {
            return mapping;
        }

        mapping = MappingFactory.ForObject(type);
        _mappings[type] = mapping;
        return mapping;
    }
}