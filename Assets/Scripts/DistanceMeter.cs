using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DistanceMeter : MonoBehaviour
{
    [SerializeField] private int frameInterval;
    [SerializeField] private Color distanceMeterTextColor;
    [SerializeField] private BoxCollider boxCollider;
    private Vector3 _rayStartingPosition;
    private TextMeshProUGUI _distanceInMeterText;

    public void SetupDistanceMeterText(TextMeshProUGUI text)
    {
        _distanceInMeterText = text;
        _distanceInMeterText.color = distanceMeterTextColor;
    }

    public void CalculateDistance()
    {
        if (Time.frameCount % frameInterval != 0) return;

        var pos = boxCollider.center;
        pos.y -= boxCollider.size.y / 2;
        _rayStartingPosition = transform.TransformPoint(pos);

        RaycastHit hit;
        Debug.DrawRay(_rayStartingPosition, Vector3.down, Color.magenta);
        Physics.Raycast(_rayStartingPosition, Vector3.down, out hit);

        if (hit.transform != null)
        {
            var distance = Vector3.Distance(_rayStartingPosition, hit.point);
            _distanceInMeterText.text = $"{distance:N2}m";
        }
    }
}