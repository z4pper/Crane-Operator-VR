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

    public List<HookableBase> CargoList { get; } = new List<HookableBase>();
    public List<HookableBase> TargetedCargo { get; private set; } = new List<HookableBase>();
    private AudioSource _audioSource;

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
        if (position != null && position.VehicleTargetPosition == VehicleTargetPosition.Delivery)
        {
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
                var slot = CargoSlots.FirstOrDefault(cargoSlot => cargoSlot.Cargo == null);
    
                StartCoroutine(PositionCargoInSlot(hookable, slot));
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
    
    public void AttachCargoToSlot(HookableBase cargo)
    {
        CargoSlots.ForEach(slot =>
        {
            if (slot.Cargo == null)
            {
                slot.Cargo = cargo;
                
                return;
            }
        });
    }
    
    private IEnumerator PositionCargoInSlot(HookableBase cargo, CargoSlot cargoSlot)
    {
        CargoList.Add(cargo);
        cargoSlot.Cargo = cargo;
        var elapsedTime = 0f;
        cargo.transform.SetParent(transform);
        var startPos = cargo.transform.localPosition;
        var slotPosition = cargoSlot.SlotPosition;
        var distance = Vector3.Distance(startPos, slotPosition);
        while (distance > 0.1f)
        {
            elapsedTime += Time.deltaTime;
            cargo.transform.localPosition = Vector3.Lerp(startPos, slotPosition, elapsedTime / cargoLerpSeconds);
            distance = Vector3.Distance(startPos, slotPosition);
            yield return null;
        }
        cargoEventChannel.RaiseCargoLoadedEvent(this);
    }

    public void SetTargetCargoList(List<HookableBase> cargoList)
    {
        TargetedCargo = cargoList;
    }
}

[Serializable]
public struct CargoSlot
{
    [field: SerializeField] public HookableBase Cargo { get; set; }
    [field: SerializeField] public Vector3 SlotPosition { get; set; }
}
