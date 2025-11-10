using Microsoft.AspNetCore.Http.HttpResults;
using PersonalAccounting.Application.CreateItems.Commands.CreateItem;
using PersonalAccounting.Application.CreateItems.Queries;
using PersonalAccounting.Application.TodoItems.Commands.CreateTodoItem;
using PersonalAccounting.Domain.Entities;

namespace PersonalAccounting.Web.Endpoints;

public class Receipt: EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateReceiptItem);
        groupBuilder.MapGet(GetAllReceipts);
    }
    public async Task<Created<int>> CreateReceiptItem(ISender sender, CreateReceiptCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(ReceiptItem)}/{id}", id);
    }

    public async Task<Ok<GetAllReceiptVm>> GetAllReceipts(ISender sender)
    {
        var vm = await sender.Send(new GetAllReceipts());
        return TypedResults.Ok(vm);
    }
}
