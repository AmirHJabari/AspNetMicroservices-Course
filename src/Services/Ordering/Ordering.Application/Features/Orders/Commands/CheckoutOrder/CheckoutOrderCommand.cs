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
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommand : IRequest<int>
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        // BillingAddress
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string AddressLine { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        // Payment
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }
    }

    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IMapper mapper, IOrderRepository repository, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            this._mapper = mapper;
            this._repository = repository;
            this._emailService = emailService;
            this._logger = logger;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request);

            var newOrder = await _repository.AddAsync(order, cancellationToken);
            await SendMail();

            return newOrder.Id;
        }

        private async Task SendMail()
        {
            var email = new Email
            {
                To = "amir@email.com",
                Subject = "Order checkedout",
                Body = "The order checkedout."
            };

            try
            {
                await _emailService.SendEmailAsync(email);

                _logger.LogInformation("Checkout email sent.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send order checkout email.");
            }
        }
    }
}
