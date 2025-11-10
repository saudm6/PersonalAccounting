using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using PersonalAccounting.Application.Common.Interfaces;
using PersonalAccounting.Application.Common.Models;
using PersonalAccounting.Application.Common.Security;
using PersonalAccounting.Application.CreateItems.Dto;
using PersonalAccounting.Application.TodoLists.Queries.GetTodos;
using PersonalAccounting.Domain.Enums;

namespace PersonalAccounting.Application.CreateItems.Queries;


[Authorize]
public record GetAllReceipts : IRequest<GetAllReceiptVm>;
public class GetAllReceiptsHandler : IRequestHandler<GetAllReceipts, GetAllReceiptVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllReceiptsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetAllReceiptVm> Handle(GetAllReceipts request, CancellationToken cancellationToken)
    {
        var receipts = await _context.Receipts
            .AsNoTracking()
            .ProjectTo<ReceiptLookupDto>(_mapper.ConfigurationProvider)
            .OrderBy(r => r.ReceiptName)
            .ToListAsync(cancellationToken);
        return new GetAllReceiptVm
        {
            Receipts = receipts
        };
    }
}
