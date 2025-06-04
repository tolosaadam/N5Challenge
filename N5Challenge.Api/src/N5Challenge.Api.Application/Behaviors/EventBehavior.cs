using MediatR;
using N5Challenge.Api.Application.Interfaces.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N5Challenge.Api.Application.Behaviors;

public class EventBehavior<TRequest, TResponse>(IKafkaProducer producer) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IPublishEvent
{
    private readonly IKafkaProducer _producer = producer;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await _producer.SendMessageAsync(request.Topic, request.Operation, cancellationToken);

        return await next(cancellationToken);
    }
}
