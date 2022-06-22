using EventHandler;

namespace EventHandlerTests.Fakes;

public class TwoSpecificEventAgent : FakeAgent, IEventHandler<FakeEvent1>, IEventHandler<FakeEvent2>
{
    public void HandleEvent(FakeEvent1 theEvent)
    {
        AddCall(theEvent);
    }

    public void HandleEvent(FakeEvent2 theEvent)
    {
        AddCall(theEvent);
    }
}