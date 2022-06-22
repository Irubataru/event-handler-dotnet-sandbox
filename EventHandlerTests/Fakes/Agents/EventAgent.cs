using EventHandler;

namespace EventHandlerTests.Fakes.Agents;

public class EventAgent<TEvent> : FakeAgent, IEventHandler<TEvent> where TEvent : Event
{
    public void HandleEvent(TEvent theEvent)
    {
        AddCall(theEvent);
    }
}