using UnityEngine;

public class Crane : MonoBehaviour
{
    [SerializeField] private Transform upperCranePart;
    [SerializeField] private Transform cablePlate;
    [SerializeField] private Transform hook;
    [SerializeField] private Transform cable;

    [SerializeField] private Rigidbody cableRB;
    [SerializeField] private Rigidbody hookRB;
    [SerializeField] private Rigidbody magnetPlateRB;
    [SerializeField] private HingeJoint hookHJ;
    [SerializeField] private HingeJoint cableHJ;

    [SerializeField] private float maxCablePlatePosZ;
    [SerializeField] private float minCablePlatePosZ;
    [SerializeField] private float maxHookPosY;
    [SerializeField] private float minHookPosY;

    [SerializeField] private float cablePlateMoveSpeed;
    [SerializeField] private float hookMoveSpeed;
    [SerializeField] private float rotateSpeed;

    private void Update()
    {
        OVRInput.Update();
        // var primaryThumbX = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        // var primaryThumbY = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
        var secondaryThumbY = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;
        
        // if (primaryThumbX != 0f)
        // {
        //     RotateCrane(primaryThumbX);
        //     //cable.GetChild(0).GetComponent<Rigidbody>().AddForce(0,0, primaryThumbX, ForceMode.Impulse);
        // }
        //
        // if (primaryThumbY != 0f)
        // {
        //     MoveCablePlate(primaryThumbY);
        // }

        if (secondaryThumbY != 0f)
        {
            MoveHook(secondaryThumbY);
        }
    }


    private void RotateCrane(float speed)
    {
        upperCranePart.RotateAround(upperCranePart.transform.position, Vector3.up,
            speed * rotateSpeed * Time.deltaTime);
    }

    private void MoveCablePlate(float direction)
    {
        var pos = cablePlate.localPosition;
        pos.z -= direction * cablePlateMoveSpeed * Time.deltaTime;

        if (pos.z > maxCablePlatePosZ) pos.z = maxCablePlatePosZ;
        if (pos.z < minCablePlatePosZ) pos.z = minCablePlatePosZ;

        cablePlate.localPosition = pos;
    }

    private void MoveHook(float direction)
    {
        cableRB.isKinematic = true;
        hookRB.isKinematic = true;
        magnetPlateRB.isKinematic = true;

        var posHookHJ = hookHJ.connectedAnchor;
        var posHook = hook.position;
        var posCable = cableHJ.connectedAnchor;
        var posDelta = direction * hookMoveSpeed * Time.deltaTime;
        posHook.y -= posDelta;
        posHookHJ.y -= posDelta;
        
        // var pos = hook.localPosition;
        // pos.y -= direction * hookMoveSpeed * Time.deltaTime;

        if (posHook.y > maxHookPosY) posHook.y = maxHookPosY;
        if (posHook.y < minHookPosY) posHook.y = minHookPosY;

        hookHJ.connectedAnchor = posHookHJ;
        hook.position = posHook;
        //hook.localPosition = pos;

        var scale = 1 - posHookHJ.y / maxHookPosY;

        var cableScale = cable.localScale;
        cableScale.y = scale;
        cable.localScale = cableScale;

        posCable.y -= posDelta;
        cableHJ.connectedAnchor = posCable; 
        
        cableRB.isKinematic = false;
        hookRB.isKinematic = false;
        magnetPlateRB.isKinematic = false;
    }
}