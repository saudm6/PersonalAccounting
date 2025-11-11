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

//public record GetReceiptById : IRequest<GetReceiptByIdVm>;
//public class GetReceiptByIdHandler : IRequestHandler<GetReceiptById, GetReceiptByIdVm>
//{
//    private readonly IApplicationDbContext _context;
//    private readonly IMapper _mapper;
//    public GetReceiptByIdHandler(IApplicationDbContext context, IMapper mapper)
//    {
//        _context = context;
//        _mapper = mapper;
//    }

//    public async Task<GetReceiptByIdVm> Handle(GetReceiptById request, CancellationToken cancellationToken)
//    {


//        var receipt = await _context.Receipts
//            .AsNoTracking()
//            .Where(r => r.Id == request.Id)
//            .ProjectTo<ReceiptLookupDto>(_mapper.ConfigurationProvider)
//            .FirstOrDefaultAsync(cancellationToken);
//        if (receipt == null)
//        {
//            throw new NotFoundException(nameof(Receipt), request.Id);
//        }
//        return new GetReceiptByIdVm
//        {
//            Id = Guid.NewGuid(),
//            Receipts = new List<ReceiptLookupDto> { receipt }
//        };
//    }
//}
