using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new boolean", menuName = "Variable/Boolean")]
public class BooleanSO : ScriptableObject, ISerializationCallbackReceiver
{
    public bool InitialValue;

    [NonSerialized] public bool Value;

    public void OnAfterDeserialize()
    {
        Value = InitialValue;
    }
    
    public void OnBeforeSerialize() { }
}
