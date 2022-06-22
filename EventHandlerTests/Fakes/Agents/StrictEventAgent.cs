using EventHandler;

namespace EventHandlerTests.Fakes.Agents;

public class StrictEventAgent<TEvent> : FakeAgent, IStrictEventHandler<TEvent> where TEvent : Event
{
    public void HandleEvent(TEvent theEvent)
    {
        AddCall(theEvent);
    }
}