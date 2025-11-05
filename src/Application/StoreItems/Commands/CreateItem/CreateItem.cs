using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalAccounting.Application.StoreItems.Commands.CreateItem;

public class CreateItem: IRequest<int>
{
    int ItemId { get; init; }
    public string? ItemName { get; init; }

    public string? ItemDescription { get; init; }

    public decimal ItemPrice { get; init; }





}
