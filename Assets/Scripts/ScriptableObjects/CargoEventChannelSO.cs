using System;
using UnityEngine;


[CreateAssetMenu(fileName = "new Cargo Event Channel", menuName = "Channel/Cargo", order = 0)]
public class CargoEventChannelSO : ScriptableObject
{
    public event Action<VehicleController> OnCargoUnload;
    public event Action<VehicleController> OnCargoLoad;

    public void RaiseCargoUnloadedEvent(VehicleController vehicleController)
    {
        OnCargoUnload?.Invoke(vehicleController);
    }  
    
    public void RaiseCargoLoadedEvent(VehicleController vehicleController)
    {
        OnCargoLoad?.Invoke(vehicleController);
    }
}
