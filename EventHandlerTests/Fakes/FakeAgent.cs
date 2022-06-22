using EventHandler;

namespace EventHandlerTests.Fakes;

public class FakeAgent : Agent
{
    public Dictionary<Event, int> Calls { get; } = new();

    protected void AddCall(Event theEvent)
    {
        var timesCalled = Calls.GetOrAdd(theEvent, _ => 0);
        Calls[theEvent] = timesCalled + 1;
    }
}