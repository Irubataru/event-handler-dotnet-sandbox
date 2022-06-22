using EventHandler;
using EventHandlerTests.Fakes;
using EventHandlerTests.Mocks;

namespace EventHandlerTests;

public class SimulatorTests
{
    [Fact]
    public void FireEvent_ShouldCallAgent_WhenSpecificEventSpecificAgent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new SpecificEventAgent<TestEvent>();

        sut.AddAgent(agent);

        // Act:
        var theEvent = new TestEvent();
        sut.FireEvent(theEvent);

        // Assert:
        agent.Calls.Should().BeEquivalentTo(new Dictionary<Event, int> {{theEvent, 1}});
    }

    [Fact]
    public void FireEvent_ShouldNotCallAgent_WhenGenericEventSpecificAgent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new SpecificEventAgent<TestEvent>();

        sut.AddAgent(agent);

        // Act:
        var theEvent = new Event();
        sut.FireEvent(theEvent);

        // Assert:
        agent.Calls.Should().BeEmpty();
    }

    [Fact]
    public void FireEvent_ShouldNotCallAgent_WhenOtherSpecificEventSpecificAgent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new SpecificEventAgent<TestEvent>();

        sut.AddAgent(agent);

        // Act:
        var theEvent = new FakeEvent1();
        sut.FireEvent(theEvent);

        // Assert:
        agent.Calls.Should().BeEmpty();
    }

    [Fact]
    public void FireEvent_ShouldCallAgent_WhenSpecificEventGenericAgent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new GenericEventAgent();

        sut.AddAgent(agent);

        // Act:
        var theEvent = new TestEvent();
        sut.FireEvent(theEvent);

        // Assert:
        agent.Calls.Should().BeEquivalentTo(new Dictionary<Event, int> {{theEvent, 1}});
    }

    [Fact]
    public void FireEvent_ShouldCallAgent_WhenGenericEventGenericAgent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new GenericEventAgent();

        sut.AddAgent(agent);

        // Act:
        var theEvent = new Event();
        sut.FireEvent(theEvent);

        // Assert:
        agent.Calls.Should().BeEquivalentTo(new Dictionary<Event, int> {{theEvent, 1}});
    }

    [Fact]
    public void FireEvent_ShouldCallAgentTwice_WhenGenericEventDoubleSpecificAgent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new TwoSpecificEventAgent();

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
    public void FireEvent_ShouldNotCallAgent_WhenOtherEventsDoubleSpecificAgent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new TwoSpecificEventAgent();

        sut.AddAgent(agent);

        // Act:
        sut.FireEvent(new Event());
        sut.FireEvent(new TestEvent());

        // Assert:
        agent.Calls.Should().BeEmpty();
    }
    
    [Fact]
    public void FireEvent_ShouldNotCallAgent_WhenBaseEventAndDerivedAgent()
    {
        // Arrange:
        var sut = new Simulator();
        var agent = new SpecificEventAgent<FakeEvent1>();

        sut.AddAgent(agent);

        // Act:
        sut.FireEvent(new FakeDerivedEvent());

        // Assert:
        agent.Calls.Should().BeEmpty();
    }
    

    public class TestEvent : Event
    {
    }
}