using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    [SerializeField] private List<CargoSlot> cargoSlots;
    [SerializeField] private float cargoLerpSeconds;
    [SerializeField] private CargoEventChannelSO cargoEventChannel;

    public List<HookableBase> CargoList { get; private set; } = new List<HookableBase>();
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        cargoSlots.ForEach(slot =>
        {
            CargoList.Add(slot.GetCargoSlot().GetComponent<HookableBase>());
        });
    }

    private void OnTriggerExit(Collider other)
    {
        var cargo = other.GetComponent<HookableBase>();
        if (cargo != null)
        {
            var isTaskObject = false;
            cargoSlots.ForEach(slot =>
            {
                if (slot.GetCargoSlot() == cargo.gameObject)
                {
                    isTaskObject = true;
                    return;
                }
            });

            if (!isTaskObject) return;

            RemoveCargoFromSlot(cargo.gameObject);
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

    private void RemoveCargoFromSlot(GameObject cargo)
    {
        cargoSlots.ForEach(slot =>
        {
            if(slot.GetCargoSlot() == cargo) slot.SetCargoSlot(cargo);
        });
    }

    public void AttachCargoToSlot(GameObject cargo)
    {
        cargoSlots.ForEach(slot =>
        {
            if (slot.GetCargoSlot() == null)
            {
                slot.SetCargoSlot(cargo);
                
                return;
            }
        });
    }

    private IEnumerator PositionCargoInSlot(Transform cargo, Vector3 slotPosition)
    {
        var elapsedTime = 0f;
        var startPos = cargo.localPosition;
        var distance = Vector3.Distance(startPos, slotPosition);
        while (distance > 0.1f)
        {
            elapsedTime += Time.deltaTime;
            cargo.localPosition = Vector3.Lerp(startPos, slotPosition, elapsedTime / cargoLerpSeconds);
            distance = Vector3.Distance(startPos, slotPosition);
            yield return null;
        }
    }
}

[Serializable]
public struct CargoSlot
{
   [SerializeField] private GameObject cargoSlot;
   [SerializeField] private Vector3 slotPosition;

   public GameObject GetCargoSlot()
   {
       return cargoSlot;
   }

   public void SetCargoSlot(GameObject cargo)
   {
       this.cargoSlot = cargo;
   }
}
