namespace PersonalAccounting.Web.Infrastructure;

// Cant inherit from classes but other classes can inherit from it (meaning of abstract class)
public abstract class EndpointGroupBase
{
    // Can be overridden by child class if not it will default to null
    public virtual string? GroupName { get; }

    // Child classes *must* implement it
    // Framework will pass in a RouteGroupBuilder that points to a base URL like /api/{GroupName}.
    public abstract void Map(RouteGroupBuilder groupBuilder);
}
