namespace MessageBus.Producer;
using Amazon.SQS;
using Amazon.SQS.Model;
using FraudPreventionV2.Infra.MessageBus.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace FraudPreventionV2.Infra.MessageBus;

public interface IMessageBusEvent {
  string Message { get; }
}
public interface IProducer {
  Task Send(IMessageBusEvent iMessageBusEvent, CancellationToken cancellationToken = default);
}
public interface IConsumer<T> where T : IMessageBusEvent {
  Task ConsumeAsync(CancellationToken cancellationToken = default);
}
public abstract class Consumer<T> : IConsumer<T> where T : IMessageBusEvent {
  public virtual Task ConsumeAsync(CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }
}
public abstract class Producer<T> : IProducer where T : IMessageBusEvent {
  public virtual Task Send(IMessageBusEvent iMessageBusEvent, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }
}
public record CreatedIMessageBusEvent(string Message) : IMessageBusEvent;
public record DeletedIMessageBusEvent(string Message) : IMessageBusEvent;
public class CreatedEventConsumer(IAmazonSQS amazonSqs) : Consumer<CreatedIMessageBusEvent> {
}
public class DeletedEventProducer(IAmazonSQS amazonSqs)  : Producer<CreatedIMessageBusEvent> {
  public override Task Send(IMessageBusEvent iMessageBusEvent, CancellationToken cancellationToken = default) {
    throw new NotImplementedException();
  }
}


// public class CreatedEventProducer(IAmazonSQS amazonSqs) : Producer<CreatedTransactionEvent>  {
//   public async Task Send(CreatedTransactionEvent @event, CancellationToken cancellationToken = default) {
//     var request = new SendMessageRequest {
//       // MessageBody = @event.Message,
//     };
//     await amazonSqs.SendMessageAsync(@request, cancellationToken);
//   }
// }


public static class MessageBusDependenciesExtensions {
  public static void AddMessageBus(this IServiceCollection services) {
    services.AddProducers<CreatedIMessageBusEvent>();
    services.AddConsumer<CreatedIMessageBusEvent>();
    services.AddProducers<DeletedIMessageBusEvent>();
    services.AddConsumer<DeletedIMessageBusEvent>();
  }

  public static void AddProducers<TEvent>(this IServiceCollection services)
    where TEvent : IMessageBusEvent{
    services.AddTransient(typeof(IMessageBusEvent), typeof(TEvent));
    services.AddSingleton(typeof(IProducer), typeof(Producer<TEvent>));
  }
  private static void AddConsumer<T>(this IServiceCollection services)
    where T : IMessageBusEvent {
    services.AddSingleton<IConsumer<T>, Consumer<T>>();
  }
