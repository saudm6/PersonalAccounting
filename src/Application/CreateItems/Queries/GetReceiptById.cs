using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using PersonalAccounting.Application.Common.Interfaces;
using PersonalAccounting.Application.CreateItems.Dto;
using PersonalAccounting.Domain.Entities;

namespace PersonalAccounting.Application.CreateItems.Queries;

// Id request for getting receipts Id
public record GetReceiptById : IRequest<GetReceiptByIdVm>
{
    public int Id { get; init; }
}

public class GetReceiptByIdHandler : IRequestHandler<GetReceiptById, GetReceiptByIdVm>
{
    // Database context and mapper for handling the request
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetReceiptByIdHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // Create and return the GetReceiptByIdVm object
    public async Task<GetReceiptByIdVm> Handle(GetReceiptById request, CancellationToken cancellationToken)
    {
        return new GetReceiptByIdVm
        {
            Receipts = await _context.Receipts
                .AsNoTracking()
                .Where(r => r.Id == request.Id)
                .ProjectTo<ReceiptLookupDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
        };
    }
}
