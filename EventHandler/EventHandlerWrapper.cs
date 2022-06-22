namespace EventHandler;

internal abstract class EventHandlerWrapper : IUntypedEventHandler
{
    public abstract bool AddHandler(IEventHandler handler);
    
    public abstract bool HasHandler(IEventHandler handler);

    public abstract void HandleEvent(Event theEvent);
}

internal class EventHandlerWrapper<TEvent> : EventHandlerWrapper where TEvent : Event
{
    private readonly List<IStrictEventHandler<TEvent>> _handlers = new();

    public override bool AddHandler(IEventHandler handler)
    {
        if (handler is not IStrictEventHandler<TEvent> specificHandler)
            throw new ArgumentException($"The {nameof(handler)} does not implement {typeof(IStrictEventHandler<TEvent>)}");

        if (_handlers.Contains(handler)) return false;
        
        _handlers.Add(specificHandler);
        return true;
    }

    public override bool HasHandler(IEventHandler handler)
    {
        return handler is IStrictEventHandler<TEvent> specificHandler && _handlers.Contains(specificHandler);
    }

    public override void HandleEvent(Event theEvent)
    {
        if (theEvent is not TEvent specificEvent)
            throw new ArgumentException($"The event does not have the same type as the handlers ({typeof(TEvent)})");

        foreach (var handler in _handlers)
        {
            handler.HandleEvent(specificEvent);
        }
    }
}

internal class UntypedEventHandlerWrapper : EventHandlerWrapper
{
    private readonly List<IUntypedEventHandler> _handlers = new();

    public override bool AddHandler(IEventHandler handler)
    {
        if (handler is not IUntypedEventHandler specificHandler)
            throw new ArgumentException($"The {nameof(handler)} does not implement {typeof(IUntypedEventHandler)}");
        
        if (_handlers.Contains(handler)) return false;
        
        _handlers.Add(specificHandler);
        return true;
    }

    public override bool HasHandler(IEventHandler handler)
    {
        return handler is IUntypedEventHandler specificHandler && _handlers.Contains(specificHandler);
    }

    public override void HandleEvent(Event theEvent)
    {
        foreach (var handler in _handlers)
        {
            handler.HandleEvent(theEvent);
        }
    }
}
