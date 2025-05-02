namespace MessageBus.Producer;
public interface IProducer {}
public interface IConsumer {}
public interface IEvent {}

public record Event : IEvent {}
public class Producer : IProducer {
public void SendAsync();
}
public class Consumer : IConsumer {}

public class Handler(IProducer producer){
public void Handler(){
producer.SendAsync();
}
}