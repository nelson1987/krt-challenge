namespace MessageBus.Producer;
public interface IProducer {}
public interface IConsumer {}
public interface IEvent {}

public record Event : IEvent {}
public class Producer : IProducer {}
public class Consumer : IConsumer {}

public class Handler{
public void Handler(){
}
}