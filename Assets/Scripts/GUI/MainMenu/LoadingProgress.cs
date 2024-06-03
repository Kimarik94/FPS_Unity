using System.Collections.Generic;
using UnityEngine;

public class LoadingProgress : MonoBehaviour
{
    private List<GameObject> points = new List<GameObject>();

    private RectTransform rectTransform;
    public RectTransform parentPanel;

    public GameObject pointPrefab;

    public static bool isLoading;
    public static bool minimizeWindow = false;

    public float radius = 50f;
    public int numberOfPoints = 13;
    public float rotationSpeed = -80f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        SpanwPointsLoad();
        isLoading = true;
    }

    private void Update()
    {
        if (isLoading)
        {
            RotateAround();
        }
        if (minimizeWindow)
        {
            minimizeWindow = false;
            parentPanel.transform.localScale = Vector3.zero;

        }
    }

    private void SpanwPointsLoad()
    {
        for(int i = 0; i < numberOfPoints; i++)
        {
            float angle = i * Mathf.PI * 2f / numberOfPoints;
            Vector3 position = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

            GameObject point = Instantiate(pointPrefab, transform);
            points.Add(point);

            point.transform.localPosition = position;
        }
    }

    private void RotateAround()
    {
        rectTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
