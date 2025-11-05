using System.Reflection;

namespace PersonalAccounting.Web.Infrastructure;

// Web application extension 1: Create a route group for an endpoint group, Grouping by common URL prefix
public static class WebApplicationExtensions
{
    // Create a route group for an endpoint group
    private static RouteGroupBuilder MapGroup(this WebApplication app, EndpointGroupBase group)
    {
        // Use GroupName if provided; otherwise, use the class name as GroupName
        // group.GetType().Name = Classs Name
        var groupName = group.GroupName ?? group.GetType().Name;

        return app
            .MapGroup($"/api/{groupName}")
            .WithGroupName(groupName)
            .WithTags(groupName);
    }

    // Web application extension 2: Map all endpoint groups
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        // Get all types derived from EndpointGroupBase
        var endpointGroupType = typeof(EndpointGroupBase);

        // Return current project in assembly
        var assembly = Assembly.GetExecutingAssembly();

        // Filter types that are subclasses of EndpointGroupBase (inherit from EndpointGroupBase)
        var endpointGroupTypes = assembly.GetExportedTypes()
            .Where(t => t.IsSubclassOf(endpointGroupType));

        foreach (var type in endpointGroupTypes)
        {
            if (Activator.CreateInstance(type) is EndpointGroupBase instance)
            {
                // Creates a base route and provides you with RouteGroupBuilder
                instance.Map(app.MapGroup(instance));
            }
        }

        return app;
    }
}
