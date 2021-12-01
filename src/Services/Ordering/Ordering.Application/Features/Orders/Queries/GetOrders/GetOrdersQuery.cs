using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using System.Threading;
using AutoMapper;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrders
{
    public class GetOrdersQuery : IRequest<IEnumerable<OrderDto>>
    {
        public string UserName { get; set; }

        public GetOrdersQuery(string userName)
        {
            this.UserName = userName;
        }
    }

    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;

        public GetOrdersQueryHandler(IOrderRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repository.GetOrdersByUserNameAsync(request.UserName, cancellationToken);

            return _mapper.Map<List<OrderDto>>(orders);
        }
    }
}
