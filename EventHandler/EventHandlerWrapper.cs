namespace EventHandler;

internal abstract class EventHandlerWrapper
{
    public abstract void AddHandler(object handler);
    
    public abstract bool HasHandler(object handler);

    public abstract void HandleEvent(Event theEvent);
}

internal class SpecificEventHandlerWrapper<TEvent> : EventHandlerWrapper where TEvent : Event
{
    private List<IEventHandler<TEvent>> _handlers = new();
    

    public override void AddHandler(object handler)
    {
        if (handler is not IEventHandler<TEvent> specificHandler)
            throw new ArgumentException($"The {nameof(handler)} does not implement {typeof(IEventHandler<TEvent>)}");
        
        _handlers.Add(specificHandler);
    }

    public override bool HasHandler(object handler)
    {
        return handler is IEventHandler<TEvent> specificHandler && _handlers.Contains(specificHandler);
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

internal class GenericEventHandlerWrapper : EventHandlerWrapper
{
    private List<IEventHandler> _handlers = new();

    public override void AddHandler(object handler)
    {
        if (handler is not IEventHandler specificHandler)
            throw new ArgumentException($"The {nameof(handler)} does not implement {typeof(IEventHandler)}");
        
        _handlers.Add(specificHandler);
    }

    public override bool HasHandler(object handler)
    {
        return handler is IEventHandler specificHandler && _handlers.Contains(specificHandler);
    }

    public override void HandleEvent(Event theEvent)
    {
        foreach (var handler in _handlers)
        {
            handler.HandleEvent(theEvent);
        }
    }
}
