using EventHandler;
using EventHandlerTests.Fakes.Events;

namespace EventHandlerTests.Fakes.Agents;

public class TwoStrictEventAgent : FakeAgent, IStrictEventHandler<FakeEvent1>, IStrictEventHandler<FakeEvent2>
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