using MediatR;
using Order.Infrastructure.DataAccess.EntityFramework;
using Order.Services.DTO.Request;
using Order.Services.DTO.Response;
using Saga.Choreography.Core.Enums;
using Saga.Choreography.Core.Mappings.Abstract;
using Saga.Choreography.Core.MessageBrokers.Abstract;
using Saga.Choreography.Shared.MessageBrokers.Consumers.Models.Stock;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Services.Commands
{
    public class CreateOrderCommand : IRequest<CreateOrderResponseModel>
    {
        public CreateOrderRequestModel CreateOrderRequest { get; set; }

        public CreateOrderCommand(CreateOrderRequestModel createOrderRequest)
        {
            CreateOrderRequest = createOrderRequest;
        }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponseModel>
    {
        private readonly OrderDbContext _dbContext;
        private readonly IMapping _mapping;
        private readonly IEventBus _eventBus;

        public CreateOrderCommandHandler(OrderDbContext dbContext,
            IMapping mapping,
            IEventBus eventBus)
        {
            _dbContext = dbContext;
            _mapping = mapping;
            _eventBus = eventBus;
        }

        public async Task<CreateOrderResponseModel> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _mapping.Map<CreateOrderRequestModel, Infrastructure.Entities.Order>(request.CreateOrderRequest);
            order.OrderStatus = (int)OrderStatus.Pending;

            var response = await _dbContext.AddAsync(order);

            if (await _dbContext.SaveChangesAsync() > 0)
            {
                await _eventBus.Publish(new UpdateStockEvent
                {
                    CorrelationId = Guid.NewGuid(),
                    ProductId = order.ProductId,
                    Quantity = order.Quantity,
                    OrderId = order.Id
                });
            }

            return _mapping.Map<Infrastructure.Entities.Order, CreateOrderResponseModel>(response.Entity);
        }
    }
}
