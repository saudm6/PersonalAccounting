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

// Request for getting all receipts
public record GetAllReceipts : IRequest<GetAllReceiptVm>;
public class GetAllReceiptsHandler : IRequestHandler<GetAllReceipts, GetAllReceiptVm>
{
    // Database context and mapper for handling the request
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllReceiptsHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    
    public async Task<GetAllReceiptVm> Handle(GetAllReceipts request, CancellationToken cancellationToken)
    {

        // Fetch all receipts from the database, map them to DTOs, and return them in a view model
        return new GetAllReceiptVm
        {
            Receipts = await _context.Receipts
                .AsNoTracking()
                .ProjectTo<ReceiptLookupDto>(_mapper.ConfigurationProvider)
                .OrderBy(r => r.ReceiptName)
                .ToListAsync(cancellationToken)
        };
    }
}
