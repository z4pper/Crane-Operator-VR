using System;
using UnityEngine;

[Serializable]
public class CargoSlot
{
    [field: SerializeField] public HookableBase Cargo { get; set; }
    [field: SerializeField] public Vector3 SlotPosition { get; set; }
    [field: SerializeField] public Vector3 SlotRotation { get; set; }
}
