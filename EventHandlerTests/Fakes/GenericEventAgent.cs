using EventHandler;

namespace EventHandlerTests.Fakes;

public class GenericEventAgent : FakeAgent, IEventHandler
{
    public void HandleEvent(Event theEvent)
    {
        AddCall(theEvent);
    }
}