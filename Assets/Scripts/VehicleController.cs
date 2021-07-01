using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [field: SerializeField] public List<CargoSlot> CargoSlots { get; set; }
    [SerializeField] private float cargoLerpSeconds;
    [SerializeField] private CargoEventChannelSO cargoEventChannel;
    [SerializeField] private VehicleEventChannelSO vehicleEventChannel;

    public List<HookableBase> CargoList { get; } = new List<HookableBase>();
    public List<HookableBase> TargetedCargo { get; private set; } = new List<HookableBase>();
    
    private AudioSource _audioSource;
    // TODO: find better way
    private bool _isAtDestination;
    

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        CargoSlots.ForEach(slot =>
        {
            if (slot.Cargo != null)
            {
                CargoList.Add(slot.Cargo.GetComponent<HookableBase>());
            }
        });
    }
    
    private void OnTriggerExit(Collider other)
    {
        var cargo = other.GetComponent<HookableBase>();
        if (cargo != null)
        {
            var isTaskObject = false;
            CargoSlots.ForEach(slot =>
            {
                if (slot.Cargo == cargo.gameObject)
                {
                    isTaskObject = true;
                    return;
                }
            });
    
            if (!isTaskObject) return;
    
            RemoveCargoFromSlot(cargo);
            //cargoEventChannel.RaiseCargoUnloadedEvent(this);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var position = other.gameObject.GetComponent<VehiclePosition>();
        if (position != null && position.VehicleTargetPosition == VehicleTargetPosition.Delivery && !_isAtDestination)
        {
            _isAtDestination = true;
            vehicleEventChannel.RaiseVehicleArrivedAtDeliveryZoneEvent(this);
            _audioSource.PlayOneShot(_audioSource.clip);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        var hookable = other.gameObject.GetComponent<HookableBase>();
        if (hookable != null && TargetedCargo.Contains(hookable))
        {
            if (!CargoList.Contains(hookable) && !hookable.IsHooked)
            {
                StartCoroutine(PositionCargoInSlot(hookable));
            }
        }
    }
    
    private void RemoveCargoFromSlot(HookableBase cargo)
    {
        CargoSlots.ForEach(slot =>
        {
            if (slot.Cargo == cargo) slot.Cargo = cargo;
        });
    }
    
    private CargoSlot AttachCargoToSlot(HookableBase cargo)
    {
        var slot = CargoSlots.FirstOrDefault(cargoSlot => cargoSlot.Cargo == null);
        slot.Cargo = cargo;
        CargoList.Add(cargo);

        cargo.GetComponent<Rigidbody>().isKinematic = true;
        cargo.transform.SetParent(transform);
        
        return slot;
    }
    
    private IEnumerator PositionCargoInSlot(HookableBase cargo)
    {
        var cargoSlot = AttachCargoToSlot(cargo);
        
        var elapsedTime = 0f;
        var startPos = cargo.transform.localPosition;
        var startRotation = cargo.transform.localRotation;
        var slotPosition = cargoSlot.SlotPosition;
        var slotRotation = Quaternion.Euler(cargoSlot.SlotRotation);
        
        while (elapsedTime < cargoLerpSeconds)
        {
            elapsedTime += Time.deltaTime;
            cargo.transform.localPosition = Vector3.Lerp(startPos, slotPosition, elapsedTime / cargoLerpSeconds);
            cargo.transform.localRotation = Quaternion.Lerp(startRotation, slotRotation, elapsedTime / cargoLerpSeconds);
            yield return null;
        }
        
        cargoEventChannel.RaiseCargoLoadedEvent(this);
        cargo.UnmarkOutline();
        Destroy(cargo);
    }

    public void SetTargetCargoList(List<HookableBase> cargoList)
    {
        TargetedCargo = cargoList;
    }
}
