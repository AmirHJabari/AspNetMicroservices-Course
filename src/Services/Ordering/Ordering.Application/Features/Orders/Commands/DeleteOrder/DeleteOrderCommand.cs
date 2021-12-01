using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommand : IRequest
    {
        public int Id { get; set; }

        public DeleteOrderCommand(int id)
        {
            this.Id = id;
        }
    }

    public class DeleteOrderCommandHandle : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<DeleteOrderCommandHandle> _logger;

        public DeleteOrderCommandHandle(IOrderRepository repository, ILogger<DeleteOrderCommandHandle> logger)
        {
            this._repository = repository;
            this._logger = logger;
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteByIdAsync(request.Id);

            return Unit.Value;
        }
    }
}
