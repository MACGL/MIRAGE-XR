using UnityEngine;
using TMPro;
using System;

public class QRDistanceCheck : MonoBehaviour
{
    [SerializeField, Range(4, 60)]
    private int circleDensity;

    [SerializeField, Range(0, 5)]
    private int circleCount;

    [SerializeField, Range(0, 20)] private float _scanAngle;
    [SerializeField, Range(0, 2)] private float rayDistance;
    
    private RayResult[] raycasts = new RayResult[0];
    private int shortest = -1;
    private LineRenderer _lineRend;

    [SerializeField] private TMP_Text textDistance;

    private void Start()
    {
        _lineRend = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!_lineRend.enabled)
            _lineRend.enabled = true;

        textDistance.canvas.transform.position = transform.position + Vector3.up * .1f; 

        if ((circleCount * circleDensity + 1) * 2 != raycasts.Length)
            raycasts = new RayResult[(circleCount * circleDensity + 1) * 2];

        shortest = -1;

        RaycastDrawDebug(transform.position, transform.up, raycasts.Length / 2 - 1);
        RaycastDrawDebug(transform.position, -transform.up, raycasts.Length - 1);

        for (int j = 0; j < circleCount; j++)
        {
            Vector3 angleDirection = Vector3.RotateTowards(transform.up, transform.forward, (j + 1) / (float)circleCount * _scanAngle * Mathf.Deg2Rad, 0);

            for (int i = 0; i < circleDensity; i++)
            {
                Vector3 circleDirection = Quaternion.AngleAxis(i / (float)circleDensity * 360, transform.up) * angleDirection.normalized;

                RaycastDrawDebug(transform.position, circleDirection, i + j * circleDensity);
                RaycastDrawDebug(transform.position, -circleDirection, raycasts.Length / 2 + i + j * circleDensity);
            }
        }

        for (int i = 0; i < raycasts.Length; i++)
        {
            Color color = Color.red;
            if (i == shortest)
            {
                color = Color.green;
                textDistance.text = raycasts[i].trueDistance.ToString("0.0000");
                _lineRend.SetPositions(new Vector3[] { transform.position, raycasts[i].ray.origin + raycasts[i].ray.direction * raycasts[i].trueDistance });
                _lineRend.startColor = Color.green;
                _lineRend.endColor = Color.green;
            }
            Debug.DrawRay(raycasts[i].ray.origin, raycasts[i].ray.direction.normalized * rayDistance, color, 0);
        }

        if (shortest == -1)
        {
            textDistance.text = "0.0000";
            _lineRend.SetPositions(new Vector3[] { transform.position, transform.position + transform.up * rayDistance });
            _lineRend.startColor = Color.red;
            _lineRend.endColor = Color.red;
        }
    }

    private void RaycastDrawDebug(Vector3 position, Vector3 direction, int index)
    {
        Ray ray = new Ray(position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
            if (shortest < 0 || hit.distance < raycasts[shortest].trueDistance)
                shortest = index;
        Ray rayReverse = new Ray(position + direction * rayDistance, -direction);
        if (Physics.Raycast(rayReverse, out RaycastHit hitReverse, rayDistance))
            if (shortest < 0 || Mathf.Abs((position - hitReverse.point).magnitude) < raycasts[shortest].trueDistance)
                shortest = index;

        if(hit.distance > hitReverse.distance)
        {
            raycasts[index].ray = ray;
            raycasts[index].trueDistance = Mathf.Abs((position - hit.point).magnitude);
        }
        else
        {
            raycasts[index].ray = rayReverse;
            raycasts[index].trueDistance = Mathf.Abs((position - hitReverse.point).magnitude);
        }
    }
}

public struct RayResult
{
    public Ray ray;
    public float trueDistance;
}