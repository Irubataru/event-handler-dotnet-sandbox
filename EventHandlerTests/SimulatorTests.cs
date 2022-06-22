using EventHandler;
using EventHandlerTests.Fakes.Agents;
using EventHandlerTests.Fakes.Events;

namespace EventHandlerTests;

public class SimulatorTests
{
    [Fact]
    public void StrictEventAgent_ShouldCallAgent_WhenTypedEvent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new StrictEventAgent<TestEvent>();

        sut.AddAgent(agent);

        // Act:
        var theEvent = new TestEvent();
        sut.FireEvent(theEvent);

        // Assert:
        agent.Calls.Should().BeEquivalentTo(new Dictionary<Event, int> {{theEvent, 1}});
    }

    [Fact]
    public void StrictEventAgent_ShouldNotCallAgent_WhenGenericEvent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new StrictEventAgent<TestEvent>();

        sut.AddAgent(agent);

        // Act:
        var theEvent = new Event();
        sut.FireEvent(theEvent);

        // Assert:
        agent.Calls.Should().BeEmpty();
    }

    [Fact]
    public void StrictEventAgent_ShouldNotCallAgent_WhenOtherTypedEvent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new StrictEventAgent<TestEvent>();

        sut.AddAgent(agent);

        // Act:
        var theEvent = new FakeEvent1();
        sut.FireEvent(theEvent);

        // Assert:
        agent.Calls.Should().BeEmpty();
    }
    
    [Fact]
    public void StrictEventAgent_ShouldNotCallAgent_WhenDerivedEvent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new StrictEventAgent<FakeEvent1>();

        sut.AddAgent(agent);

        // Act:
        sut.FireEvent(new FakeDerivedEvent1());

        // Assert:
        agent.Calls.Should().BeEmpty();
    }

    [Fact]
    public void UnknownEventAgent_ShouldCallAgent_WhenSpecificEvent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new UntypedEventAgent();

        sut.AddAgent(agent);

        // Act:
        var theEvent = new TestEvent();
        sut.FireEvent(theEvent);

        // Assert:
        agent.Calls.Should().BeEquivalentTo(new Dictionary<Event, int> {{theEvent, 1}});
    }

    [Fact]
    public void UnknownEventAgent_ShouldCallAgent_WhenGenericEvent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new UntypedEventAgent();

        sut.AddAgent(agent);

        // Act:
        var theEvent = new Event();
        sut.FireEvent(theEvent);

        // Assert:
        agent.Calls.Should().BeEquivalentTo(new Dictionary<Event, int> {{theEvent, 1}});
    }

    [Fact]
    public void MultipleStrictTypedImplementingAgent_ShouldCallAgentTwice_WhenBothTypedEvents()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new TwoStrictEventAgent();

        sut.AddAgent(agent);

        // Act:
        var event1 = new FakeEvent1();
        var event2 = new FakeEvent2();
        sut.FireEvent(event1);
        sut.FireEvent(event2);

        // Assert:
        agent.Calls.Should()
        .BeEquivalentTo(
            new Dictionary<Event, int>
            {
                {event1, 1},
                {event2, 1}
            });
    }
    
    [Fact]
    public void MultipleStrictTypedImplementingAgent_ShouldNotCallAgent_WhenOtherEvents()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new TwoStrictEventAgent();

        sut.AddAgent(agent);

        // Act:
        sut.FireEvent(new Event());
        sut.FireEvent(new TestEvent());

        // Assert:
        agent.Calls.Should().BeEmpty();
    }
    
    
    [Fact]
    public void EventAgent_ShouldCallAgent_WhenDerivedEvent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new EventAgent<FakeEvent1>();

        sut.AddAgent(agent);

        // Act:
        var theEvent = new FakeDerivedEvent1();
        sut.FireEvent(theEvent);

        // Assert:
        agent.Calls.Should().BeEquivalentTo(new Dictionary<Event, int> {{theEvent, 1}});
    }
    
    [Fact]
    public void EventAgents_ShouldCallAgents_WhenAddingNewDerivedAgentAfterCall()
    {
        // Arrange:
        var sut = new Simulator();
        var agent1 = new EventAgent<FakeEvent1>();
        var agent2 = new EventAgent<FakeEvent1>();

        sut.AddAgent(agent1);

        // Act:
        var event1 = new FakeDerivedEvent1();
        sut.FireEvent(event1);
        
        sut.AddAgent(agent2);
        
        var event2 = new FakeDerivedEvent1();
        sut.FireEvent(event2);

        // Assert:
        agent1.Calls.Should().BeEquivalentTo(new Dictionary<Event, int> {{event1, 1}, {event2, 1}});
        agent2.Calls.Should().BeEquivalentTo(new Dictionary<Event, int> {{event2, 1}});
    }

    [Fact]
    public void EventAgent_ShouldNotCallAgent_WhenBaseClassEvent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new EventAgent<FakeDerivedEvent1>();

        sut.AddAgent(agent);

        // Act:
        var theEvent = new FakeEvent1();
        sut.FireEvent(theEvent);

        // Assert:
        agent.Calls.Should().BeEmpty();
        
    }

    public class TestEvent : Event
    {
    }
}