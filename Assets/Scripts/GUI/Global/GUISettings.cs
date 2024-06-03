using UnityEngine;

public class GUISettings : MonoBehaviour
{
    Canvas canvas;

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        Camera mainCamera = transform.root.GetComponentInChildren<Camera>();
        if (mainCamera != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = mainCamera;
        }
        else
        {
            Debug.LogError("Main camera not found for GUISettings.");
        }
    }
}
