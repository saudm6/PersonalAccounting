using Microsoft.AspNetCore.Http.HttpResults;
using PersonalAccounting.Application.CreateItems.Commands.CreateItem;
using PersonalAccounting.Application.TodoItems.Commands.CreateTodoItem;
using PersonalAccounting.Domain.Entities;

namespace PersonalAccounting.Web.Endpoints;

public class Receipt: EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateReceiptItem);
    }
    public async Task<Created<int>> CreateReceiptItem(ISender sender, CreateReceiptCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(ReceiptItem)}/{id}", id);
    }
}
