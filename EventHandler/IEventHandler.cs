namespace EventHandler;

public interface IEventHandler
{
    void HandleEvent(Event theEvent);
}

public interface IEventHandler<in T> where T : Event
{
    void HandleEvent(T theEvent);
}
