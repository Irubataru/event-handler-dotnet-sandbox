using DrivingSimulation.Events;
using EventHandler;

namespace DrivingSimulation.Agents;

public class DrivingAgent : Agent, IEventHandler<DriveVehicle>, IEventHandler<StopVehicle>
{
    public void HandleEvent(DriveVehicle theEvent)
    {
        throw new NotImplementedException();
    }

    public void HandleEvent(StopVehicle theEvent)
    {
        throw new NotImplementedException();
    }
}