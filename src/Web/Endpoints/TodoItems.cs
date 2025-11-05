using PersonalAccounting.Application.Common.Models;
using PersonalAccounting.Application.TodoItems.Commands.CreateTodoItem;
using PersonalAccounting.Application.TodoItems.Commands.DeleteTodoItem;
using PersonalAccounting.Application.TodoItems.Commands.UpdateTodoItem;
using PersonalAccounting.Application.TodoItems.Commands.UpdateTodoItemDetail;
using PersonalAccounting.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using Microsoft.AspNetCore.Http.HttpResults;

namespace PersonalAccounting.Web.Endpoints;

// Forwords work to application layer but from where does it take the data from????

// All APIs thay are start with /api/TodoItems are defined here
// All APIs will be grouped under /api/TodoItems because of the RouteGroupBuilder passed in
public class TodoItems : EndpointGroupBase
{
    // Build group of routes that share common URL (RouteGroupBuilder)
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(GetTodoItemsWithPagination).RequireAuthorization();
        groupBuilder.MapPost(CreateTodoItem).RequireAuthorization();
        groupBuilder.MapPut(UpdateTodoItem, "{id}").RequireAuthorization();
        groupBuilder.MapPut(UpdateTodoItemDetail, "UpdateDetail/{id}").RequireAuthorization();
        groupBuilder.MapDelete(DeleteTodoItem, "{id}").RequireAuthorization();
    }

    public async Task<Ok<PaginatedList<TodoItemBriefDto>>> GetTodoItemsWithPagination(ISender sender, [AsParameters] GetTodoItemsWithPaginationQuery query)
    {
        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public async Task<Created<int>> CreateTodoItem(ISender sender, CreateTodoItemCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(TodoItems)}/{id}", id);
    }

    public async Task<Results<NoContent, BadRequest>> UpdateTodoItem(ISender sender, int id, UpdateTodoItemCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public async Task<Results<NoContent, BadRequest>> UpdateTodoItemDetail(ISender sender, int id, UpdateTodoItemDetailCommand command)
    {
        if (id != command.Id) return TypedResults.BadRequest();

        await sender.Send(command);

        return TypedResults.NoContent();
    }

    public async Task<NoContent> DeleteTodoItem(ISender sender, int id)
    {
        await sender.Send(new DeleteTodoItemCommand(id));

        return TypedResults.NoContent();
    }
}
