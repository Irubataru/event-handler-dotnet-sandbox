namespace EventHandler;

public class Simulator
{
    private List<Agent> _agents = new();

    private Dictionary<Type, EventHandlerWrapper> _specificHandlers = new();

    private GenericEventHandlerWrapper _genericHandlers = new();

    public void AddAgent(Agent agent)
    {
        _agents.Add(agent);

        var eventTypes = agent.GetType()
            .GetInterfaces()
            .Where(
                t => t.GenericTypeArguments.Length == 1 &&
                     typeof(Event).IsAssignableFrom(t.GenericTypeArguments[0]) &&
                     t == typeof(IEventHandler<>).MakeGenericType(t.GenericTypeArguments[0]))
            .Select(t => t.GenericTypeArguments[0]);

        foreach (var type in eventTypes)
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(type);
            var handler = _specificHandlers.GetOrAdd(
                type,
                t => (EventHandlerWrapper) (Activator.CreateInstance(
                                                typeof(SpecificEventHandlerWrapper<>).MakeGenericType(type)) ??
                                            throw new InvalidOperationException(
                                                $"Could not create wrapper for type {t}")));
            
            handler.AddHandler(agent);
        }

        if (agent.GetType().GetInterface(nameof(IEventHandler)) is not null)
            _genericHandlers.AddHandler(agent);
    }

    public void FireEvent(Event theEvent)
    {
        var eventType = theEvent.GetType();

        if (_specificHandlers.ContainsKey(eventType))
        {
            var handler = _specificHandlers[eventType];
            handler.HandleEvent(theEvent);
        }
        
        _genericHandlers.HandleEvent(theEvent);
    }
}