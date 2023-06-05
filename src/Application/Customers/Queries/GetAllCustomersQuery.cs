using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Customers.Queries;

public class GetAllCustomersQuery : IRequest<IList<CustomerViewModel>> { }

public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IList<CustomerViewModel>>
{
    private readonly INorthwindContext _context;
    private readonly IMapper _mapper;

    #region Constructor
    public GetAllCustomersQueryHandler(INorthwindContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    #endregion

    #region IRequestHandler Implementation
    public async Task<IList<CustomerViewModel>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {   
        return await _context.Customers.ProjectTo<CustomerViewModel>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
    }
    #endregion
}
