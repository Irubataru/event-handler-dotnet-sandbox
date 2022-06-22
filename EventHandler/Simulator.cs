namespace EventHandler;

public class Simulator
{
    private readonly List<Agent> _agents = new();
    private readonly HashSet<Type> _checkedEventTypes = new();

    /// <summary>
    ///     Event handlers that handle one specific event (no inheritance checking)
    /// </summary>
    private readonly Dictionary<Type, EventHandlerWrapper> _handlers = new();

    /// <summary>
    ///     Event handlers that handle every type of event
    /// </summary>
    private readonly UntypedEventHandlerWrapper _untypedHandlers = new();

    /// <summary>
    ///     Event handlers that handle an event and all derived events
    /// </summary>
    private readonly Dictionary<Type, List<IEventHandler>> _partiallyUntypedHandlers = new();

    public void AddAgent(Agent agent)
    {
        _agents.Add(agent);

        AddStrictEventHandlers(agent);
        AddPartiallyUntypedEventHandlers(agent);
        AddUntypedEventHandlers(agent);
    }

    public void FireEvent(Event theEvent)
    {
        var eventType = theEvent.GetType();

        if (_partiallyUntypedHandlers.Any() && !_checkedEventTypes.Contains(eventType))
            UpdatePartiallyUntypedEventHandlers(eventType);

        if (_handlers.ContainsKey(eventType))
        {
            var handler = _handlers[eventType];
            handler.HandleEvent(theEvent);
        }

        _untypedHandlers.HandleEvent(theEvent);
    }

    /// <summary>
    ///     Register every specific event the agent is able to process
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    private void AddStrictEventHandlers(IEventHandler agent)
    {
        var eventTypes = agent.GetType()
            .GetInterfaces()
            .Where(
                t => t.GenericTypeArguments.Length == 1 && typeof(Event).IsAssignableFrom(t.GenericTypeArguments[0]) &&
                     t == typeof(IStrictEventHandler<>).MakeGenericType(t.GenericTypeArguments[0]))
            .Select(t => t.GenericTypeArguments[0]);

        foreach (var type in eventTypes)
        {
            var handler = _handlers.GetOrAdd(
                type,
                t => (EventHandlerWrapper)
                    (Activator.CreateInstance(typeof(EventHandlerWrapper<>).MakeGenericType(t)) ??
                     throw new InvalidOperationException($"Could not create wrapper for type {t}")));

            handler.AddHandler(agent);
        }
    }

    /// <summary>
    ///     Register every base class event the agent is able to handle
    /// </summary>
    private void AddPartiallyUntypedEventHandlers(IEventHandler agent)
    {
        var eventTypes = agent.GetType()
            .GetInterfaces()
            .Where(
                static t => t.GenericTypeArguments.Length == 1 &&
                            typeof(Event).IsAssignableFrom(t.GenericTypeArguments[0]) &&
                            t == typeof(IEventHandler<>).MakeGenericType(t.GenericTypeArguments[0]))
            .Select(t => t.GenericTypeArguments[0]);

        foreach (var type in eventTypes)
        {
            var handlers = _partiallyUntypedHandlers.GetOrAdd(type, _ => new());
            handlers.Add(agent);

            foreach (var checkedType in _checkedEventTypes)
            {
                if (!type.IsAssignableFrom(checkedType))
                    continue;

                _checkedEventTypes.Remove(checkedType);
            }
        }
    }

    /// <summary>
    ///     Register every untyped event the agent is able to handle
    /// </summary>
    private void AddUntypedEventHandlers(IEventHandler agent)
    {
        if (agent.GetType().GetInterface(nameof(IUntypedEventHandler)) is not null)
            _untypedHandlers.AddHandler(agent);
    }

    /// <summary>
    ///     Update the cache of specific event handlers in the case that the <paramref name="eventType" /> can be handled by
    ///     any of the event handlers in the list of derivable event handlers
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    private void UpdatePartiallyUntypedEventHandlers(Type eventType)
    {
        foreach (var (type, handlers) in _partiallyUntypedHandlers)
        {
            if (!type.IsAssignableFrom(eventType))
                continue;

            var specificHandler = _handlers.GetOrAdd(
                eventType,
                static t =>
                    (EventHandlerWrapper) (Activator.CreateInstance(typeof(EventHandlerWrapper<>).MakeGenericType(t)) ??
                                           throw new InvalidOperationException(
                                               $"Could not create wrapper for type {t}")));

            foreach (var handler in handlers)
                specificHandler.AddHandler(handler);
        }

        _checkedEventTypes.Add(eventType);
    }
}