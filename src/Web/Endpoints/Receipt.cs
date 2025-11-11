using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
        groupBuilder.MapGet(GetReceiptById, "{id}");
    }

    // POST api/receipt
    public async Task<Created<int>> CreateReceiptItem(ISender sender, CreateReceiptCommand command)
    {
        var id = await sender.Send(command);

        return TypedResults.Created($"/{nameof(ReceiptItem)}/{id}", id);
    }

    // GET api/receipt
    public async Task<Ok<GetAllReceiptVm>> GetAllReceipts(ISender sender)
    {
        var vm = await sender.Send(new GetAllReceipts());
        return TypedResults.Ok(vm);
    }

    // GET api/receipt/{id}
    public async Task<Ok<GetReceiptByIdVm>> GetReceiptById(ISender sender, int id, CancellationToken cancellationToken)
    {
        var vm = await sender.Send(new GetReceiptById { Id = id}, cancellationToken);

        return TypedResults.Ok(vm);
    }
}
