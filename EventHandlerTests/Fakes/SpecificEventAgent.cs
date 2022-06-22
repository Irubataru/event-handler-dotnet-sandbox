using EventHandler;
using EventHandlerTests.Fakes;

namespace EventHandlerTests.Mocks;

public class SpecificEventAgent<TEvent> : FakeAgent, IEventHandler<TEvent> where TEvent : Event
{
    public void HandleEvent(TEvent theEvent)
    {
        AddCall(theEvent);
    }
}