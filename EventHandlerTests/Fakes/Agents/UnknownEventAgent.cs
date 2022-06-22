using EventHandler;

namespace EventHandlerTests.Fakes.Agents;

public class UntypedEventAgent : FakeAgent, IUntypedEventHandler
{
    public void HandleEvent(Event theEvent)
    {
        AddCall(theEvent);
    }
}