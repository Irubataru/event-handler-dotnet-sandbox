namespace EventHandler;

/// <summary>
///     Type tag for event handlers
/// </summary>
public interface IEventHandler
{
}

/// <summary>
///     Can handle events of any type
/// </summary>
public interface IUntypedEventHandler : IEventHandler
{
    void HandleEvent(Event theEvent);
}

/// <summary>
///     Can handle events of a specific type <typeparamref name="TEvent"/>, not including derived types
/// </summary>
/// <remarks>
///     If you do not need the ability to handle derived types then using this is more performant than using
///     <see cref="IEventHandler{TEvent}" />
/// </remarks>
public interface IStrictEventHandler<in TEvent> : IEventHandler where TEvent : Event
{
    void HandleEvent(TEvent theEvent);
}

/// <summary>
///     Can handle events of type <typeparamref name="TEvent"/> and all derived event types
/// </summary>
public interface IEventHandler<in TEvent> : IStrictEventHandler<TEvent> where TEvent : Event
{
}