using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DistanceMeter : MonoBehaviour
{
    [SerializeField] private int frameInterval;
    [SerializeField] private Color distanceMeterTextColor;
    private Vector3 _rayStartingPosition;
    private BoxCollider _boxCollider;
    private TextMeshProUGUI _distanceInMeterText;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
    }

    public void SetupDistanceMeterText(TextMeshProUGUI text)
    {
        _distanceInMeterText = text;
        _distanceInMeterText.color = distanceMeterTextColor;
    }

    public void CalculateDistance()
    {
        if (Time.frameCount % frameInterval != 0) return;

        _rayStartingPosition = transform.TransformPoint(_boxCollider.center);
        _rayStartingPosition.y -= _boxCollider.size.y / 2;

        RaycastHit hit;
        Physics.Raycast(_rayStartingPosition, Vector3.down, out hit);

        if (hit.transform != null)
        {
            var distance = Vector3.Distance(_rayStartingPosition, hit.point);
            _distanceInMeterText.text = $"{distance:N2}m";
        }
    }
}